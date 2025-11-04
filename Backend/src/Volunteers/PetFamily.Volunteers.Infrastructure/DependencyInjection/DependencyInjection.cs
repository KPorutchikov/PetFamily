using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.MessageQueues;
using PetFamily.Shared.Core.Messaging;
using PetFamily.Shared.Core.Providers;
using PetFamily.Shared.SharedKernel;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Infrastructure.BackgroundServices;
using PetFamily.Volunteers.Infrastructure.Configurations;
using PetFamily.Volunteers.Infrastructure.Database;
using PetFamily.Volunteers.Infrastructure.Repositories;
using PetFamily.Volunteers.Presentation.DependencyInjection;

namespace PetFamily.Volunteers.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddVolunteersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();
        services.AddOptions<SoftDeleteOptions>();
        services.Configure<SoftDeleteOptions>(configuration.GetSection(SoftDeleteOptions.SOFT_DELETE));
        
        services
            .AddVolunteerDbContexts(configuration)
            .AddVolunteersRepositories()
            .AddHostedService()
            .AddVolunteersDatabase()
            .AddVolunteerPresentation()
            .AddMinio(configuration);
        
        return services;
    }
    
    private static IServiceCollection AddHostedService(this IServiceCollection services)
    {
        services.AddHostedService<FilesCleanerBackgroundService>();
        services.AddHostedService<DeleteExpiredItemsBackgroundService>();

        services.AddScoped<DeleteExpiredItems>();
        
        return services;
    }
    
    private static IServiceCollection AddVolunteerDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<VolunteerDbContext>(_ => new VolunteerDbContext(configuration.GetConnectionString("Database")!));
        
        return services;
    }
    
    private static IServiceCollection AddVolunteersDatabase(this IServiceCollection services)
    {
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Volunteers);
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        
        return services;
    }

    private static IServiceCollection AddVolunteersRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVolunteerRepository, VolunteerRepository >();

        return services;
    }

    private static IServiceCollection AddMinio(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection("Minio"));
        
        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection("Minio").Get<MinioOptions>()
                               ?? throw new ArgumentException("Missing minio configuration");
            
            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
}