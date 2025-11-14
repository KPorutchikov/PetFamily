using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application.AccountManagement.Queries.Users.GetUser;
using PetFamily.Accounts.Contracts;

namespace PetFamily.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services)
    {
        services.AddScoped<IAccountsContract,AccountsContract>();
        services.AddScoped<GetUserHandlerDapper>();
        
        return services;
    }
}