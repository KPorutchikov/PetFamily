using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;
namespace PetFamily.Domain.Volunteers;

public class Volunteer : Entity<VolunteerId>
{
    private List<Pet> _pets = [];
    public IReadOnlyList<Pet> Pets => _pets;

    // for EF Core
    private Volunteer(VolunteerId id) : base(id) { }
    
    private Volunteer(VolunteerId id, string fullName, string email, string description, string phone, 
                        int experienceInYears) : base(id)
    {
        FullName = fullName;
        Email = email;
        Description = description;
        Phone = phone;
        ExperienceInYears = experienceInYears;
    }
    public string FullName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Phone { get; private set; } = default!;
    public int? ExperienceInYears { get; private set; }
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
    
    public static Result<Volunteer> Create(VolunteerId volunteerId, string fullName, string email, string description, 
                                            string phone, int experienceInYears)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return Result.Failure<Volunteer>($"{nameof(fullName)} is not be empty");
            
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Volunteer>($"{nameof(email)} is not be empty");
        
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Volunteer>($"{nameof(description)} is not be empty");
        
        if (string.IsNullOrWhiteSpace(phone))
            return Result.Failure<Volunteer>($"{nameof(phone)} is not be empty");
        
        return Result.Success(new Volunteer(volunteerId, fullName, email, description, phone, experienceInYears));
    }
}