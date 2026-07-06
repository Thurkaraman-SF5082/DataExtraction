using DataExtraction.Models;

namespace DataExtraction.Interfaces
{
    public interface IAgentTicketInsights
    {
        Task<IEnumerable<BoldInsightsEntity>> GetTicketsAync(string assignee, int month, int year,int request);
    }
}