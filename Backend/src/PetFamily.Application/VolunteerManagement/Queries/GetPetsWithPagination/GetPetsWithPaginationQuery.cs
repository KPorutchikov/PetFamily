using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;

public record GetPetsWithPaginationQuery(
    string? Id, string? Name, string? Description, Guid? SpeciesId, Guid? BreedId, 
    string? SortBy, string? SortDirection, int? Page, int? PageSize) : IQuery;