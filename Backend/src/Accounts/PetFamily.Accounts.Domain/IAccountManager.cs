using PetFamily.Accounts.Domain.Users;

namespace PetFamily.Accounts.Domain;

public interface IParticipantAccountManager
{
    Task CreateParticipantAccount(ParticipantAccount participantAccount);
    Task<string?> GetParticipantIfExists(User user);
}