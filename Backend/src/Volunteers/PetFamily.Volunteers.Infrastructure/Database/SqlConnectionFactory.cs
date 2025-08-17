using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.Volunteers.Infrastructure.Database;

public class SqlConnectionFactory: ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create() =>
        new NpgsqlConnection(_configuration.GetConnectionString("Database"));
}