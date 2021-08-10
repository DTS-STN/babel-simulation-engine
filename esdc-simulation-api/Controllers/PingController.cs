using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            _logger.LogInformation("Ping!");
            return $"Welcome to the Simulation API: {DateTime.Now}";
        }
    }
}
