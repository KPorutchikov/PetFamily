using PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetPetsWithPagination;

namespace PetFamily.Volunteers.Presentation.Pets.Requests;

public record GetPetsWithPaginationRequest(
    Guid? PetId,
    Guid? VolunteerId,
    string? Name,
    string? Description,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Color,
    string? AddressCity,
    string? AddressStreet,
    string? SortByColumns,
    int? Page,
    int? PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() =>
        new GetPetsWithPaginationQuery(PetId, VolunteerId, Name, Description, SpeciesId, BreedId, 
                                        Color, AddressCity, AddressStreet, SortByColumns, Page, PageSize);
}