using System.Data;

namespace DataExtraction.Interfaces
{
    public interface IDbWriteService
    {
        Task Upsert(DataTable dataTable);
    }
}