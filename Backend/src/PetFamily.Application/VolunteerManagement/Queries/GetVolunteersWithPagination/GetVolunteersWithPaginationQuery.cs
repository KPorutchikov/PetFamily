using PetFamily.Application.Abstractions;

namespace PetFamily.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(
    string? id, string? FullName, string? SortBy, string? SortDirection, int Page, int PageSize) : IQuery;