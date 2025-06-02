using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Abstractions;
using PetFamily.Application.Database;
using PetFamily.Application.Dtos;
using PetFamily.Application.Models;

namespace PetFamily.Application.SpeciesManagement.Queries.GetSpeciesDapper;

public class GetSpeciesHandlerDapper : IQueryHandler<PagedList<SpeciesDto>, GetSpeciesQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetSpeciesHandlerDapper> _logger;

    public GetSpeciesHandlerDapper(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetSpeciesHandlerDapper> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<PagedList<SpeciesDto>> Handle(GetSpeciesQuery query, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();
        var sql = new StringBuilder(
            """
            SELECT id, name, title
            FROM species
            WHERE 1 = 1
            """);
        if (!string.IsNullOrWhiteSpace(query.SpeciesId.ToString()))
        {
            sql.Append(" AND id = uuid(@SpeciesId)");
            parameters.Add("SpeciesId", query.SpeciesId);
        }

        _logger.LogInformation($"SQL: {sql}");

        var connection = _connectionFactory.Create();
        var species = await connection.QueryAsync<SpeciesDto>(sql.ToString(), parameters);

        return new PagedList<SpeciesDto>()
        {
            Items = species.ToList(),
            TotalCount = species.Count(),
            PageSize = 1,
            Page = 1
        };
    }
}