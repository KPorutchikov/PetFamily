using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.Delete;
using PetFamily.Application.Volunteers.UpdateMainInfo;
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
        services.AddScoped<DeleteVolunteerHandler>();

        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}