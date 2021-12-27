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

public record DeleteFormInput(Guid FormId);

public class DeleteFormInputValidator : AbstractValidator<DeleteFormInput>
{
    public DeleteFormInputValidator()
    {
        RuleFor(i => i.FormId).NotEmpty();
    }
}

[ExtendObjectType(OperationTypeNames.Mutation)]
public class DeleteFormMutation
{
    [Authorize]
    [UseDbContext(typeof(ApplicationDbContext))]
    public async Task<FormPayloadBase> DeleteForm(
        [UseFluentValidation] DeleteFormInput input,
        [ScopedService] ApplicationDbContext dbContext,
        [Service] ICurrentUser user)
    {
        var form = await dbContext.Forms.FindAsync(input.FormId);
        if (form == null)
        {
            return new FormPayloadBase(
                new PayloadError("Form not found", "ERRORS.NOT_FOUND")
            );
        }

        if (form.CreatorId != user.Id)
        {
            return new FormPayloadBase(
                new PayloadError("You can't delete this form", "ERRORS.NOT_AUTHORIZED")
            );
        }

        dbContext.Forms.Remove(form);
        await dbContext.SaveChangesAsync();

        return new FormPayloadBase();
    }
}