using System.Text;
using System.Text.Json;
using DataExtraction.Interfaces;
using Npgsql;

namespace DataExtraction.Services
{
    public class JsonQueryBuilder : IQueryBuilder
    {
        private readonly string? _connectionStringPsql;

        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public JsonQueryBuilder(IConfiguration configuration)
        {
            _connectionStringPsql = configuration.GetConnectionString("PostgresDbLocal");
        }
        public async Task<IEnumerable<T>> QueryBuilderAsync<T>(string tableName,
        int request,
        Dictionary<string, object>? filters = null,
        DateRangeFilter? dateRange = null,
        IEnumerable<string>? columns = null,
        CancellationToken token = default)
        {
            System.Console.WriteLine($"http request : {request}");
            var conditions = new List<string>();
            var parameters = new List<NpgsqlParameter>();

            if (filters is { Count: > 0 })
            {
                foreach (var (jsonField, value) in filters)
                {
                    conditions.Add($"data->>'{jsonField}' = @{jsonField}");
                    parameters.Add(new NpgsqlParameter($"@{jsonField}", value));
                }
            }

            if (dateRange is not null)
            {
                conditions.Add($"(data->>'{dateRange.Field}')::date >= @startDate AND (data->>'{dateRange.Field}')::date < @endDate");
                parameters.Add(new NpgsqlParameter("@startDate", dateRange.StartDate));
                parameters.Add(new NpgsqlParameter("@endDate", dateRange.EndDate));
            }

            var sql = new StringBuilder($"SELECT data FROM \"{tableName}\"");

            sql.Append(conditions.Count > 0 ? $"WHERE {string.Join(" AND ", conditions)}" : string.Empty);

            await using var connection = new NpgsqlConnection(_connectionStringPsql);
            await connection.OpenAsync(token);

            await using var command = new NpgsqlCommand(sql.ToString(), connection);
            command.Parameters.AddRange(parameters.ToArray());

            await using var reader = await command.ExecuteReaderAsync(token);

            var result = new List<T>();

            while (await reader.ReadAsync(token))
            {
                var json = reader.GetString(0);
                var record = JsonSerializer.Deserialize<T>(json, _serializerOptions);
                if (record is not null)
                {
                    result.Add(record);
                }
            }

            return result;
        }
    }
}