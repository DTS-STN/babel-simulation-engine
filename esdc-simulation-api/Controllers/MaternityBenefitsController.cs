using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using esdc_simulation_base.Src.Classes;
using maternity_benefits;
using esdc_simulation_classes.MaternityBenefits;
using Mapper = esdc_simulation_api.Controllers.MaternityBenefitMappers;

namespace esdc_simulation_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MaternityBenefitsController : ControllerBase
    {
        private readonly IHandleSimulationRequests _handler;
        private readonly ILogger<MaternityBenefitsController> _logger;

        public MaternityBenefitsController(
            IHandleSimulationRequests handler,
            ILogger<MaternityBenefitsController> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        /// <summary>
        /// Get Simulation information by Id. Does not include results
        /// </summary>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpGet("{simulationId}")]
        public ActionResult<SimulationResponse> GetSimulation(Guid simulationId)
        {
            try {
                var simulation = _handler.GetSimulation(simulationId);
                return Mapper.Convert(simulation);
            } catch (NotFoundException e) {
                _logger.LogError("Simulation not found: {0}", simulationId);
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get all Simulations
        /// </summary>
        /// <returns></returns>
        [DisableFilter]
        [HttpGet]
        public ActionResult<AllSimulationsResponse> GetAllSimulations()
        {
            var result = _handler.GetAllSimulations()
                .Select(Mapper.Convert)
                .ToList();

            return new AllSimulationsResponse() {
                Simulations = result
            };
        }

        /// <summary>
        /// Generate a new simulation.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Unique Id that identifies the simulation</returns>
        [HttpPost]
        public ActionResult<CreateSimulationResponse> CreateSimulation(CreateSimulationRequest request)
        {
            var simulation = Mapper.Convert(request);
            _handler.CreateSimulation(simulation);
            return new CreateSimulationResponse {
                Id = simulation.Id
            };
        }

        /// <summary>
        /// Get Simulation information and associated results
        /// </summary>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpGet("{simulationId}/Results")]
        public ActionResult<FullResponse> GetFullResponse(Guid simulationId)
        {
            try {
                var simulationResult = _handler.GetSimulationWithResult(simulationId);
                return new FullResponse() {
                    Simulation = Mapper.Convert(simulationResult.Item1),
                    Result = Mapper.Convert(simulationResult.Item2)
                };
            }
            catch (NotFoundException e) {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete a simulation by Id
        /// </summary>
        /// <param name="simulationId"></param>
        /// <returns></returns>
        [HttpDelete("{simulationId}")]
        public ActionResult DeleteSimulation(Guid simulationId)
        {
            try {
                _handler.DeleteSimulation(simulationId);
                return Ok();
            }
            catch (NotFoundException e) {
                return BadRequest(e.Message);
            }
        }
    }
}
