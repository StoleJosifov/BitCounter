using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BitCounter.Models;
using BitCounter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BitCounter.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHostedService counterService;
        private readonly IDataProviderService dataProviderService;
        private readonly SaveSettings saveSettings;

        public ApiController(
            IHostedService counterService,
            IDataProviderService dataProviderService,
            IOptions<SaveSettings> settings)
        {
            this.counterService = counterService;
            this.dataProviderService = dataProviderService;
            this.saveSettings = settings.Value;
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
            var result = dataProviderService.GetCounterDataFromFolder(saveSettings.SaveFolder).TakeLast(10);
            return Ok(result);
        }
    }
}