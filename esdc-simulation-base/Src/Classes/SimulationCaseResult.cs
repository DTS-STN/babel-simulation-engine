using System;
using System.Collections.Generic;

namespace esdc_simulation_base.Src.Classes
{
    public class SimulationCaseResult
    {
        public Dictionary<Guid, PersonCaseResult> ResultSet { get; set; }
        
        public SimulationCaseResult()
        {
            ResultSet = new Dictionary<Guid, PersonCaseResult>();
        }
    }
}