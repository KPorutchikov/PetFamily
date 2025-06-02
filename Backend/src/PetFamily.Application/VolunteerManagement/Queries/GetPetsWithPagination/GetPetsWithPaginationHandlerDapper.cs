using System.Data;
using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;

namespace PetFamily.Application.VolunteerManagement.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationHandlerDapper : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetPetsWithPaginationHandlerDapper> _logger;

    public GetPetsWithPaginationHandlerDapper(ISqlConnectionFactory connectionFactory,
        ILogger<GetPetsWithPaginationHandlerDapper> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<PagedList<PetDto>> Handle(GetPetsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        string? sqlPart;
        long? totalCount;
        List<PetDto> pets;
        
        SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
        using var connection = _connectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", query.PageSize);
        parameters.Add("@PageSize", (query.Page - 1) * query.PageSize);

        var sqlCount = new StringBuilder("SELECT COUNT(*) FROM pets p WHERE 1 = 1");

        var sql = new StringBuilder(
            """
            SELECT  p.id, p.name, p.description, s.name as species, b.name as breed, p.color, p.weight
                   ,p.height, p.health_information, p.phone, p.is_vaccinated, p.birthdate
            FROM pets p
            LEFT JOIN species s ON s.id = p.species_id
            LEFT JOIN breed   b ON b.id = p.breed_id
            WHERE 1 = 1
            """);
        
        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            sqlPart = " AND p.name LIKE @Name";
            sqlCount.Append(sqlPart);
            sql.Append(sqlPart);
            parameters.Add("Name", $"%{query.Name}%");
        }

        if (!string.IsNullOrWhiteSpace(query.Description))
        {
            sqlPart = " AND p.description LIKE @Description";
            sqlCount.Append(sqlPart);
            sql.Append(sqlPart);
            parameters.Add("Description", $"%{query.Description}%");
        }

        if (!string.IsNullOrWhiteSpace(query.SpeciesId.ToString()))
        {
            sqlPart = " AND p.species_id = uuid(@SpeciesId)";
            sqlCount.Append(sqlPart);
            sql.Append(sqlPart);
            parameters.Add("SpeciesId", query.SpeciesId);
        }

        if (!string.IsNullOrWhiteSpace(query.BreedId.ToString()))
        {
            sqlPart = " AND p.breed_id = uuid(@BreedId)";
            sqlCount.Append(sqlPart);
            sql.Append(sqlPart);
            parameters.Add("BreedId", query.BreedId);
        }

        var totalSql = sqlCount + "; " + Environment.NewLine + sql;
        _logger.LogInformation($"SQL: {totalSql}");

        using (var multi = connection.QueryMultiple(totalSql, parameters))
        {
            totalCount = multi.Read<long>().Single();
            pets = multi.Read<PetDto>().ToList();
        }

        return new PagedList<PetDto>()
        {
            Items = pets.ToList(),
            TotalCount = totalCount ?? pets.Count(),
            PageSize = query.PageSize ?? 1,
            Page = query.Page ?? 1
        };
    }

    private class DapperSqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly date)
            => parameter.Value = date.ToDateTime(new TimeOnly(0, 0));
    
        public override DateOnly Parse(object value)
            => DateOnly.FromDateTime((DateTime)value);
    }
}