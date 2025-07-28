using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;

public record GetPetsWithPaginationQuery(
    Guid? PetId, Guid? VolunteerId, string? Name, string? Description, Guid? SpeciesId, 
    Guid? BreedId, string? Color, string? AddressCity, string? AddressStreet, 
    string? SortByColumns, int? Page, int? PageSize) : IQuery;