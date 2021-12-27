using System;
using System.Threading.Tasks;
using AppAny.HotChocolate.FluentValidation;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Common;
using DoodleForms.Infrastructure.Data;
using DoodleForms.Shared.Abstractions;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using QuestionTypeEnum = DoodleForms.Domain.Models.QuestionType;

namespace DoodleForms.GraphQL.Questions.Mutations;

public record AddQuestionInput(Guid FormId);

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AddQuestionMutation
{
    [Authorize]
    [UseDbContext(typeof(ApplicationDbContext))]
    public async Task<QuestionPayloadBase> AddQuestion(
        [UseFluentValidation] AddQuestionInput input,
        [ScopedService] ApplicationDbContext dbContext,
        [Service] ICurrentUser user)
    {
        var form = await dbContext.Forms.FindAsync(input.FormId);
        if (form == null)
        {
            return new QuestionPayloadBase(
                new PayloadError("Form not found", "ERRORS.NOT_FOUND")
            );
        }

        if (user.Id != form.CreatorId)
        {
            return new QuestionPayloadBase(
                new PayloadError("You can't edit this form", "ERRORS.NOT_AUTHORIZED")
            );
        }

        var question = new Question
        {
            QuestionType = QuestionTypeEnum.SingleChoice
        };
        form.Questions.Add(question);

        await dbContext.SaveChangesAsync();

        return new QuestionPayloadBase(question);
    }
}