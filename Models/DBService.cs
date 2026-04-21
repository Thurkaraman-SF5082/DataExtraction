using System.Data;
using DataExtraction.Interfaces;

namespace DataExtraction.Models
{
    public class DBService : IDBService
    {
        private readonly string _query;
        private readonly IConfiguration _configuration;
        public DBService(IConfiguration configuration)
        {
            _configuration=configuration;
            _query = new QueryReader().ReadQuery("fetch_tickets.sql");
        }
        public async Task<DataTable> QueryRouter()
        //this error is confusing, I couldn't got any solution from web
        {
            var connectionStringPsql = _configuration.GetConnectionString("PostgresDb");
            // string connectionStringPsql = "Host=172.30.240.34;Port=5432;Username=salesopreaduser;Password=b9c!l9M4#zQ;Database=org-538";
            try
            {
                // await DBHandling.ExecuteQueryAsync(connectionStringPsql, query);
                DataTable dataTable = await DBHandling.ExecuteQueryAsync(connectionStringPsql, _query);
                return dataTable;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}