namespace esdc_simulation_classes.MaternityBenefits
{
    public class CreateSimulationRequest
    {
        /// <summary>
        /// Name of Simulation
        /// </summary>
        /// <value></value>
        public string SimulationName { get; set; }

        /// <summary>
        /// Base case (initial values)
        /// </summary>
        /// <value></value>
        public CaseRequest BaseCaseRequest { get; set; }

        /// <summary>
        /// Variant case (proposed changes)
        /// </summary>
        /// <value></value>
        public CaseRequest VariantCaseRequest { get; set; }
    }
}