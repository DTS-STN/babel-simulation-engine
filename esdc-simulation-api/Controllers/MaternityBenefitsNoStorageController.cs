using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using esdc_simulation_base.Src.Classes;
using maternity_benefits;
using esdc_simulation_classes.MaternityBenefits;
using Mapper = esdc_simulation_api.Controllers.MaternityBenefitMappers;

namespace esdc_simulation_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MaternityBenefitsNoStorageController : ControllerBase
    {
        private readonly IHandleNoStorageSimulationRequests _handler;

        public MaternityBenefitsNoStorageController(IHandleNoStorageSimulationRequests handler)
        {
            _handler = handler;
        }

        [HttpPost]
        public ActionResult<FullResponse> CreateSimulationNoStorage(CreateSimulationRequest request)
        {
            var simulation = Mapper.Convert(request);
            var result = _handler.CreateSimulation(simulation);
            return new FullResponse {
                Simulation = Mapper.Convert(simulation),
                Result = Mapper.Convert(result)
            };
        }
    }
}
