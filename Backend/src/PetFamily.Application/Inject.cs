using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Species.AddBreed;
using PetFamily.Application.Species.AddSpecies;
using PetFamily.Application.Species.DeleteBreed;
using PetFamily.Application.Species.DeleteSpecies;
using PetFamily.Application.SpeciesManagement.Queries.GetBreedsDapper;
using PetFamily.Application.SpeciesManagement.Queries.GetSpeciesDapper;
using PetFamily.Application.VolunteerManagement.Queries.GetPet;
using PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;
using PetFamily.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.AddPetPhotos;
using PetFamily.Application.Volunteers.Create;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.EditPet;
using PetFamily.Application.Volunteers.EditPet.DeletePet;
using PetFamily.Application.Volunteers.EditPet.UpdatePet;
using PetFamily.Application.Volunteers.EditPet.UpdatePetStatus;
using PetFamily.Application.Volunteers.UpdateMainInfo;
using PetFamily.Application.Volunteers.UpdatePetMainPhoto;
using PetFamily.Application.Volunteers.UpdateRequisiteDetails;
using PetFamily.Application.Volunteers.UpdateSocialNetwork;

namespace PetFamily.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateSocialNetworkHandler>();
        services.AddScoped<UpdateRequisiteDetailsHandler>();
        services.AddScoped<DeleteVolunteerSoftHandler>();
        services.AddScoped<DeleteVolunteerHardHandler>();
        services.AddScoped<AddPetHandler>();
        services.AddScoped<AddPetPhotosHandler>();
        services.AddScoped<MovePetHandler>();
        services.AddScoped<GetVolunteersWithPaginationHandlerDapper>();
        services.AddScoped<GetPetsWithPaginationHandlerDapper>();
        services.AddScoped<GetPetHandlerDapper>();
        services.AddScoped<GetBreedHandlerDapper>();
        services.AddScoped<DeleteBreedHandler>();
        services.AddScoped<DeleteSpeciesHandler>();
        services.AddScoped<GetSpeciesHandlerDapper>();
        services.AddScoped<UpdatePetHandler>();
        services.AddScoped<UpdatePetStatusHandler>();
        services.AddScoped<DeletePetSoftHandler>();
        services.AddScoped<DeletePetHardHandler>();
        services.AddScoped<UpdatePetMainPhotoHandler>();
        services.AddScoped<CreateSpeciesHandler>();
        services.AddScoped<AddBreedHandler>();
        

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}