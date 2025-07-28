using System.Text;
using Dapper;

namespace PetFamily.Application.Extensions;

public static class SqlExtensions
{
    public static void ApplyPagination(
        this StringBuilder sqlBuilder,
        DynamicParameters parameters,
        int? page,
        int? pageSize)
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
    
    public static void ApplyMultiSorting(
        this StringBuilder sqlBuilder,
        string? sortColumns)
    {
        if (string.IsNullOrWhiteSpace(sortColumns))
            return;

        var res = sortColumns.Split(',')
                             .Select(s => { var parts = s.Split(' ');
                                            return new {column    = parts[0].Trim().ToLower(), 
                                                        direction = parts.Length > 1 ? parts[1].Trim().ToUpper() : ""};
                                          }).ToList();
        if (res.Count > 0)
        {
            var strSql = Environment.NewLine + "ORDER BY";
            
            foreach (var c in res)
                strSql += $" {c.column} {c.direction},";

            sqlBuilder.Append(strSql[..^1] + Environment.NewLine);
        }
    }
}