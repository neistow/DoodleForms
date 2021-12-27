using DoodleForms.GraphQL.Common;
using DoodleForms.GraphQL.Users.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoodleForms.GraphQL.Users;

public class UserTypeModule : IGraphQlTypeModule
{
    public IRequestExecutorBuilder Register(IRequestExecutorBuilder builder)
    {
        return builder
            .AddType<UserType>()
            .AddTypeExtension<GetUserQuery>();
    }
}