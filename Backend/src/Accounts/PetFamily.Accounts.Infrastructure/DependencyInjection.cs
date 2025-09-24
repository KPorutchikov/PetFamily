using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Application.Commands.Login;
using PetFamily.Accounts.Domain.Roles;
using PetFamily.Accounts.Domain.Users;

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
         services.AddTransient<ITokenProvider, JwtTokenProvider>();
         services.AddScoped<LoginHandler>();

         services.AddScoped<AuthorizationDbContext>();
         services.RegisterIdentity(configuration);
         services.AddAuthorization();
         
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
        
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuer = jwtOptions.Issuer,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                     ValidateIssuer = true,
                     ValidateAudience = false,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true
                 };
             });

         services.Configure<IdentityOptions>(options =>
         {
             options.Password.RequireDigit = false;
             options.Password.RequireNonAlphanumeric = false;
             options.User.RequireUniqueEmail = true;
         });
     }
}