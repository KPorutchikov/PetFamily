using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.ValueObjects;
namespace PetFamily.Domain.Volunteers;
public class Pet : Entity<PetId>
{
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
    public Requisites Requisites { get; private set; }
    public DateTime CreatedDate = DateTime.Now;
    
    private Pet(PetId id, string name, PetBreed breed, string description, string color, float height, 
                float weight, string healthInformation, Address address, string phone, bool isCastrated,
                DateOnly birthDate, bool isVaccinated, PetStatus status, Requisites requisites) : base(id)
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
        Requisites = requisites;
    }

    public static Result<Pet> Create(PetId id, string name, PetBreed breed, string description, string color, float height, 
        float weight, string healthInformation, Address address, string phone, bool isCastrated,
        DateOnly birthDate, bool isVaccinated, PetStatus status, Requisites requisites)
    {
        if (string.IsNullOrWhiteSpace(name)) return Result.Failure<Pet>($"Pet name is not be empty");
        if (weight <= 0) return Result.Failure<Pet>("Pet weight should be more than zero");
        if (height <= 0) return Result.Failure<Pet>("Pet height should be more than zero");
        
        return Result.Success(new Pet(id, name, breed, description, color, height, weight, healthInformation, 
                                    address, phone, isCastrated, birthDate, isVaccinated, status, requisites));
    }
}