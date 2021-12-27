using DoodleForms.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoodleForms.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration
    )
    {
        services.AddPooledDbContextFactory<ApplicationDbContext>(o =>
            o.UseNpgsql(configuration.GetConnectionString("postgres"))
        );

        return services;
    }
}