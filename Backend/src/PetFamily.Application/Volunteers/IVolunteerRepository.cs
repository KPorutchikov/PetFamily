using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers;

public interface IVolunteerRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);

    Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Guid HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Task<Guid> SoftDelete(Volunteer volunteer, CancellationToken cancellationToken = default);
    
    Guid HardDeletePet(Pet volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken);

    Task<Result<Volunteer, Error>> GetByFullName(string fullName, CancellationToken cancellationToken);

    Task<Result<Pet, Error>> GetPetById(PetId petId, CancellationToken cancellationToken);
    
    Task<Result<Volunteer, Error>> GetByPetId(PetId petId, CancellationToken cancellationToken);
}