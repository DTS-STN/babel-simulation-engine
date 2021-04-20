﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

using esdc_simulation_base.Src.Classes;
using esdc_simulation_base.Src.Storage;

namespace sample_scenario
{
    public class SampleScenarioSimulationResultsStore : IStoreSimulationResults<SampleScenarioCase>
    {
        private readonly static string cacheKeyBase = "sample_scenario_results";
        private readonly IMemoryCache _cache;

        public SampleScenarioSimulationResultsStore(IMemoryCache cache) {
            _cache = cache;
        }

        public void Save(Guid simulationId, SimulationResult simulationResult) {
           _cache.Set<SimulationResult>($"{cacheKeyBase}_{simulationId}", simulationResult);
        } 

        public SimulationResult Get(Guid simulationId) {
           return _cache.Get<SimulationResult>($"{cacheKeyBase}_{simulationId}");
        }

        public void Delete(Guid simulationId) {
           _cache.Remove($"{cacheKeyBase}_{simulationId}");
        }
    }
}