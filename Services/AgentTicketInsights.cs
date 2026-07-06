using DataExtraction.Interfaces;
using DataExtraction.Models;

namespace DataExtraction.Services
{
    public class AgentTicketInsights : IAgentTicketInsights
    {
        private readonly IQueryBuilder _builder;
        public AgentTicketInsights(QueryBuilder builder)
        {
            _builder = builder;
        }
        public async Task<IEnumerable<BoldInsightsEntity>> GetTicketsAync(string assignee, int month, int year,int request)
        {
            var filter = new Dictionary<string, object>
            {
                ["assignee"] = assignee
            };

            var dateRange = new DateRangeFilter("created_on", month, year);

            return await _builder.QueryBuilderAsync<BoldInsightsEntity>("BoldInsights",request, filter, dateRange);
        }
    }
}