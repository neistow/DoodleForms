using System.Threading.Tasks;
using AppAny.HotChocolate.FluentValidation;
using DoodleForms.Domain.Models;
using DoodleForms.Infrastructure.Data;
using DoodleForms.Shared.Abstractions;
using FluentValidation;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;

namespace DoodleForms.GraphQL.Forms.Mutations;

public record AddFormInput(string Title = "New Form");

public class AddFormInputValidator : AbstractValidator<AddFormInput>
{
    public AddFormInputValidator()
    {
        RuleFor(i => i.Title).MaximumLength(512).NotEmpty();
    }
}

[ExtendObjectType(OperationTypeNames.Mutation)]
public class AddFormMutation
{
    [Authorize]
    [UseDbContext(typeof(ApplicationDbContext))]
    public async Task<FormPayloadBase> AddForm(
        [UseFluentValidation] AddFormInput input,
        [ScopedService] ApplicationDbContext dbContext,
        [Service] ICurrentUser user)
    {
        var form = new Form
        {
            Title = input.Title,
            CreatorId = user.Id!
        };

        dbContext.Forms.Add(form);
        await dbContext.SaveChangesAsync();

        return new FormPayloadBase(form);
    }
}