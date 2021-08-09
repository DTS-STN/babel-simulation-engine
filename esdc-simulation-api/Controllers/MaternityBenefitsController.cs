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
    public class MaternityBenefitsController : ControllerBase
    {
        private readonly IHandleSimulationRequests _handler;

        public MaternityBenefitsController(IHandleSimulationRequests handler)
        {
            _handler = handler;
        }

        [HttpGet("{simulationId}")]
        public ActionResult<SimulationResponse> GetSimulation(Guid simulationId)
        {
            try {
                var simulation = _handler.GetSimulation(simulationId);
                return Mapper.Convert(simulation);
            } catch (NotFoundException e) {
                return BadRequest(e.Message);
            }
        }

        [DisableFilter]
        [HttpGet]
        public ActionResult<AllSimulationsResponse> GetAllSimulations()
        {
            var result = new List<SimulationResponse>();
            var simulations = _handler.GetAllSimulations();
            foreach (var sim in simulations) {
                result.Add(Mapper.Convert(sim));
            }
            return new AllSimulationsResponse() {
                Simulations = result
            };
        }

        [HttpPost]
        public ActionResult<CreateSimulationResponse> CreateSimulation(CreateSimulationRequest request)
        {
            var simulation = Mapper.Convert(request);
            _handler.CreateSimulation(simulation);
            return new CreateSimulationResponse {
                Id = simulation.Id
            };
        }

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
        
        [DisableFilter]
        [HttpDelete("{untilLastXDays}/Batch")]
        public ActionResult DeleteSimulationBatch(int untilLastXDays)
        {
            _handler.DeleteSimulationBatch(untilLastXDays);
            return Ok();
        }

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
