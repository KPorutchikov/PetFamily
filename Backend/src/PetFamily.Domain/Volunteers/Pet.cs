using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Domain.Volunteers;

public class Pet : Entity<PetId>
{
    private bool _isDeleted = false;
    public string Name { get; private set; }
    public PetBreed Breed { get; private set; }
    public string Description { get; private set; }
    public string Color { get; private set; }
    public float Weight { get; private set; }
    public float Height { get; private set; }
    public string HealthInformation { get; private set; }
    public Address Address { get; private set; }
    public string Phone { get; private set; }
    public bool IsCastrated { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public bool IsVaccinated { get; private set; }
    public PetStatus Status { get; private set; }
    public SerialNumber SerialNumber { get; private set; }
    public RequisiteDetails? RequisitesDetails { get; private set; }

    public DateTime CreatedDate = DateTime.Now.ToUniversalTime();
    public string? PetPhoto  { get; private set; }
    public ValueObjectList<PetFile>? Files { get; private set; }

    // for EF Core
    private Pet(PetId id) : base(id)
    {
    }

    private Pet(PetId id, string name, PetBreed breed, string description, string color, float height,
        float weight, string healthInformation, Address address, string phone, bool isCastrated,
        DateOnly birthDate, bool isVaccinated, PetStatus status) : base(id)
    {
        Name = name;
        Breed = breed;
        Description = description;
        Color = color;
        Height = height;
        Weight = weight;
        HealthInformation = healthInformation;
        Address = address;
        Phone = phone;
        IsCastrated = isCastrated;
        BirthDate = birthDate;
        IsVaccinated = isVaccinated;
        Status = status;
    }

    public void UpdatePetPhoto(string pathToPhoto) => PetPhoto = pathToPhoto;
    
    public void UpdateFilesList(ValueObjectList<PetFile> files) =>
        Files = files;

    public void UpdateMain(string name, PetBreed breed, string description, string color, float weight, float height,
        string healthInformation, Address address, string phone, bool isCastrated, DateOnly birthDate, bool isVaccinated, PetStatus status)
    {
        Name = name;
        Breed = breed;
        Description = description;
        Color = color;
        Weight = weight;
        Height = height;
        HealthInformation = healthInformation;
        Address = address;
        Phone = phone;
        IsCastrated = isCastrated;  
        BirthDate = birthDate;
        IsVaccinated = isVaccinated;    
        Status = status;
    }
    
    public void AddRequisiteDetails(RequisiteDetails requisitesDetails)
    {
        RequisitesDetails = requisitesDetails;
    }
    
    public void SetStatus(PetStatus status)
    {
        Status = status;
    }
    
    public void SetSerialNumber(SerialNumber serialNumber)
    {
        SerialNumber = serialNumber;
    }

    public void SoftDelete()
    {
        _isDeleted = true;
    }

    public void Restore()
    {
        if (_isDeleted)
            _isDeleted = false;
    }

    public static Result<Pet, Error> Create(PetId id, string name, PetBreed breed, string description, string color,
        float height, float weight, string healthInformation, Address address, string phone, bool isCastrated,
        DateOnly birthDate, bool isVaccinated, PetStatus status)
    {
        if (string.IsNullOrWhiteSpace(name)) return Errors.General.ValueIsInvalid("name");
        if (weight <= 0) return Errors.General.ValueIsInvalid("weight");
        if (height <= 0) return Errors.General.ValueIsInvalid("height");

        return new Pet(id, name, breed, description, color, height, weight, healthInformation,
            address, phone, isCastrated, birthDate, isVaccinated, status);
    }
}