using System.Data;
using DataExtraction.Interfaces;

namespace DataExtraction.Services
{
    public class DBReadService : IDBReadService
    {
        private readonly string _query;
        private readonly string? _connectionStringPsql;

        //reading connection string from appsettings.json through IConfiguration
        //reading tickets data and storing it
        public DBReadService(IConfiguration configuration)
        {
            _connectionStringPsql = configuration.GetConnectionString("PostgresDb");
            _query = new QueryReader().ReadQuery("fetch_tickets.sql");
        }
        public async Task<DataTable?> QueryRouter()
        {
            try
            {
                DataTable? dataTable = await DBHandling.ExecuteQueryAsync(_connectionStringPsql, _query);

                return dataTable;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}