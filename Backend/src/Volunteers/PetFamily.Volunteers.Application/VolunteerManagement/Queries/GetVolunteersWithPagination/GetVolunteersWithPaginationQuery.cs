using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(
    string? id, string? FullName, string? Email, string? SortBy, string? SortDirection, int? Page, int? PageSize) : IQuery;