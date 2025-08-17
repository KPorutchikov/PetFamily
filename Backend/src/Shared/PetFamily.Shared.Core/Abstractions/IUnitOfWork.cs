using System.Data;

namespace PetFamily.Shared.Core.Abstractions;

public interface IUnitOfWork
{
    Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default);
 
    Task SaveChanges(CancellationToken cancellationToken = default);
}