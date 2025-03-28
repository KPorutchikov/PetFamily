using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Application.Volunteers;

public interface IVolunteerRepository
{
    Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Guid> Save(Volunteer volunteer, CancellationToken cancellationToken = default);

    Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId, CancellationToken cancellationToken);

    Task<Result<Volunteer, Error>> GetByFullName(string fullName, CancellationToken cancellationToken);
    
}