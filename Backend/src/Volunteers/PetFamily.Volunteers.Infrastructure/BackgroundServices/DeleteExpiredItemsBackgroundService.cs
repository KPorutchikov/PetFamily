using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Volunteers.Infrastructure.Configurations;

namespace PetFamily.Volunteers.Infrastructure.BackgroundServices;

public class DeleteExpiredItemsBackgroundService: BackgroundService
{
    private readonly ILogger<DeleteExpiredItemsBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SoftDeleteOptions _softDeleteOptions;


    public DeleteExpiredItemsBackgroundService(
        ILogger<DeleteExpiredItemsBackgroundService> logger, 
        IServiceScopeFactory scopeFactory,
        IOptions<SoftDeleteOptions> softDeleteOptions)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _softDeleteOptions = softDeleteOptions.Value;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeleteExpiredItemsBackgroundService is starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();

            var deleteExpiredItems = scope.ServiceProvider.GetRequiredService<DeleteExpiredItems>();

            _logger.LogInformation("DeleteExpiredItemsBackgroundService is working");
            
            await deleteExpiredItems.ProcessAsync(stoppingToken);
            
            await Task.Delay(TimeSpan.FromMinutes(_softDeleteOptions.DeleteExpiredServiceTimeOutMinutes), stoppingToken);
        }
        
        _logger.LogInformation("DeleteExpiredItemsBackgroundService is stopping");
        
        await Task.CompletedTask;
    }
}