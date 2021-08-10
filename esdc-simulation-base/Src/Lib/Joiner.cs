using System.Collections.Generic;

using esdc_simulation_base.Src.Classes;

namespace esdc_simulation_base.Src.Lib
{
    public class Joiner : IJoinResults
    {
        public List<PersonResult> Join(SimulationCaseResult baseResult, SimulationCaseResult variantResult) {
            var result = new List<PersonResult>();

            foreach (var entry in baseResult.ResultSet) {
                var person = entry.Value.Person;
                var baseAmount = entry.Value.Amount;
                var variantAmount = variantResult.ResultSet[entry.Key].Amount;

                var nextResult = new PersonResult() {
                    Person = person,
                    BaseAmount = baseAmount,
                    VariantAmount = variantAmount,
                };
                result.Add(nextResult);
            }

            return result;
        }
    }
}