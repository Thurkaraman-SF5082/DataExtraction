using System.Data;

namespace DataExtraction.Interfaces
{
    public interface IDbWriteService
    {
        Task Insert(DataTable dataTable);
    }
}