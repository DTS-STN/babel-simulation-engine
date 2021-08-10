using System;

using esdc_simulation_base.Src.Classes;

namespace maternity_benefits
{
    public class MaternityBenefitsPerson : IPerson
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public string SpokenLanguage { get; set; }
        public string EducationLevel { get; set; }
        public string Province { get; set; }
        public decimal AverageIncome { get; set; }

        public MaternityBenefitsPerson() {
            Id = new Guid();
            Age = 0;
            SpokenLanguage = "";
            EducationLevel = "";
            Province = "";
            AverageIncome = 0;
        }
    }
    
}
