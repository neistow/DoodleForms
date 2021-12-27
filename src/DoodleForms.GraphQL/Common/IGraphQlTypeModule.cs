using HotChocolate.Execution.Configuration;

namespace DoodleForms.GraphQL.Common;

public interface IGraphQlTypeModule
{
    IRequestExecutorBuilder Register(IRequestExecutorBuilder builder);
}