using System;
using System.Linq;
using AppAny.HotChocolate.FluentValidation;
using DoodleForms.GraphQL.Common;
using DoodleForms.GraphQL.Forms.Mutations;
using FluentValidation;
using FluentValidation.AspNetCore;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DoodleForms.GraphQL;

public static class ServiceCollectionsExtensions
{
    public static IServiceCollection AddGraphQlModule(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services
            .AddGraphQLServer()
            .AddQueryType()
            .AddMutationType()
            .RegisterTypeModules()
            .AddAuthorization()
            .AddFluentValidation()
            .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = environment.IsDevelopment());

        return services
            .AddFluentValidation(o => o.ImplicitlyValidateChildProperties = true)
            .AddValidatorsFromAssemblyContaining<AddFormInputValidator>();
    }

    private static IRequestExecutorBuilder RegisterTypeModules(this IRequestExecutorBuilder builder)
    {
        var modules = typeof(IGraphQlTypeModule)
            .Assembly
            .GetTypes()
            .Where(t => t.IsClass && t.IsAssignableTo(typeof(IGraphQlTypeModule)))
            .Select(Activator.CreateInstance)
            .Cast<IGraphQlTypeModule>();

        foreach (var module in modules)
        {
            module.Register(builder);
        }

        return builder;
    }
}