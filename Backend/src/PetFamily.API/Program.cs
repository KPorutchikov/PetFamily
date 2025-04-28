using Microsoft.OpenApi.Models;
using PetFamily.API.Middlewares;
using PetFamily.API.Validation;
using PetFamily.Application;
using PetFamily.Infrastructure;
using Serilog;
using Serilog.Events;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

try
{
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
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
    });

    builder.Services.AddSerilog();

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddFluentValidationAutoValidation(configuration =>
    {
        configuration.OverrideDefaultResultFactoryWith<CustomResultFactory>();
    });

    var app = builder.Build();

    app.UseExceptionMiddleware();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        //await app.ApplyMigrations();
    }

    app.UseHttpsRedirection();
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