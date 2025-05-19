using PetFamily.Application.Abstractions;
using PetFamily.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetVolunteersWithPaginationRequest(
    string? Id,
    string? FullName,
    int Page,
    int PageSize,
    string? SortBy,
    string? SortDirection)
{
    public GetVolunteersWithPaginationQuery ToQuery() =>
        new GetVolunteersWithPaginationQuery(Id, FullName, SortBy, SortDirection, Page, PageSize);
}