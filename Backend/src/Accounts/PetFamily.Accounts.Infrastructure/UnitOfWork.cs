using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Accounts.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AuthorizationDbContext _accountsDbContext;

    public UnitOfWork(AuthorizationDbContext accountsDbContext)
    {
        _accountsDbContext = accountsDbContext;
    }

    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _accountsDbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _accountsDbContext.SaveChangesAsync(cancellationToken);
    }
}