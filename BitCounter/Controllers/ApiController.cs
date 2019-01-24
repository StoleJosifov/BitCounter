using System;
using System.Threading;
using System.Threading.Tasks;
using BitCounter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BitCounter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHostedService counterService;
        private readonly IDataProviderService dataProviderService;
        private readonly IConfiguration configuration;

        public ApiController(IHostedService counterService, IDataProviderService dataProviderService, IConfiguration configuration)
        {
            this.counterService = counterService;
            this.dataProviderService = dataProviderService;
            this.configuration = configuration;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> StartCounter()
        {
            await counterService.StartAsync(new CancellationToken(false));

            return Ok("Counter Service Started");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> StopCounter()
        {
            await counterService.StopAsync(new CancellationToken(true));
            return Ok("Counter Service Stopped");
        }

        [HttpGet("[action]")]
        public IActionResult ShowData()
        {

            var result = dataProviderService.GetCounterDataFromFolder(configuration.GetSection("SaveFolder").Value);
            return Ok(result);
        }
    }
}