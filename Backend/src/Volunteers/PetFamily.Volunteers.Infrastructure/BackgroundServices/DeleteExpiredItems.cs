using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetFamily.Volunteers.Infrastructure.Configurations;
using PetFamily.Volunteers.Infrastructure.Database;

namespace PetFamily.Volunteers.Infrastructure.BackgroundServices;

public class DeleteExpiredItems
{
    private readonly ILogger<DeleteExpiredItems> _logger;
    private readonly VolunteerDbContext _dbContext;
    private readonly SoftDeleteOptions _softDeleteOptions;


    public DeleteExpiredItems(ILogger<DeleteExpiredItems> logger, VolunteerDbContext dbContext, IOptions<SoftDeleteOptions> softDeleteOptions)
    {
        _logger = logger;
        _dbContext = dbContext;
        _softDeleteOptions = softDeleteOptions.Value;
    }

    public async Task ProcessAsync(CancellationToken stoppingToken)
    {
        // удалим отмеченых волонтеров (так же удалятся и их питомцы, по каскадному удалению)
        var volunteersIsDeleteItems = await _dbContext.Volunteers
            .Where(v => v.IsDeleted
                        && v.DeletionDate != null
                        && DateTime.UtcNow >= v.DeletionDate.Value.AddDays(_softDeleteOptions.ExpiredDaysToRemove).Date)
            .ExecuteDeleteAsync(stoppingToken);
        
        if (volunteersIsDeleteItems > 0)
            _logger.LogInformation($"Process is deleted {volunteersIsDeleteItems} rows of volunteers", volunteersIsDeleteItems);

        
        // удалим отмеченых питомцев (когда сам волонтер не помечен на удаление)
        var petsIsDeleteItems = await _dbContext.Pets
            .Where(p => p.IsDeleted == true
                        && p.DeletionDate != null 
                        && DateTime.UtcNow.Date >= p.DeletionDate.Value.Date.AddDays(_softDeleteOptions.ExpiredDaysToRemove).Date)
            .ExecuteDeleteAsync(stoppingToken);

        if (petsIsDeleteItems > 0)
            _logger.LogInformation($"Process is deleted {petsIsDeleteItems} rows of pets", petsIsDeleteItems);
        
        await Task.CompletedTask;
    }
}