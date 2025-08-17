using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly VolunteerDbContext _dbContext;
 
    public UnitOfWork(VolunteerDbContext dbContext)
    {
        _dbContext = dbContext;
    }
 
    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
 
        return transaction.GetDbTransaction();
    }
 
    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}