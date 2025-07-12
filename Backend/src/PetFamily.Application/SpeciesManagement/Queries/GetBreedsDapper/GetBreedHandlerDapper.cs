using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;

namespace PetFamily.Application.SpeciesManagement.Queries.GetBreedsDapper;

public class GetBreedHandlerDapper : IQueryHandler<PagedList<BreedDto>, GetBreedQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetBreedHandlerDapper> _logger;

    public GetBreedHandlerDapper(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetBreedHandlerDapper> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<PagedList<BreedDto>> Handle(GetBreedQuery query, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        var sql = new StringBuilder(
            """
            SELECT id, name, species_id as speciesId
            FROM breed
            WHERE 1 = 1
            """);
        
        if (!string.IsNullOrWhiteSpace(query.BreedId.ToString()))
        {
            sql.Append(" AND id = uuid(@BreedId)");
            parameters.Add("BreedId", query.BreedId);
        }
        
        if (!string.IsNullOrWhiteSpace(query.SpeciesId.ToString()))
        {
            sql.Append(" AND species_id = uuid(@SpeciesId)");
            parameters.Add("SpeciesId", query.SpeciesId);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            sql.Append(" AND name = @Name");
            parameters.Add("Name", query.Name);
        }

        _logger.LogInformation($"SQL: {sql}");

        var connection = _connectionFactory.Create();
        var breed = await connection.QueryAsync<BreedDto>(sql.ToString(), parameters);

        return new PagedList<BreedDto>()
        {
            Items = breed.ToList(),
            TotalCount = breed.Count(),
            PageSize = 1,
            Page = 1
        };
    }
}