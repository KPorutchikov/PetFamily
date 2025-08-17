using CSharpFunctionalExtensions;
using PetFamily.Shared.Core.Shared;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Ids;
using PetFamily.Volunteers.Domain.ValueObjects;

namespace PetFamily.Volunteers.Domain.Models;

public class Volunteer : Entity<VolunteerId>
{
    private bool _isDeleted = false;
    private List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;

    // for EF Core
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    private Volunteer(
        VolunteerId id,
        FullName fullName,
        Email email,
        Description description,
        Phone phone,
        ExperienceInYears experienceInYears) : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Phone = phone;
        ExperienceInYears = experienceInYears;
    }

    public FullName FullName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public Phone Phone { get; private set; } = default!;
    public ExperienceInYears ExperienceInYears { get; private set; }
    public RequisiteDetails? RequisitesDetails { get; private set; }
    public int NumberOfPets => _pets.Count;
    public SocialNetworkDetails? SocialNetworkDetails { get; private set; }

    public int PetsFoundHome => _pets.Where(d => d.Status.Value == PetStatus.Status.FoundHome).Count();
    public int PetsSeekingHome => _pets.Where(d => d.Status.Value == PetStatus.Status.HomeSeeking).Count();
    public int PetsNeedHelp => _pets.Where(d => d.Status.Value == PetStatus.Status.NeedsHelp).Count();

    public void AddRequisiteDetails(RequisiteDetails requisitesDetails)
    {
        RequisitesDetails = requisitesDetails;
    }

    public void AddSocialNetworkDetails(SocialNetworkDetails socialNetworkDetails)
    {
        SocialNetworkDetails = socialNetworkDetails;
    }

    public UnitResult<Error> AddPet(Pet pet)
    {
        var serialNumberResult = SerialNumber.Create(_pets.Count + 1);
        if (serialNumberResult.IsFailure)
            return serialNumberResult.Error;

        pet.SetSerialNumber(serialNumberResult.Value);

        _pets.Add(pet);
        return Result.Success<Error>();
    }

    public UnitResult<Error> DeletePet(Guid petId)
    {
        foreach (var currentPet in _pets)
        {
            if (currentPet.Id == petId)
            {
                _pets.Remove(currentPet);
                
                return Result.Success<Error>();
            }
        }
        
        return Result.Success<Error>();
    }

    public UnitResult<Error> MovePet(Pet pet, SerialNumber serialNumber)
    {
        if (_pets.Count < serialNumber.Value)
            return Errors.General.ValueIsInvalid("serialNumber");

        int incriment = pet.SerialNumber.Value > serialNumber.Value ? -1 : 1;

        while (pet.SerialNumber.Value != serialNumber.Value)
        {
            _pets.FirstOrDefault(d => d.SerialNumber.Value == (pet.SerialNumber.Value + incriment))!
                .SetSerialNumber(SerialNumber.Create(pet.SerialNumber.Value).Value);

            pet.SetSerialNumber(SerialNumber.Create(pet.SerialNumber.Value + incriment).Value);
        }
        return Result.Success<Error>();
    }

    public void UpdateMainInfo(FullName fullName, Description description, Email email, Phone phone,
        ExperienceInYears experienceInYears)
    {
        FullName = fullName;
        Description = description;
        Email = email;
        Phone = phone;
        ExperienceInYears = experienceInYears;
    }

    public void SoftDelete()
    {
        if (_isDeleted == false)
        {
            _isDeleted = true;
            foreach (var pet in _pets)
            {
                pet.SoftDelete();
            }
        }
    }

    public void Restore()
    {
        if (_isDeleted)
        {
            _isDeleted = false;
            foreach (var pet in _pets)
            {
                pet.Restore();
            }
        }
    }

    public static Result<Volunteer, Error> Create(
        VolunteerId volunteerId,
        FullName fullName,
        Email email,
        Description description,
        Phone phone,
        ExperienceInYears experienceInYears)
    {
        return new Volunteer(volunteerId, fullName, email, description, phone, experienceInYears);
    }
}