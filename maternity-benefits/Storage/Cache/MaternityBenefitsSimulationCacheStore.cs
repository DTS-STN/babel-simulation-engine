using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

using esdc_simulation_base.Src.Classes;
using esdc_simulation_base.Src.Storage;

namespace maternity_benefits.Storage.Cache
{
    public class MaternityBenefitsSimulationCacheStore : IStoreSimulations<MaternityBenefitsCase>
    {
        private readonly static string cacheKeyBase = "maternity_benefits_simulations";
        private readonly IMemoryCache _cache;

        public MaternityBenefitsSimulationCacheStore(IMemoryCache cache) {
            _cache = cache;
        }

        public void Save(Simulation<MaternityBenefitsCase> simulation) {
            _cache.Set<Simulation<MaternityBenefitsCase>>($"{cacheKeyBase}_{simulation.Id}", simulation, GetExpiry());
            
        }

        public Simulation<MaternityBenefitsCase> Get(Guid simulationId) { 
            var res = _cache.Get<Simulation<MaternityBenefitsCase>>($"{cacheKeyBase}_{simulationId}");
            if (res == null) {
                throw new NotFoundException("Simulation not found");
            }
            return res;
        }

        public List<Simulation<MaternityBenefitsCase>> GetAll() {
            // TODO: Implement this if needed?
            throw new NotImplementedException();
        }

        public void Delete(Guid simulationId) {
           _cache.Remove($"{cacheKeyBase}_{simulationId}");
        }

        private TimeSpan GetExpiry() {
            return new TimeSpan(0, 20, 0);
        }
    }
}
