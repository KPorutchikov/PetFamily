using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using PetFamily.Shared.Core.Abstractions;
using PetFamily.Shared.Core.Dtos;

namespace PetFamily.Accounts.Application.AccountManagement.Queries.Users.GetUser;

public class GetUserHandlerDapper: IQueryHandler<UserDto, GetUserQuery>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetUserHandlerDapper> _logger;

    public GetUserHandlerDapper(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetUserHandlerDapper> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<UserDto> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var sql = new StringBuilder(
            """
            SELECT id, user_name, email, phone_number
            FROM accounts.users
            WHERE id = uuid(@Id)
            """);

        var parameters = new DynamicParameters();
        parameters.Add("Id", $"{query.UserId}");
        
        _logger.LogInformation($"SQL: {sql}");
        
        var connection = _connectionFactory.Create();
        var userResult = await connection.QueryAsync<UserDto>(sql.ToString(), parameters);
        
        var user = userResult.FirstOrDefault();
        
        if (user != null)
        {
            sql.Clear().Append(
                """
                SELECT a.id as id_account, a.full_name, 0 as account_type
                FROM accounts.admin_accounts a
                WHERE a.user_id = uuid(@Id)
                 UNION ALL
                SELECT v.id, v.full_name, 1
                FROM accounts.volunteer_accounts v
                WHERE v.user_id = uuid(@Id)
                 UNION ALL
                SELECT p.id, p.full_name, 2
                FROM accounts.participant_accounts p
                WHERE p.user_id = uuid(@Id)
                """);
            _logger.LogInformation($"SQL: {sql}");
            
            var accountsResult = await connection.QueryAsync<UserAccountDto>(sql.ToString(), parameters);
            if (accountsResult != null)
            {
                user.Accounts = accountsResult.ToArray();
            }
        }
        return user;
    }
}