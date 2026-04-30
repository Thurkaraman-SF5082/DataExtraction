using System.Data;

namespace DataExtraction.Interfaces
{
    public interface IDBReadService
    {
        Task<DataTable> QueryRouter();
    }
}