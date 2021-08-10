namespace esdc_simulation_classes.MaternityBenefits
{
    // TODO: Could replace this with the Class from the Rules Engine Nuget Package
    public class CaseRequest
    {
        /// <summary>
        /// Maximum that an applicant is entitled to on a weekly basis
        /// </summary>
        /// <value></value>
        public decimal MaxWeeklyAmount { get; set; }

        /// <summary>
        /// Percentage of their average income that an applicant is entitled to 
        /// </summary>
        /// <value></value>
        public double Percentage { get; set; }

        /// <summary>
        /// Number of weeks for which an applicant is entitled to receive the benefit
        /// </summary>
        /// <value></value>
        public int NumWeeks { get; set; }

        public CaseRequest() {
            Percentage = 0;
            MaxWeeklyAmount = 0;
            NumWeeks = 0;
        }
    }
}
