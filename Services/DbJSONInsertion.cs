using System.Data;
using System.Text.Json;
using DataExtraction.Interfaces;
using DataExtraction.Models;
using Npgsql;
using NpgsqlTypes;

namespace DataExtraction.Services
{
    public class DbJSONInsertion : IDbJSONInsertion
    {
        private readonly IConfiguration _configuration;
        private readonly string _jsonInsertQuery;
        public DbJSONInsertion(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonInsertQuery = new QueryReader().ReadQuery("json_insert.sql");
        }
        public async Task JSONInsert(DataTable dataTable)
        {
            string? connectionStringPsql = _configuration.GetConnectionString("PostgresDbLocal");

            string json = JsonSerializer.Serialize(
                dataTable.AsEnumerable().Select(row =>
                dataTable.Columns.Cast<DataColumn>().ToDictionary(
                    col => col.ColumnName,
                    col => row[col] == DBNull.Value ? null : row[col]
                ))
            );

            await using NpgsqlConnection connection = new(connectionStringPsql);
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }
            await using NpgsqlTransaction transaction = await connection.BeginTransactionAsync();

            try
            {
                await using var command = new NpgsqlCommand(_jsonInsertQuery, connection, transaction);

                command.Parameters.Add(
                    new NpgsqlParameter("@data", NpgsqlDbType.Jsonb) { Value = json }
                );

                await command.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                System.Console.WriteLine($"Json insert failed : {ex.InnerException ?? ex}");
            }
        }
    }
}