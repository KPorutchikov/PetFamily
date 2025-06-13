using PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetPetsWithPaginationRequest(
    Guid? PetId,
    string? Name,
    string? Description,
    Guid? SpeciesId,
    Guid? BreedId,
    string? SortBy,
    string? SortDirection,
    int? Page,
    int? PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() =>
        new GetPetsWithPaginationQuery(PetId, Name, Description, SpeciesId, BreedId, SortBy, SortDirection, Page, PageSize);
}