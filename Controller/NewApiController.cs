using System.Data;
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
        public NewApiController(IDBReadService dBReadService, IDbWriteService dbWriteService)
        {
            _dBReadService = dBReadService;
            _dBWriteService = dbWriteService;
        }

        //calling query router to access the db
        [HttpPost("DataTransfer")]
        public async Task<IActionResult> Transfer()
        {
            try
            {
                DataTable dataTable = await _dBReadService.QueryRouter();
                await _dBWriteService.Insert(dataTable);
            }
            catch (AggregateException ex)
            {
                System.Console.WriteLine(ex.InnerException?? ex);
            }
            return Ok("success");
        }

        // [HttpGet("customfield")]
        // public Task<IActionResult> GetCustomField()
        // {

        //     // return Ok("success");
        // }
    }
}