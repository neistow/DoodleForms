using System;
using System.Threading.Tasks;
using DoodleForms.Domain.Models;
using DoodleForms.Infrastructure.Data;
using DoodleForms.Shared.Abstractions;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;

namespace DoodleForms.GraphQL.Forms.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class GetFormQuery
{
    [UseDbContext(typeof(ApplicationDbContext))]
    public async Task<Form?> GetForm(
        Guid id,
        [ScopedService] ApplicationDbContext dbContext,
        [Service] ICurrentUser user)
    {
        var form = await dbContext.Forms.FindAsync(id);
        if (form == null || form.AcceptAnswers)
        {
            return form;
        }

        return form.CreatorId == user.Id ? form : null;
    }
}