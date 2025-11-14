using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.Core.Dtos;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application;

public interface IAccountsRepository
{
    Task<Result<User, Error>> GetUserByUserId(Guid userId, CancellationToken cancellationToken);
}