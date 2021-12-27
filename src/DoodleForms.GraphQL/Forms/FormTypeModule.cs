using DoodleForms.GraphQL.Common;
using DoodleForms.GraphQL.Forms.Mutations;
using DoodleForms.GraphQL.Forms.Queries;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoodleForms.GraphQL.Forms;

public class FormTypeModule : IGraphQlTypeModule
{
    public IRequestExecutorBuilder Register(IRequestExecutorBuilder builder)
    {
        return builder
            .AddType<FormType>()
            .AddTypeExtension<AddFormMutation>()
            .AddTypeExtension<UpdateFormMutation>()
            .AddTypeExtension<DeleteFormMutation>()
            .AddTypeExtension<GetFormQuery>()
            .AddDataLoader<FormDataLoader>();
    }
}