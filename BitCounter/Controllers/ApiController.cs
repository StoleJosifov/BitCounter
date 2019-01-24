using System;
using System.Threading;
using System.Threading.Tasks;
using BitCounter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace BitCounter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHostedService counterService;
        private readonly IDataProviderService dataProviderService;

        public ApiController(IHostedService counterService, IDataProviderService dataProviderService)
        {
            this.counterService = counterService;
            this.dataProviderService = dataProviderService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> StartCounter()
        {
            await counterService.StartAsync(new CancellationToken(false));

            return Ok("Counter Service Started");
        }

        [HttpGet("[action]")]
        public async Task StopCounter()
        {
            await counterService.StopAsync(new CancellationToken(true));
        }

        [HttpGet("[action]")]
        public IActionResult ShowDataForDate(DateTime date)
        {
            var result = dataProviderService.GetCounterData("C:\\Users\\MeanMachine\\Desktop\\cnts_01-24-2019.json");
            return Ok(result);
        }
    }
}