using System;
using Microsoft.Extensions.Caching.Memory;

using esdc_simulation_base.Src.Classes;
using esdc_simulation_base.Src.Storage;

namespace maternity_benefits.Storage.Cache
{
    public class MaternityBenefitsSimulationResultsCacheStore : IStoreSimulationResults<MaternityBenefitsCase>
    {
        private readonly static string cacheKeyBase = "maternity_benefits_results";
        private readonly IMemoryCache _cache;

        public MaternityBenefitsSimulationResultsCacheStore(IMemoryCache cache) {
            _cache = cache;
        }


        public void Save(Guid simulationId, SimulationResult simulationResult) {
           _cache.Set<SimulationResult>($"{cacheKeyBase}_{simulationId}", simulationResult, GetExpiry());
        } 

        public SimulationResult Get(Guid simulationId) {
            var res = _cache.Get<SimulationResult>($"{cacheKeyBase}_{simulationId}");
            if (res == null) {
                throw new NotFoundException("Simulation not found");
            }
            return res;
        }

        private TimeSpan GetExpiry() {
            return new TimeSpan(0, 20, 0);
        }
    }
}
