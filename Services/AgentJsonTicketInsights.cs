using DataExtraction.Interfaces;
using DataExtraction.Models;

namespace DataExtraction.Services
{
    public class AgentJsonTicketInsights : IAgentTicketInsights
    {
        private readonly IQueryBuilder _jsonQueryBuilder;
        public AgentJsonTicketInsights(JsonQueryBuilder jsonQueryBuilder)
        {
            _jsonQueryBuilder = jsonQueryBuilder;
        }
        public async Task<IEnumerable<BoldInsightsEntity>> GetTicketsAync(string assignee, int month, int year,int request)
        {
            var filters = new Dictionary<string, object>
            {
                ["assignee"] = assignee
            };

            var dateRange = new DateRangeFilter("created_on", month, year);

            return await _jsonQueryBuilder.QueryBuilderAsync<BoldInsightsEntity>("Json_BoldInsights",request, filters, dateRange);
        }
    }
}