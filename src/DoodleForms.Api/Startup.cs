using DoodleForms.Api.Common;
using DoodleForms.GraphQL;
using DoodleForms.Infrastructure;
using DoodleForms.Shared.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DoodleForms.Api;

public class Startup
{
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        Configuration = configuration;
        Environment = environment;
    }

    private IConfiguration Configuration { get; }
    private IWebHostEnvironment Environment { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddCors(a => a
            .AddDefaultPolicy(pb => pb
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
            )
        );

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerUrl"];
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.ValidTypes = new[] {"at+jwt"};
            });
        services.AddAuthorization();

        services.AddInfrastructure(Configuration);
        services.AddGraphQlModule(Configuration, Environment);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging();

        app.UseRouting();
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGraphQL();
            endpoints.MapGraphQLSchema();
        });
    }
}