using System.Text;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Extensions;
using PetFamily.Application.Models;

namespace PetFamily.Application.VolunteerManagement.Queries.GetPet;

public class GetPetHandlerDapper: IQueryHandler<PagedList<PetDto>, GetPetQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetPetHandlerDapper> _logger;

    public GetPetHandlerDapper(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetPetHandlerDapper> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }
    
    public async Task<PagedList<PetDto>> Handle(GetPetQuery query, CancellationToken cancellationToken)
    {
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
            sql.Append(" AND p.id = uuid(@Id)");
            parameters.Add("Id", $"{query.PetId}");
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
                    files.AddRange(result!.Values.Select(item => new PetFileDto { PathToStorage = item.PathToStorage }));
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
            PageSize = 1,
            Page = 1
        };
    }
}