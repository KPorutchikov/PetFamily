using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Dtos;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.Core.Models;

namespace PetFamily.Volunteers.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandlerDapper
    : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetVolunteersWithPaginationHandlerDapper> _logger;

    public GetVolunteersWithPaginationHandlerDapper(ISqlConnectionFactory connectionFactory,
        ILogger<GetVolunteersWithPaginationHandlerDapper> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<PagedList<VolunteerDto>> Handle(GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        string sqlPart;
        long? totalCount = null;
        var connection = _connectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", query.PageSize);
        parameters.Add("@PageSize", (query.Page - 1) * query.PageSize);

        var sqlCount = new StringBuilder("SELECT COUNT(*) FROM volunteers WHERE 1 = 1");

        var sql = new StringBuilder(
            """
            SELECT id, full_name as fullName, description, experience_in_years as experienceInYears, phone, email
            FROM volunteers
            WHERE 1 = 1
            """);

        if (!string.IsNullOrWhiteSpace(query.id))
        {
            sqlPart = " AND id = uuid(@id)";
            sqlCount.Append(sqlPart);
            sql.Append(sqlPart);
            parameters.Add("@id", query.id);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(query.FullName))
            {
                sqlPart = " AND full_name like @FullName";
                sqlCount.Append(sqlPart);
                sql.Append(sqlPart);
                parameters.Add("@FullName", $"%{query.FullName}%");
            }

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                sqlPart = " AND email like @Email";
                sqlCount.Append(sqlPart);
                sql.Append(sqlPart);
                parameters.Add("@Email", $"%{query.Email}%");
            }

            _logger.LogInformation($"SQL_COUNT: {sqlCount}");
            totalCount = await connection.ExecuteScalarAsync<long>(sqlCount.ToString(), parameters);

            if (!string.IsNullOrWhiteSpace(query.SortBy) && !string.IsNullOrWhiteSpace(query.SortDirection))
                sql.ApplySorting(parameters, query.SortBy, query.SortDirection);

            if (query.Page > 0 && query.PageSize > 0)
                sql.ApplyPagination(parameters, query.Page, query.PageSize);
        }

        _logger.LogInformation($"SQL: {sql}");
        var volunteers = await connection.QueryAsync<VolunteerDto>(sql.ToString(), parameters);

        return new PagedList<VolunteerDto>()
        {
            Items = volunteers.ToList(),
            TotalCount = totalCount ?? volunteers.Count(),
            PageSize = query.PageSize ?? 1,
            Page = query.Page ?? 1
        };
    }
}