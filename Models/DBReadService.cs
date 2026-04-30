using System.Data;
using DataExtraction.Interfaces;

namespace DataExtraction.Models
{
    public class DBReadService : IDBReadService
    {
        private readonly string _query;
        private readonly IConfiguration _configuration;

        //reading connection string from appsettings.json through IConfiguration
        //creating sql query by reading .sql file
        public DBReadService(IConfiguration configuration)
        {
            _configuration = configuration;
            _query = new QueryReader().ReadQuery("fetch_tickets.sql");
        }
        public async Task<DataTable?> QueryRouter()
        {
            var connectionStringPsql = _configuration.GetConnectionString("PostgresDb");

            try
            {
                DataTable? dataTable = await DBHandling.ExecuteQueryAsync(connectionStringPsql, _query);

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