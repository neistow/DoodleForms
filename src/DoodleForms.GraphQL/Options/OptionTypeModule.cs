using DoodleForms.GraphQL.Common;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoodleForms.GraphQL.Options;

public class OptionTypeModule : IGraphQlTypeModule
{
    public IRequestExecutorBuilder Register(IRequestExecutorBuilder builder)
    {
        return builder
            .AddType<OptionType>()
            .AddDataLoader<OptionDataLoader>();
    }
}