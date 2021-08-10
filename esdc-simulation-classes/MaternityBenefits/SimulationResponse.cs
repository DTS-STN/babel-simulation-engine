using System;

namespace esdc_simulation_classes.MaternityBenefits
{
    public class SimulationResponse
    {
        /// <summary>
        /// Unique Identifier
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of simulation
        /// </summary>
        /// <value></value>
        public string SimulationName { get; set; }

        /// <summary>
        /// Date simulation was created
        /// </summary>
        /// <value></value>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Base case
        /// </summary>
        /// <value></value>
        public CaseRequest BaseCase { get; set; }

        /// <summary>
        /// Variant case
        /// </summary>
        /// <value></value>
        public CaseRequest VariantCase { get; set; }

    }
}