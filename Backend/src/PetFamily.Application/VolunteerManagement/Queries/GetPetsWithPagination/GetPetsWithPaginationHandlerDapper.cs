using System.Data;
using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
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
        List<PetDto> pets;
        
        SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
        using var connection = _connectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@PageNumber", query.PageSize);
        parameters.Add("@PageSize", (query.Page - 1) * query.PageSize);

        var sql = new StringBuilder(
            """
            SELECT  p.id, p.name, p.description, s.name as species, b.name as breed, p.color, p.weight
                   ,p.height, p.health_information, p.phone, p.is_vaccinated, p.birthdate
                   ,p.files
            FROM pets p
            LEFT JOIN species s ON s.id = p.species_id
            LEFT JOIN breed   b ON b.id = p.breed_id
            WHERE 1 = 1
            """);
        
        if (!string.IsNullOrWhiteSpace(query.Id.ToString()))
        {
            sqlPart = " AND p.id = uuid(@Id)";
            sql.Append(sqlPart);
            parameters.Add("Id", $"%{query.Id}%");
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                sqlPart = " AND p.name LIKE @Name";
                sql.Append(sqlPart);
                parameters.Add("Name", $"%{query.Name}%");
            }

            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                sqlPart = " AND p.description LIKE @Description";
                sql.Append(sqlPart);
                parameters.Add("Description", $"%{query.Description}%");
            }

            if (!string.IsNullOrWhiteSpace(query.SpeciesId.ToString()))
            {
                sqlPart = " AND p.species_id = uuid(@SpeciesId)";
                sql.Append(sqlPart);
                parameters.Add("SpeciesId", query.SpeciesId);
            }

            if (!string.IsNullOrWhiteSpace(query.BreedId.ToString()))
            {
                sqlPart = " AND p.breed_id = uuid(@BreedId)";
                sql.Append(sqlPart);
                parameters.Add("BreedId", query.BreedId);
            }
            
            if (!string.IsNullOrWhiteSpace(query.SortBy) && !string.IsNullOrWhiteSpace(query.SortDirection))
                sql.ApplySorting(parameters, query.SortBy, query.SortDirection);

            if (query.Page > 0 && query.PageSize > 0)
                sql.ApplyPagination(parameters, query.Page, query.PageSize);
        }

        _logger.LogInformation($"SQL: {sql}");

        var petsResult = await connection.QueryAsync<PetDto, string, PetDto>(
            sql.ToString(),
            (pet, json) =>
            {
                List<PetFileDto> files = [];
                var result = JsonSerializer.Deserialize<RootValueObject>(json);
                files.AddRange(result.Values.Select(item => new PetFileDto { PathToStorage = item.PathToStorage }));
                pet.Files = files.ToArray();
                return pet;
            },
            parameters,
            splitOn: "files");
        
        return new PagedList<PetDto>()
        {
            Items = petsResult.ToList(),
            TotalCount = petsResult.Count(),
            PageSize = query.PageSize ?? 1,
            Page = query.Page ?? 1
        };
    }
    private class RootValueObject
    {
        public List<PetFileDto> Values { get; set; }
    }
    private class DapperSqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly date)
            => parameter.Value = date.ToDateTime(new TimeOnly(0, 0));
    
        public override DateOnly Parse(object value)
            => DateOnly.FromDateTime((DateTime)value);
    }
}