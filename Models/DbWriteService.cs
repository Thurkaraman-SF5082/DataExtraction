using System.Data;
using DataExtraction.Interfaces;
using Npgsql;
using NpgsqlTypes;

namespace DataExtraction.Models
{
    public class DbWriteService : IDbWriteService
    {
        private readonly string _query;
        private readonly IConfiguration _configuration;
        public DbWriteService(IConfiguration configuration)
        {
            _configuration = configuration;
            _query = new QueryReader().ReadQuery("writer.sql");
        }
        public async Task Insert(DataTable dataTable)
        {
            var connectionStringPsql = _configuration.GetConnectionString("PostgresDbLocal");

            using var conn = new NpgsqlConnection(connectionStringPsql);
            await conn.OpenAsync();

            using var writer = await conn.BeginBinaryImportAsync(_query);

            try
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    await writer.StartRowAsync();

                    if (row["id"] == DBNull.Value) { await writer.WriteNullAsync(); }
                    else { await writer.WriteAsync(Convert.ToInt64(row["id"]), NpgsqlDbType.Bigint); }

                    if (row["title"] == DBNull.Value) { await writer.WriteNullAsync(); }
                    else { await writer.WriteAsync(row["title"]?.ToString(), NpgsqlDbType.Text); }

                    if (row["assignee"] == DBNull.Value) { await writer.WriteNullAsync(); }
                    else { await writer.WriteAsync(row["assignee"]?.ToString(), NpgsqlDbType.Text); }

                    if (row["Amount After Discount"] == DBNull.Value) { await writer.WriteNullAsync(); }
                    else { await writer.WriteAsync(Convert.ToDouble(row["Amount After Discount"]), NpgsqlDbType.Double); }

                    if (row["Commission"] == DBNull.Value) { await writer.WriteNullAsync(); }
                    else { await writer.WriteAsync(Convert.ToDouble(row["Commission"]), NpgsqlDbType.Double); }
                }
                await writer.CompleteAsync();
            }
            catch (Exception ex)
            {
                await writer.CloseAsync();
                System.Console.WriteLine($"Import not successful: {ex.Message}");
            }

        }
    }
}