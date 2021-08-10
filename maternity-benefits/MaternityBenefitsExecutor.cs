using esdc_simulation_base.Src.Lib;
using esdc_simulation_base.Src.Rules;
using Rule = esdc_rules_classes.MaternityBenefits;

namespace maternity_benefits
{
    public class MaternityBenefitsExecutor : IExecuteRules<MaternityBenefitsCase, MaternityBenefitsPerson>
    {
        private readonly IRulesEngine _rulesApi;
        private static readonly string ENDPOINT = "MaternityBenefits";
        public MaternityBenefitsExecutor(IRulesEngine rulesApi) {
            _rulesApi = rulesApi;
        }

        public decimal Execute(MaternityBenefitsCase simulationCase, MaternityBenefitsPerson person) {
            var rule = new Rule.MaternityBenefitsCase() {
                NumWeeks = simulationCase.NumWeeks,
                MaxWeeklyAmount = simulationCase.MaxWeeklyAmount,
                Percentage = simulationCase.Percentage
            };
            var rulePerson = new Rule.MaternityBenefitsPerson() {
                AverageIncome = person.AverageIncome
            };
            var rulesReq = new Rule.MaternityBenefitsRequest() {
                Rule = rule,
                Person = rulePerson
            };

            var result = _rulesApi.Execute<Rule.MaternityBenefitsResponse>(ENDPOINT, rulesReq);
            return result.Amount;
        }
    }
}
