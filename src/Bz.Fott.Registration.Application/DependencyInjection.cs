using Bz.Fott.Registration.Application.Common;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Bz.Fott.Registration.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        return service
            .AddMediatR(
                typeof(Application.Common.IUnitOfWork),
                typeof(Domain.Common.IDomainEvent))
            .AddDomainServices()
            .AddApplicationServices()
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddValidators();
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services
            .Scan(scan => scan.FromAssemblyOf<Domain.Common.IDomainService>()
            .AddClasses(classes => classes.AssignableTo<Domain.Common.IDomainService>())
            .AsMatchingInterface()
            .WithScopedLifetime());
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .Scan(scan => scan.FromAssemblyOf<IApplicationService>()
            .AddClasses(classes => classes.AssignableTo<IApplicationService>())
            .AsMatchingInterface()
            .WithScopedLifetime());
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services
            .Scan(scan => scan.FromAssemblyOf<IApplicationService>()
            .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
            .AsImplementedInterfaces(i => i.Name.StartsWith("IValidator") && i.IsGenericType)
            .WithScopedLifetime());

        return services;
    }
}
