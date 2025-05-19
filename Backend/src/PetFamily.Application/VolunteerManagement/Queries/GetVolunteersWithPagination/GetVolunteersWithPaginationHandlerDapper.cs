using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;
using PetFamily.Domain.Shared;

namespace PetFamily.Application.VolunteerManagement.Queries.GetVolunteersWithPagination;

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
        var connection = _connectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", query.PageSize);
        parameters.Add("@PageSize", (query.Page - 1) * query.PageSize);

        var sql_count = new StringBuilder("SELECT COUNT(*) FROM volunteers WHERE 1 = 1");

        var sql = new StringBuilder(
            """
            SELECT id, full_name as fullName, description, experience_in_years as experienceInYears, phone, email
            FROM volunteers
            WHERE 1 = 1
            """);

        if (!string.IsNullOrWhiteSpace(query.id))
        {
            sqlPart = " AND id = uuid(@id)";
            sql_count.Append(sqlPart);
            sql.Append(sqlPart);
            parameters.Add("@id", query.id);
        }

        if (!string.IsNullOrWhiteSpace(query.FullName))
        {
            sqlPart = " AND full_name like @FullName";
            sql_count.Append(sqlPart);
            sql.Append(sqlPart);
            parameters.Add("@FullName", $"%{query.FullName}%");
        }

        _logger.LogInformation($"SQL_COUNT: {sql_count}");
        long totalCount = await connection.ExecuteScalarAsync<long>(sql_count.ToString(), parameters);

        sql.ApplySorting(parameters, query.SortBy, query.SortDirection);
        sql.ApplyPagination(parameters, query.Page, query.PageSize);

        _logger.LogInformation($"SQL: {sql}");
        var volunteers = await connection.QueryAsync<VolunteerDto>(sql.ToString(), parameters);

        return new PagedList<VolunteerDto>()
        {
            Items = volunteers.ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page
        };
    }
}