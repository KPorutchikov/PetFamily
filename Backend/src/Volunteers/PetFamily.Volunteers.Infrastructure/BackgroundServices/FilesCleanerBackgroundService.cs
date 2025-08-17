using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Messaging;
using PetFamily.Shared.Core.Providers;
using FileInfo = PetFamily.Shared.Core.FileProvider.FileInfo;

namespace PetFamily.Volunteers.Infrastructure.BackgroundServices;

public class FilesCleanerBackgroundService : BackgroundService
{
    private readonly ILogger<FilesCleanerBackgroundService> _logger;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;
    private readonly IServiceScopeFactory _scopeFactory;

    public FilesCleanerBackgroundService(
        ILogger<FilesCleanerBackgroundService> logger,
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IServiceScopeFactory scopeFactory
    )
    {
        _logger = logger;
        _messageQueue = messageQueue;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("FilesCleanerBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var fileInfos = await _messageQueue.ReadAsync(stoppingToken);
            
            await using var scope = _scopeFactory.CreateAsyncScope();

            var fileProvider = scope.ServiceProvider.GetRequiredService<IFileProvider>();

            foreach (var fileInfo in fileInfos)
            {
                await fileProvider.RemoveFile(fileInfo, stoppingToken);
            }
        }

        await Task.CompletedTask;
    }
}