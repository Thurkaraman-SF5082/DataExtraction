using System.Data;
using DataExtraction.Enums;
using DataExtraction.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataExtraction
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewApiController : ControllerBase
    {
        private readonly IDBReadService _dBReadService;
        private readonly IDbWriteService _dBWriteService;
        private readonly ICustomField _customField;
        public NewApiController(IDBReadService dBReadService, IDbWriteService dbWriteService, ICustomField customField)
        {
            _dBReadService = dBReadService;
            _dBWriteService = dbWriteService;
            _customField = customField;
        }

        //calling query router to access the db
        [HttpPost("DataTransfer")]
        public async Task<IActionResult> DataTransfer()
        {
            try
            {
                DataTable dataTable = await _dBReadService.QueryRouter();
                await _dBWriteService.Upsert(dataTable);
            }
            catch (AggregateException ex)
            {
                System.Console.WriteLine(ex.InnerException ?? ex);
            }
            return Ok("success");
        }

        [HttpGet("customfield")]
        public async Task<IActionResult> GetCustomField([FromBody] PaymentFrequency paymentFrequency)
        {
            int code = _customField.GetOperationsCustomFieldCode();
            return Ok(code.ToString());
        }
    }
}