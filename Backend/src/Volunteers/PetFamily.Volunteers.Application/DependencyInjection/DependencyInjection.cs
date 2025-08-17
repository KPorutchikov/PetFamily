using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.MessageQueues;
using PetFamily.Shared.Core.Messaging;

namespace PetFamily.Volunteers.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersApplication(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IMessageQueue<>), typeof(InMemoryMessageQueue<>));
        
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes.AssignableToAny([typeof(ICommandHandler<>), typeof(ICommandHandler<,>)]))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}