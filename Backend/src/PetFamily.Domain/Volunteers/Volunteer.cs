using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Domain.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
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

    public void AddPet(Pet pet)
    {
        _pets.Add(pet);
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