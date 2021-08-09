using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using esdc_simulation_base.Src.Rules;

namespace esdc_simulation_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> _logger;
        public PingController(ILogger<PingController> logger) {
            _logger = logger;
        }

        /// <summary>
        /// Ping
        /// </summary>
        /// <returns>Welcome text with date</returns>
        [HttpGet]
        public ActionResult<string> Index()
        {
            _logger.Log(LogLevel.Information, "Ping!");
            _logger.Log(LogLevel.Error, "Ping Error!");
            return $"Welcome to the Simulation API: {DateTime.Now}";
        }
    }
}
