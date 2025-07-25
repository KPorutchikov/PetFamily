﻿using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Application.Database;

namespace PetFamily.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create() =>
        new NpgsqlConnection(_configuration.GetConnectionString("Database"));
}


public class SqlConnectionFactoryTest(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection Create() => new NpgsqlConnection(connectionString);
}