using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Application;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.Models;
using PetFamily.Volunteers.Infrastructure.Database;

namespace PetFamily.Volunteers.Infrastructure.Repositories;

public class VolunteerRepository : IVolunteerRepository
{
    private readonly VolunteerDbContext _dbContext;

    public VolunteerRepository(VolunteerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Volunteers.AddAsync(volunteer, cancellationToken);

        return volunteer.Id;
    }

    public Guid Save(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Attach(volunteer);

        return volunteer.Id;
    }

    public Guid SoftDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        volunteer.Delete();

        return volunteer.Id;
    }

    public Guid HardDelete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        _dbContext.Volunteers.Remove(volunteer);

        _dbContext.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }
    
    public Guid HardDeletePet(Pet pet, CancellationToken cancellationToken = default)
    {
        _dbContext.Pets.Remove(pet);

        return pet.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId volunteerId,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(p => p.Pets)
            .FirstOrDefaultAsync(v => v.Id == volunteerId, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(volunteerId);

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetByFullName(string fullName,
        CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Include(p => p.Pets)
            .FirstOrDefaultAsync(v => v.FullName.Value == fullName, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        return volunteer;
    }

    public async Task<Result<Pet, Error>> GetPetById(PetId petId, CancellationToken cancellationToken = default)
    {
        var pet = await _dbContext.Volunteers
            .Where(x => x.Pets.Any(p => p.Id == petId))
            .Include(p => p.Pets)
            .Select(p => p.Pets.FirstOrDefault(x => x.Id == petId))
            .FirstOrDefaultAsync(cancellationToken);

        if (pet == null)
            return Errors.General.NotFound(petId);

        return pet;
    }

    public async Task<Result<Volunteer, Error>> GetByPetId(PetId petId, CancellationToken cancellationToken = default)
    {
        var volunteer = await _dbContext.Volunteers
            .Where(x => x.Pets.Any(y => y.Id == petId))
            .Include(p => p.Pets)
            .FirstOrDefaultAsync(cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(petId);

        return volunteer;
    }
}