using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Domain.Roles;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Accounts.Infrastructure.IdentityManagers;
using PetFamily.Accounts.Infrastructure.Options;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Framework.Factory;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddOptions<JwtOptions>();
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        services.AddOptions<RefreshTokenOptions>();
        services.Configure<RefreshTokenOptions>(configuration.GetSection(RefreshTokenOptions.REFRESH_SESSION));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));
        services.AddTransient<ITokenProvider, JwtTokenProvider>();
        services.AddScoped<LoginHandler>();
        services.AddScoped<IParticipantAccountManager, ParticipantAccountManager>();
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);
        services.AddScoped<IAccountsRepository, AccountsRepository>();

        services.AddScoped<AuthorizationDbContext>();
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireClaim("Role", "User")
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("RequireAdminRole", policy =>
            {
                policy.RequireClaim("Role", "Admin");
                policy.RequireAuthenticatedUser();
            });
        });

        services.RegisterIdentity(configuration);
        
        return services;
    }

    private static void RegisterIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing JWT configuration");

                options.TokenValidationParameters = TokenValidationParametersFactory.CreateWithLifeTime(jwtOptions);
            });

        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<AdminAccountManager>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        });
    }
   
}