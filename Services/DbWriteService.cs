using System.Data;
using System.Text.Json;
using DataExtraction.Interfaces;
using DataExtraction.Models;
using Npgsql;
using NpgsqlTypes;

namespace DataExtraction.Services
{
    public class DbWriteService : IDbWriteService
    {
        // private readonly string _query;
        // private readonly string _tempTableQuery;
        private readonly string _upsertQuery;
        private readonly IDataTableToJsonConvertor _jsonConvertor;
        private readonly IConfiguration _configuration;
        public DbWriteService(IConfiguration configuration, IDataTableToJsonConvertor jsonConvertor)
        {
            _configuration = configuration;
            _jsonConvertor = jsonConvertor;
            // _query = new QueryReader().ReadQuery("import.sql");
            // _tempTableQuery = new QueryReader().ReadQuery("temp_table.sql");
            _upsertQuery = new QueryReader().ReadQuery("upsert_table.sql");
        }
        public async Task Upsert(DataTable dataTable)
        {
            // DBHandling.FetchDataFromDatabaseAsync(connectionString,_query,npgsqlreader=>new BoldInsightsEntity(){CreatedOn=(DateTime)npgsqlreader[3]});
            var connectionStringPsql = _configuration.GetConnectionString("PostgresDbLocal");

            string json = _jsonConvertor.DataTableToJson(dataTable);

            // await DBHandling.ExecuteTransactionAsync(connectionStringPsql,_tempTableQuery);

            // await DBHandling.ExecuteTransactionAsync(connectionStringPsql,_query,dataTable);

            await DBHandling.ExecuteTransactionAsync(connectionStringPsql, _upsertQuery, new { payload = json });

            // using var conn = new NpgsqlConnection(connectionStringPsql);
            // await conn.OpenAsync();

            // await using var transaction = await conn.BeginTransactionAsync();

            // try
            // {
            //     //creating temporary table
            //     await using var cmd = new NpgsqlCommand(_tempTableQuery, conn, transaction);
            //     await cmd.ExecuteNonQueryAsync();

            //     {
            //         //importing tickets to temp table
            //         using var writer = await conn.BeginBinaryImportAsync(_query);

            //         foreach (DataRow row in dataTable.Rows)
            //         {
            //             await writer.StartRowAsync();

            //             if (row["id"] == DBNull.Value) { await writer.WriteNullAsync(); }
            //             else { await writer.WriteAsync(Convert.ToInt64(row["id"]), NpgsqlDbType.Bigint); }

            //             if (row["title"] == DBNull.Value) { await writer.WriteNullAsync(); }
            //             else { await writer.WriteAsync(row["title"]?.ToString(), NpgsqlDbType.Text); }

            //             if (row["created_on"] == DBNull.Value) { await writer.WriteNullAsync(); }
            //             else { await writer.WriteAsync(Convert.ToDateTime(row["created_on"]), NpgsqlDbType.TimestampTz); }

            //             if (row["assignee"] == DBNull.Value) { await writer.WriteNullAsync(); }
            //             else { await writer.WriteAsync(row["assignee"]?.ToString(), NpgsqlDbType.Text); }

            //             if (row["Amount After Discount"] == DBNull.Value) { await writer.WriteNullAsync(); }
            //             else { await writer.WriteAsync(Convert.ToDouble(row["Amount After Discount"]), NpgsqlDbType.Double); }

            //             if (row["Commission"] == DBNull.Value) { await writer.WriteNullAsync(); }
            //             else { await writer.WriteAsync(Convert.ToDouble(row["Commission"]), NpgsqlDbType.Double); }

            //             if (row["Payment Frequency"] == DBNull.Value) { await writer.WriteNullAsync(); }
            //             else { await writer.WriteAsync(row["Payment Frequency"]?.ToString(), NpgsqlDbType.Text); }
            //         }
            //         await writer.CompleteAsync();
            //     }

            //     //updating the main table//
            //     await using var cmd1 = new NpgsqlCommand(_upsertQuery, conn, transaction);
            //     await cmd1.ExecuteNonQueryAsync();

            //     await transaction.CommitAsync();
            // }

            // catch (Exception ex)
            // {
            //     // await transaction.RollbackAsync();
            //     System.Console.WriteLine($"Import not successful: {ex.Message}");
            // }

        }
    }
}