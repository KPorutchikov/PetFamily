using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Infrastructure;
using PetFamily.Accounts.Infrastructure.Seeding;
using PetFamily.Accounts.Presentation;
using PetFamily.Shared.Framework.Authorization;
using PetFamily.Species.Application.DependencyInjection;
using PetFamily.Species.Infrastructure.DependencyInjection;
using PetFamily.Volunteers.Application.DependencyInjection;
using PetFamily.Volunteers.Infrastructure.DependencyInjection;
using PetFamily.Web.Middlewares;
using Serilog;
using Serilog.Events;

try
{
    DotNetEnv.Env.Load();
    
    var builder = WebApplication.CreateBuilder(args);
    
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq") ?? throw new ArgumentNullException("Seq"))
        .Enrich.FromLogContext()
        .Enrich.WithThreadId()
        .Enrich.WithThreadName()
        .Enrich.WithEnvironmentUserName()
        .Enrich.WithMachineName()
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Information)
        .CreateLogger();


    builder.Services
        .AddSpeciesApplication()
        .AddSpeciesInfrastructure(builder.Configuration)
        .AddVolunteersApplication()
        .AddVolunteersInfrastructure(builder.Configuration);

    
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "PetFamily", Version = "v1" });
    
        c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
        
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
        
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {   new OpenApiSecurityScheme { Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme,
                                                                              Id = "Bearer"}}, []
            }});
    });

    builder.Services.AddSerilog();

    builder.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
    builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
    
    builder.Services.AddAccountsApplication();
    builder.Services.AddAccountsPresentation();
    builder.Services.AddAccountsInfrastructure(builder.Configuration);
    
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    
    var app = builder.Build();

    var accountsSeeder = app.Services.GetRequiredService<AccountsSeeder>();
    await accountsSeeder.SeedAsync(new CancellationToken());
    
    app.UseExceptionMiddleware();
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        //await app.ApplyMigrations();
    }

    app.UseHttpsRedirection();
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllers();
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

namespace PetFamily.Web
{
    public partial class Program;
}