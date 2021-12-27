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

namespace DoodleForms.GraphQL.Forms.Mutations;

public record UpdateFormInput(
    Guid Id,
    string Title,
    string? Description,
    bool AcceptAnswers = true
);

public class UpdateFormInputValidator : AbstractValidator<UpdateFormInput>
{
    public UpdateFormInputValidator()
    {
        RuleFor(i => i.Id).NotEmpty();
        RuleFor(i => i.Title).MaximumLength(512).NotEmpty();
        RuleFor(i => i.Description).MaximumLength(2048);
    }
}

[ExtendObjectType(OperationTypeNames.Mutation)]
public class UpdateFormMutation
{
    [Authorize]
    [UseDbContext(typeof(ApplicationDbContext))]
    public async Task<FormPayloadBase> UpdateForm(
        [UseFluentValidation] UpdateFormInput input,
        [ScopedService] ApplicationDbContext dbContext,
        [Service] ICurrentUser user)
    {
        var form = await dbContext.Forms.FindAsync(input.Id);
        if (form == null)
        {
            return new FormPayloadBase(
                new PayloadError("Form not found", "ERRORS.NOT_FOUND")
            );
        }

        if (user.Id != form.CreatorId)
        {
            return new FormPayloadBase(
                new PayloadError("You can't edit this form", "ERRORS.NOT_AUTHORIZED")
            );
        }

        form.Title = input.Title;
        form.Description = input.Description;
        form.AcceptAnswers = input.AcceptAnswers;

        await dbContext.SaveChangesAsync();

        return new FormPayloadBase(form);
    }
}