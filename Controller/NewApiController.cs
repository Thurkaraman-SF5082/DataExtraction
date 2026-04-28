using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataExtraction.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataExtraction
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewApiController : ControllerBase
    {
        private readonly IDBService _dBService;
        // private readonly IDbConnection _dbConnection;
        public NewApiController(IDBService dBService)
        {
            _dBService = dBService;
        }

        [HttpGet("data")]
        // public async Task<IActionResult> Get()
        public async Task<DataTable> Get()
        {
            try
            {
                DataTable dataTable = await _dBService.QueryRouter();
                System.Console.WriteLine($"controller : {dataTable}");
                return dataTable;
                // return Ok(new { Message = "success"});
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}