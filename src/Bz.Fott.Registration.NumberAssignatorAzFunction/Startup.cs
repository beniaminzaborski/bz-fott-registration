using Bz.Fott.Registration.Application.CompetitorRegistration;
using Bz.Fott.Registration.NumberAssignatorAzFunction;
using Bz.Fott.Registration.NumberAssignatorAzFunction.Consumers;
using MassTransit;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Bz.Fott.Registration.NumberAssignatorAzFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services
            .AddScoped<ICompetitorService, CompetitorService>()
            .AddScoped<NumberAssignatorFunction>()
            .AddMassTransitForAzureFunctions(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<ConsumerNamespace>();
                cfg.AddRequestClient<RegisterCompetitor>(new Uri($"queue:{NumberAssignatorFunction.QueueName}"));
            },
            "ServiceBusConnectionString");
    }
}
