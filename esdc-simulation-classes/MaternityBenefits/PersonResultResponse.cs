namespace esdc_simulation_classes.MaternityBenefits
{
    public class PersonResultResponse {
        public PersonResponse Person { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal VariantAmount { get; set; }
    }
}