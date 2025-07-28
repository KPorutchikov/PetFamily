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

    public GetPetsWithPaginationHandlerDapper(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetPetsWithPaginationHandlerDapper> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<PagedList<PetDto>> Handle(GetPetsWithPaginationQuery query, CancellationToken cancellationToken)
    {
        string? sqlPart;

        SqlMapper.AddTypeHandler(new DapperSqlDateOnlyTypeHandler());
        using var connection = _connectionFactory.Create();

        var parameters = new DynamicParameters();

        var sql = new StringBuilder(
            """
            SELECT  p.id, v.full_name as volunteer, p.name, p.description, s.name as species, b.name as breed, p.color
                   ,p.weight, p.height, p.health_information, p.phone, p.is_vaccinated, p.birthdate
                   ,p.address_city, p.address_street, p.files
            FROM pets p
            LEFT JOIN volunteers v ON v.id = p.volunteer_id    
            LEFT JOIN species    s ON s.id = p.species_id
            LEFT JOIN breed      b ON b.id = p.breed_id
            WHERE 1 = 1
            """);

        if (!string.IsNullOrWhiteSpace(query.PetId.ToString()))
        {
            sqlPart = " AND p.id = uuid(@Id)";
            sql.Append(sqlPart);
            parameters.Add("Id", $"{query.PetId}");
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(query.VolunteerId.ToString()))
            {
                sqlPart = " AND p.volunteer_id = uuid(@volunteer_id)";
                sql.Append(sqlPart);
                parameters.Add("volunteer_id", query.VolunteerId);
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                sqlPart = " AND p.name LIKE @Name";
                sql.Append(sqlPart);
                parameters.Add("Name", $"%{query.Name}%");
            }

            if (!string.IsNullOrWhiteSpace(query.SpeciesId.ToString()))
            {
                sqlPart = " AND p.species_id = uuid(@species_id)";
                sql.Append(sqlPart);
                parameters.Add("species_id", query.SpeciesId);
            }

            if (!string.IsNullOrWhiteSpace(query.BreedId.ToString()))
            {
                sqlPart = " AND p.breed_id = uuid(@breed_id)";
                sql.Append(sqlPart);
                parameters.Add("breed_id", query.BreedId);
            }

            if (!string.IsNullOrWhiteSpace(query.Color))
            {
                sqlPart = " AND p.color = @color";
                sql.Append(sqlPart);
                parameters.Add("color", $"{query.Color}");
            }

            if (!string.IsNullOrWhiteSpace(query.AddressCity))
            {
                sqlPart = " AND p.address_city LIKE @AddressCity";
                sql.Append(sqlPart);
                parameters.Add("address_city", $"%{query.AddressCity}%");
            }

            if (!string.IsNullOrWhiteSpace(query.AddressStreet))
            {
                sqlPart = " AND p.address_street LIKE @address_street";
                sql.Append(sqlPart);
                parameters.Add("address_street", $"%{query.AddressStreet}%");
            }

            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                sqlPart = " AND p.description LIKE @description";
                sql.Append(sqlPart);
                parameters.Add("description", $"%{query.Description}%");
            }

            if (!string.IsNullOrWhiteSpace(query.SortByColumns))
                sql.ApplyMultiSorting(query.SortByColumns);

            if (query.Page > 0 && query.PageSize > 0)
                sql.ApplyPagination(parameters, query.Page, query.PageSize);
        }

        _logger.LogInformation($"SQL: {sql}");

        var petsResult = await connection.QueryAsync<PetDto, string, PetDto>(
            sql.ToString(),
            (pet, json) =>
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    pet.Files = Array.Empty<PetFileDto>();
                    return pet;
                }
                try
                {
                    List<PetFileDto> files = [];
                    var result = JsonSerializer.Deserialize<RootValueObject>(json);
                    files.AddRange(result.Values.Select(item => new PetFileDto { PathToStorage = item.PathToStorage }));
                    pet.Files = files.ToArray();
                }
                catch (JsonException)
                {
                    pet.Files = Array.Empty<PetFileDto>();
                }

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
}