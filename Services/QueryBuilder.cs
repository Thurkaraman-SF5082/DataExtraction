using System.Text;
using Dapper;
using DataExtraction.Interfaces;
using Npgsql;

namespace DataExtraction.Services
{
    public class QueryBuilder : IQueryBuilder
    {
        private readonly string? _connectionStringPsql;
        public QueryBuilder(IConfiguration configuration)
        {
            _connectionStringPsql = configuration.GetConnectionString("PostgresDbLocal");
        }

        public async Task<IEnumerable<T>> QueryBuilderAsync<T>(string table,
        int request,
        Dictionary<string, object>? filters = null,
        DateRangeFilter? dateRange = null,
        IEnumerable<string>? columns = null,
        CancellationToken token = default)
        {
            var selectedColumns = columns != null ? string.Join(", ", columns) : "*";

            var sql = new StringBuilder($"SELECT {selectedColumns} FROM \"{table}\"");

            var parameters = new DynamicParameters();
            var conditions = new List<string>();

            if (filters is { Count: > 0 })
            {
                foreach (var (key, value) in filters)
                {
                    conditions.Add($"{key} = @{key}");
                    parameters.Add($"@{key}", value);
                }
            }

            if (dateRange is not null)
            {
                conditions.Add($"{dateRange.Field} >= @startDate AND {dateRange.Field} < @endDate");
                parameters.Add("@startDate", dateRange.StartDate);
                parameters.Add("@endDate", dateRange.EndDate);
            }

            sql.Append(conditions.Count > 0 ? $"WHERE {string.Join(" AND ", conditions)}" : string.Empty);

            await using var connection = new NpgsqlConnection(_connectionStringPsql);

            return await connection.QueryAsync<T>(sql.ToString(), parameters);
        }
    }
}