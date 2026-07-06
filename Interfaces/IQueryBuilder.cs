using DataExtraction.Services;

namespace DataExtraction.Interfaces
{
    public interface IQueryBuilder
    {
        Task<IEnumerable<T>> QueryBuilderAsync<T>(string tableName,
        int request,
        Dictionary<string, object>? filters = null,
        DateRangeFilter? dateRange = null,
        IEnumerable<string>? columns = null,
        CancellationToken token = default);
    }
}