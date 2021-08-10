using System;

using esdc_simulation_base.Src.Classes;
using esdc_simulation_classes.MaternityBenefits;

namespace maternity_benefits
{
    public class MaternityBenefitsCase : CaseRequest, ISimulationCase
    {
        public Guid Id { get; set; }

        public MaternityBenefitsCase() : base() {
        }

        public MaternityBenefitsCase(CaseRequest request) {
            Id = Guid.NewGuid();
            NumWeeks = request.NumWeeks;
            MaxWeeklyAmount = request.MaxWeeklyAmount;
            Percentage = request.Percentage;
        }
    }
}
