namespace PetFamily.Volunteers.Contracts.Requests;

public record PetExistsRequest(Guid? PetId, Guid? VolunteerId, string? Name, string? Description, 
    Guid? SpeciesId, Guid? BreedId, string? Color, string? AddressCity, string? AddressStreet);