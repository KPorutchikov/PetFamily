using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.SharedKernel;
using PetFamily.Species.Application;
using PetFamily.Species.Application.Commands.AddBreed;
using PetFamily.Species.Application.Commands.AddSpecies;
using PetFamily.Species.Application.SpeciesManagement.GetBreedsDapper;
using PetFamily.Species.Infrastructure.Database;
using PetFamily.Species.Infrastructure.Repositories;
using PetFamily.Species.Presentation.DependencyInjection;
using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetPet;
using PetFamily.Volunteers.Application.Volunteers.EditPet.DeletePet;
using PetFamily.Volunteers.Application.Volunteers.UpdatePetMainPhoto;

namespace PetFamily.Species.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddSpeciesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSpeciesDbContexts(configuration)
            .AddSpeciesRepositories()
            .AddSpeciesDatabase()
            .AddSpeciesModule();

        
        services.AddScoped<DeletePetSoftHandler>();
        services.AddScoped<DeletePetHardHandler>();
        services.AddScoped<UpdatePetMainPhotoHandler>();
        services.AddScoped<CreateSpeciesHandler>();
        services.AddScoped<AddBreedHandler>();
        
        services.AddScoped<GetPetHandlerDapper>();
        services.AddScoped<GetBreedHandlerDapper>();
        
        return services;
    }

    private static IServiceCollection AddSpeciesDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<SpeciesDbContext>(_ => new SpeciesDbContext(configuration.GetConnectionString("Database")));
        
        return services;
    }
    
    private static IServiceCollection AddSpeciesRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();

        return services;
    }
    
    private static IServiceCollection AddSpeciesDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Species);
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        return services;
    }
}