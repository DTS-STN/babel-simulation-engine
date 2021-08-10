using System.Collections.Generic;

namespace esdc_simulation_base.Src.Classes
{
    public class SimulationResult
    {
        public List<PersonResult> PersonResults { get; set; }

        public SimulationResult() {
            PersonResults = new List<PersonResult>();
        }
    }
}