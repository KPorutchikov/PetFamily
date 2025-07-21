using System.Data.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NSubstitute;
using PetFamily.Application.Constants;
using PetFamily.Application.Database;
using PetFamily.Application.FileProvider;
using PetFamily.Application.Providers;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure;
using Respawn;
using Testcontainers.PostgreSql;
using Xunit;
using FileInfo = PetFamily.Application.FileProvider.FileInfo;

namespace PetFamily.Application.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly IFileProvider _fileProviderMock = Substitute.For<IFileProvider>();
    
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_family_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;
    
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }
    
    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var writeContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(ApplicationDbContext));

        if (writeContext is not null)
            services.Remove(writeContext);
        
        services.AddScoped<ApplicationDbContext>(_ =>
            new ApplicationDbContext(_dbContainer.GetConnectionString()));
        
        var readContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(ISqlConnectionFactory));
        
        if (readContext is not null)
            services.Remove(readContext);

        services.AddSingleton<ISqlConnectionFactory>(_ => 
            new SqlConnectionFactoryTest(_dbContainer.GetConnectionString()));
        
        var fileProvider = services.SingleOrDefault(s => 
            s.ServiceType == typeof(IFileProvider));
        
        if (fileProvider is not null)
            services.Remove(fileProvider);

        services.AddScoped<IFileProvider>(_ => _fileProviderMock);

    }

    public void SetupSuccessFileProviderMock(FilePath filePath)
    {
        _fileProviderMock
            .UploadFiles(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<IReadOnlyList<FilePath>, Error>([filePath]));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}