using Bz.Fott.Registration.WebAPI.ExceptionsHandling;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Azure.Monitor.OpenTelemetry.Exporter;

namespace Bz.Fott.Registration.WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(GlobalExceptionFilter));
            options.SuppressAsyncSuffixInActionNames = false;
        });

        return services;
    }

    public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration config, string serviceName, string serviceVersion)
    {
        var appInsightsConnectionString = config.GetConnectionString("ApplicationInsights");

        return services
            .AddOpenTelemetry()
            .WithTracing(builder => builder
                .AddSource(serviceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion: serviceVersion))
                .AddAspNetCoreInstrumentation()
                .AddNpgsql()
                .AddMassTransitInstrumentation().AddSource("MassTransit")
                .AddConsoleExporter()
                .AddAzureMonitorTraceExporter(cfg => cfg.ConnectionString = appInsightsConnectionString))
            .WithMetrics(builder => builder
                .AddMeter(serviceName)
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion: serviceVersion))
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter()
                .AddAzureMonitorMetricExporter(cfg => cfg.ConnectionString = appInsightsConnectionString))
            .StartWithHost()
            .Services;
    }
}
