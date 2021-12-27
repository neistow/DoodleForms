using System;
using System.Threading.Tasks;
using AppAny.HotChocolate.FluentValidation;
using DoodleForms.GraphQL.Common;
using DoodleForms.Infrastructure.Data;
using DoodleForms.Shared.Abstractions;
using FluentValidation;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace DoodleForms.GraphQL.Questions.Mutations;

public record DeleteQuestionInput(Guid QuestionId);

public class DeleteQuestionInputValidator : AbstractValidator<DeleteQuestionInput>
{
    public DeleteQuestionInputValidator()
    {
        RuleFor(i => i.QuestionId).NotEmpty();
    }
}

[ExtendObjectType(OperationTypeNames.Mutation)]
public class DeleteQuestionMutation
{
    [Authorize]
    [UseDbContext(typeof(ApplicationDbContext))]
    public async Task<QuestionPayloadBase> DeleteQuestion(
        [UseFluentValidation] DeleteQuestionInput input,
        [ScopedService] ApplicationDbContext dbContext,
        [Service] ICurrentUser user)
    {
        var question = await dbContext.Questions
            .Include(q => q.Form)
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
                new PayloadError("You can't delete this question", "ERRORS.NOT_AUTHORIZED")
            );
        }

        dbContext.Questions.Remove(question);
        await dbContext.SaveChangesAsync();

        return new QuestionPayloadBase();
    }
}