using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;

public record GetPetsWithPaginationQuery(
    Guid? Id, Guid? volunteer_id, string? name, string? description, Guid? species_id, 
    Guid? breed_id, string? color, string? address_city, string? address_street, 
    string? SortByColumns, int? Page, int? PageSize) : IQuery;