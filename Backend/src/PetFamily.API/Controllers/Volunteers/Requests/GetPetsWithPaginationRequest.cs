using PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetPetsWithPaginationRequest(
    Guid? pet_Id,
    Guid? volunteer_id,
    string? name,
    string? description,
    Guid? species_id,
    Guid? breed_id,
    string? color,
    string? address_city,
    string? address_street,
    string? SortByColumns,
    int? Page,
    int? PageSize)
{
    public GetPetsWithPaginationQuery ToQuery() =>
        new GetPetsWithPaginationQuery(pet_Id, volunteer_id, name, description, species_id, breed_id, 
                                        color, address_city, address_street, SortByColumns, Page, PageSize);
}