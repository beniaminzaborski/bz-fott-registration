using Bz.Fott.Registration.Application.Common;
using Bz.Fott.Registration.Infrastructure.Messaging;
using Bz.Fott.Registration.Infrastructure.Persistence;
using Bz.Fott.Registration.Infrastructure.Persistence.Common;
using FluentMigrator.Runner;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bz.Fott.Registration.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        return service
            .AddPersistence(configuration)
            .AddMessageBus(configuration);
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres");

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services
            .AddEntityFrameworkNpgsql()
            .AddDbContext<ApplicationDbContext>(
                opts => opts.UseNpgsql(connectionString))
            .AddScoped<IUnitOfWork>(x => x.GetRequiredService<ApplicationDbContext>())
            .AddDbMigrator(connectionString);

        // Register all repositories
        services
            .Scan(scan => scan.FromAssemblyOf<ApplicationDbContext>()
            .AddClasses(classes => classes.AssignableTo(typeof(Repository<,,>)))
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }

    private static IServiceCollection AddDbMigrator(this IServiceCollection services, string connectionString)
    {
        return services
            .AddLogging(c => c.AddFluentMigratorConsole())
            .AddFluentMigratorCore()
            .ConfigureRunner(
                opts =>
                {
                    opts
                        .AddPostgres()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(Assembly.GetExecutingAssembly()).For.All();
                });
    }

    private static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<IEntityNameFormatter, ShortTypeEntityNameFormatter>()
            .AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumers(typeof(IApplicationService).Assembly);

            x.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

                cfg.ConfigureEndpoints(context);

                cfg.MessageTopology.SetEntityNameFormatter(context.GetRequiredService<IEntityNameFormatter>());
            });
        });
    }
}
