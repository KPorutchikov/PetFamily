using CSharpFunctionalExtensions;
using PetFamily.Accounts.Domain.Users;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.Accounts.Application;

public interface ITokenProvider
{
    Result<string, ErrorList> GenerateAccessToken(User user);
}