using System;
using System.Collections.Generic;
using System.Linq;

using esdc_simulation_base.Src.Lib;
using esdc_simulation_base.Src.Classes;
using esdc_simulation_base.Src.Storage;

namespace maternity_benefits
{
    public interface IHandleSimulationRequests {
        Simulation<MaternityBenefitsCase> GetSimulation(Guid simulationId);
        List<Simulation<MaternityBenefitsCase>> GetAllSimulations();
        void CreateSimulation(Simulation<MaternityBenefitsCase> simulation);
        (Simulation<MaternityBenefitsCase>, SimulationResult) GetSimulationWithResult(Guid simulationId);
        void DeleteSimulation(Guid simulationId);
    }
    public class SimulationRequestHandler : IHandleSimulationRequests
    {
        private readonly IStoreSimulations<MaternityBenefitsCase> _simulationStore;
        private readonly IStoreSimulationResults<MaternityBenefitsCase> _resultStore;
        private readonly IRunSimulations<MaternityBenefitsCase, MaternityBenefitsPerson> _runner;
        private readonly IStorePersons<MaternityBenefitsPerson> _personStore;

        public SimulationRequestHandler(
            IStoreSimulations<MaternityBenefitsCase> simulationStore,
            IStoreSimulationResults<MaternityBenefitsCase> resultStore,
            IRunSimulations<MaternityBenefitsCase, MaternityBenefitsPerson> runner,
            IStorePersons<MaternityBenefitsPerson> personStore

        )
        {
            _simulationStore = simulationStore;
            _resultStore = resultStore;
            _runner = runner;
            _personStore = personStore;
        }

        public Simulation<MaternityBenefitsCase> GetSimulation(Guid simulationId)
        {
            return _simulationStore.Get(simulationId);
        }

        public List<Simulation<MaternityBenefitsCase>> GetAllSimulations()
        {
            return _simulationStore.GetAll();
        }

        public void CreateSimulation(Simulation<MaternityBenefitsCase> simulation)
        {
            _simulationStore.Save(simulation);
            
            var persons = _personStore.GetAllPersons();

            var result = _runner.Run(simulation, persons);

            _resultStore.Save(simulation.Id, result);
        }

        public (Simulation<MaternityBenefitsCase>, SimulationResult) GetSimulationWithResult(Guid simulationId)
        {
            var simulation = _simulationStore.Get(simulationId);
            var result = _resultStore.Get(simulationId); 
            return (simulation, result);
        }

        public void DeleteSimulation(Guid simulationId)
        {
            _simulationStore.Delete(simulationId);
        }
    }
}