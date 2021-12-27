using DoodleForms.GraphQL.Common;
using DoodleForms.GraphQL.Questions.Mutations;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoodleForms.GraphQL.Questions;

public class QuestionTypeModule : IGraphQlTypeModule
{
    public IRequestExecutorBuilder Register(IRequestExecutorBuilder builder)
    {
        return builder
            .AddType<QuestionType>()
            .AddTypeExtension<AddQuestionMutation>()
            .AddTypeExtension<UpdateQuestionMutation>()
            .AddTypeExtension<DeleteQuestionMutation>()
            .AddDataLoader<QuestionDataLoader>();
    }
}