
using System;

using esdc_simulation_base.Src.Classes;

namespace esdc_simulation_base.Src.Storage
{
    public interface IStoreSimulationResults<T> where T : ISimulationCase
    {
        void Save(Guid simulationId, SimulationResult simulationResult);
        SimulationResult Get(Guid simulationId);
    }
}