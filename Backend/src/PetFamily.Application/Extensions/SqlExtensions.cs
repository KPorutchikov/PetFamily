using System.Text;
using Dapper;

namespace PetFamily.Application.Extensions;

public static class SqlExtensions
{
    public static void ApplyPagination(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        int page,
        int pageSize)
    {
        sqlBuilder.Append(" LIMIT @PageSize OFFSET @Offset");

        parameters.Add("@PageSize", pageSize);
        parameters.Add("@Offset", (page - 1) * pageSize);
    }

    public static void ApplySorting(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        string? sortBy,
        string? sortDirection)
    {
        sqlBuilder.Append(" ORDER BY @SortBy");

        parameters.Add("@SortBy", $"{sortBy} {sortDirection}" ?? "1");
    }
}