using System.Data;
using DataExtraction.Enums;
using DataExtraction.Interfaces;
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
        public NewApiController(IDBReadService dBReadService, IDbWriteService dbWriteService, ICustomField customField, IPaymentOccurency paymentOccurency)
        {
            _dBReadService = dBReadService;
            _dBWriteService = dbWriteService;
            _customField = customField;
            _paymentOccurency = paymentOccurency;
        }

        //calling query router to access the db
        [HttpPost("DataTransfer")]
        public async Task<IActionResult> DataTransfer()
        {
            try
            {
                DataTable dataTable = await _dBReadService.QueryRouter();
                await _dBWriteService.Upsert(dataTable);
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
    }
}