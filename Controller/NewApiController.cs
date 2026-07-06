using System.Data;
using DataExtraction.Enums;
using DataExtraction.Interfaces;
using DataExtraction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataExtraction
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewApiController : ControllerBase
    {
        private readonly IDBReadService _dBReadService;
        private readonly IDbWriteService _dBWriteService;
        private readonly ICustomField _customField;
        private readonly IPaymentOccurency _paymentOccurency;
        private readonly IDbJSONInsertion _dbJSONInsertion;
        private readonly IAgentTicketInsights _ticketInsights;
        private readonly IAgentTicketInsights _ticketInsightsJSON;
        public NewApiController(IDBReadService dBReadService, IDbWriteService dbWriteService, ICustomField customField, IPaymentOccurency paymentOccurency, IDbJSONInsertion dbJSONInsertion, IAgentTicketInsights ticketInsights, IAgentTicketInsights ticketInsightsJSON)
        {
            _dBReadService = dBReadService;
            _dBWriteService = dbWriteService;
            _customField = customField;
            _paymentOccurency = paymentOccurency;
            _dbJSONInsertion = dbJSONInsertion;
            _ticketInsights = ticketInsights;
            _ticketInsightsJSON = ticketInsightsJSON;
        }

        //calling query router to access the db
        [HttpPost("DataTransfer")]
        public async Task<IActionResult> DataTransfer()
        {
            try
            {
                DataTable dataTable = await _dBReadService.QueryRouter();
                await _dBWriteService.Upsert(dataTable);
                await _dbJSONInsertion.JSONInsert(dataTable);
                return Ok("success");
            }
            catch (AggregateException ex)
            {
                System.Console.WriteLine(ex.InnerException ?? ex);
                return BadRequest("Request failed");
            }
        }

        [HttpGet("CustomField")]
        public async Task<IActionResult> GetCustomField(string paymentFrequency)
        {
            int code = _customField.GetOperationsCustomFieldCode(paymentFrequency);
            return Ok(code);
        }

        [HttpGet("DisplayCustomField")]
        public async Task<IActionResult> DisplayCustomField()
        {
            var names = Enum.GetNames(typeof(PaymentFrequency)).ToList();
            return Ok(names);
        }

        [HttpGet("Occurency-PaymentFrequency")]
        public async Task<IActionResult> GetOccurencyPerYear(string paymentFrequency)
        {
            try
            {
                int Occurency = _paymentOccurency.GetPaymentOccurencyPerYear(paymentFrequency);
                return Ok(Occurency);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.InnerException ?? ex);
                return BadRequest();
            }
        }

        [HttpGet("FilteredTickets")]
        public async Task<IActionResult> GetFilteredTickets(string assignee, int month, int year,int request)
        {
            try
            {
                var tickets = await _ticketInsights.GetTicketsAync(assignee, month, year,request);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.InnerException ?? ex);
                return BadRequest("Request failed");
            }
        }

        [HttpGet("FilteredTicketsFromJSON")]
        public async Task<IActionResult> GetFilteredTicketsJSON(string assignee, int month, int year,int request)
        {
            try
            {
                var tickets = await _ticketInsightsJSON.GetTicketsAync(assignee, month, year,request);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.InnerException ?? ex);
                return BadRequest("Request failed");
            }
        }
    }
}