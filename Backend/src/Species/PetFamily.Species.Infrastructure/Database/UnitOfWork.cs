using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Species.Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly SpeciesDbContext _dbContext;
 
    public UnitOfWork(SpeciesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
 
    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
 
        return transaction.GetDbTransaction();
    }
 
    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}