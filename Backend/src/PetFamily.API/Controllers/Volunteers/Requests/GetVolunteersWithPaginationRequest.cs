﻿using PetFamily.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;

namespace PetFamily.API.Controllers.Volunteers.Requests;

public record GetVolunteersWithPaginationRequest(
    string? FullName,
    string? Email,
    string? SortBy,
    string? SortDirection,
    int? Page,
    int? PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery(Guid? volunteerId) =>
        new GetVolunteersWithPaginationQuery(volunteerId.ToString(), FullName, Email, SortBy, SortDirection, Page, PageSize);
}