using System;
using System.Linq;
using System.Threading.Tasks;
using AppAny.HotChocolate.FluentValidation;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Common;
using DoodleForms.Infrastructure.Data;
using DoodleForms.Shared.Abstractions;
using FluentValidation;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using QuestionTypeEnum = DoodleForms.Domain.Models.QuestionType;

namespace DoodleForms.GraphQL.Questions.Mutations;

public record UpdateQuestionInput(
    Guid QuestionId,
    string? Text = null,
    bool Required = false,
    QuestionTypeEnum QuestionType = QuestionTypeEnum.Open,
    AddOptionInput[]? Options = null
);

public record AddOptionInput(string Text);

public class UpdateQuestionInputValidator : AbstractValidator<UpdateQuestionInput>
{
    public UpdateQuestionInputValidator()
    {
        RuleFor(i => i.QuestionId).NotEmpty();
        RuleFor(i => i.Text).MaximumLength(8192);
        RuleFor(i => i.QuestionType).IsInEnum();
    }
}

public class UpdateOptionInputValidator : AbstractValidator<AddOptionInput>
{
    public UpdateOptionInputValidator()
    {
        RuleFor(i => i.Text).MaximumLength(2048);
    }
}

[ExtendObjectType(OperationTypeNames.Mutation)]
public class UpdateQuestionMutation
{
    [Authorize]
    [UseDbContext(typeof(ApplicationDbContext))]
    public async Task<QuestionPayloadBase> UpdateQuestion(
        [UseFluentValidation] UpdateQuestionInput input,
        [ScopedService] ApplicationDbContext dbContext,
        [Service] ICurrentUser user)
    {
        var question = await dbContext.Questions
            .Include(q => q.Form)
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.Id == input.QuestionId);
        if (question == null)
        {
            return new QuestionPayloadBase(
                new PayloadError("Question not found", "ERRORS.NOT_FOUND")
            );
        }

        if (question.Form?.CreatorId != user.Id)
        {
            return new QuestionPayloadBase(
                new PayloadError("You can't edit this question", "ERRORS.NOT_AUTHORIZED")
            );
        }

        question.Text = input.Text;
        question.Required = input.Required;
        question.QuestionType = input.QuestionType;
        if (input.Options != null)
        {
            dbContext.RemoveRange(question.Options);
            question.Options = input.Options.Select(i => new Option {Text = i.Text}).ToList();
        }

        await dbContext.SaveChangesAsync();

        return new QuestionPayloadBase(question);
    }
}