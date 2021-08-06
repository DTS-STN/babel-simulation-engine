using System;
using System.Collections.Generic;
using System.Linq;

using esdc_simulation_base.Src.Lib;
using esdc_simulation_base.Src.Classes;
using esdc_simulation_base.Src.Storage;


namespace maternity_benefits
{
    public interface IHandleNoStorageSimulationRequests {
        SimulationResult CreateSimulation(Simulation<MaternityBenefitsCase> simulation);
    }
    public class NoStorageSimulationRequestHandler : IHandleNoStorageSimulationRequests
    {
        private readonly IRunSimulations<MaternityBenefitsCase, MaternityBenefitsPerson> _runner;
        private readonly IStorePersons<MaternityBenefitsPerson> _personStore;

        public NoStorageSimulationRequestHandler(
            IRunSimulations<MaternityBenefitsCase, MaternityBenefitsPerson> runner,
            IStorePersons<MaternityBenefitsPerson> personStore

        )
        {
            _runner = runner;
            _personStore = personStore;
        }

        public SimulationResult CreateSimulation(Simulation<MaternityBenefitsCase> simulation) {
            var persons = _personStore.GetAllPersons();

            var result = _runner.Run(simulation, persons);

            return result;
        }
    }
}