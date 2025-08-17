using Microsoft.Extensions.DependencyInjection;
using PetFamily.Volunteers.Contracts;

namespace PetFamily.Volunteers.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteerPresentation(this IServiceCollection services)
    {
        services
            .AddContracts();
        
        return services;
    }
    
    private static IServiceCollection AddContracts(this IServiceCollection services)
    {
        return services.AddScoped<IVolunteersContract, VolunteersContract>();
    }
}