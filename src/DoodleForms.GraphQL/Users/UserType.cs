using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoodleForms.Domain.Models;
using DoodleForms.GraphQL.Forms;
using DoodleForms.Infrastructure.Data;
using DoodleForms.Shared.Abstractions;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

namespace DoodleForms.GraphQL.Users;

public class User
{
    public Guid Id { get; set; }
    public ICollection<Form> Forms { get; set; } = new List<Form>();
}

public class UserType : ObjectType<User>
{
    protected override void Configure(IObjectTypeDescriptor<User> descriptor)
    {
        descriptor.Authorize();

        descriptor.Field(f => f.Forms)
            .ResolveWith<UserResolvers>(r => r.GetForms(default!, default!, default!));
    }

    private class UserResolvers
    {
        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<IEnumerable<Form>> GetForms(
            [ScopedService] ApplicationDbContext dbContext,
            [Service] FormDataLoader loader,
            [Service] ICurrentUser user)
        {
            var formIds = await dbContext.Forms
                .Where(f => f.CreatorId == user.Id)
                .Select(f => f.Id)
                .ToListAsync();

            return await loader.LoadAsync(formIds);
        }
    }
}