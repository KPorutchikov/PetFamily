using Microsoft.Extensions.DependencyInjection;
using PetFamily.Species.Application.SpeciesManagement.GetBreedsDapper;
using PetFamily.Species.Application.SpeciesManagement.GetSpeciesDapper;
using PetFamily.Species.Contracts;
using PetFamily.Species.Presentation.Contracts;


namespace PetFamily.Species.Presentation.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesModule(this IServiceCollection services)
    {
        services.AddContracts();
        
        return services;
    }
   
    private static IServiceCollection AddContracts(this IServiceCollection services)
    {
        services.AddScoped<GetSpeciesHandlerDapper>();
        services.AddScoped<GetBreedHandlerDapper>();
        services.AddScoped<ISpeciesContract, SpeciesContract>();
        services.AddScoped<IBreedContract, BreedContract>();

        return services;
    }
    
}