using System;
using DoodleForms.Shared.Abstractions;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;

namespace DoodleForms.GraphQL.Users.Queries;

[ExtendObjectType(OperationTypeNames.Query)]
public class GetUserQuery
{
    [Authorize]
    public User GetUser([Service] ICurrentUser user)
    {
        return new User
        {
            Id = Guid.Parse(user.Id!)
        };
    }
}