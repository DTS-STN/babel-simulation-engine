namespace esdc_simulation_classes.MaternityBenefits
{
    public class MaternityBenefitsPersonRequest
    {
        /// <summary>
        /// Age of applicant
        /// </summary>
        /// <value></value>
        public int Age { get; set; }

        /// <summary>
        /// Spoke language of applicant
        /// </summary>
        /// <value></value>
        public string SpokenLanguage { get; set; }

        /// <summary>
        /// Province of applicant
        /// </summary>
        /// <value></value>
        public string Province { get; set; }

        /// <summary>
        /// Education level of applicant
        /// </summary>
        /// <value></value>
        public string EducationLevel { get; set; }

        /// <summary>
        /// Average weekly income of applicant
        /// </summary>
        /// <value></value>
        public decimal AverageIncome { get; set; }
    }
}
