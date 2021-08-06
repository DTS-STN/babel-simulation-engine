
using System;
using System.Linq;

using esdc_simulation_base.Src.Classes;
using maternity_benefits;
using esdc_simulation_classes.MaternityBenefits;

namespace esdc_simulation_api.Controllers
{
    public static class MaternityBenefitMappers
    {
        public static SimulationResponse Convert(Simulation<MaternityBenefitsCase> simulation) {
            return new SimulationResponse() {
                Id = simulation.Id,
                SimulationName = simulation.Name,
                DateCreated = simulation.DateCreated,
                BaseCase = Convert(simulation.BaseCase),
                VariantCase = Convert(simulation.VariantCase)
            };
        }

        public static SimulationResultResponse Convert(SimulationResult result) 
        {
            var personResults = result.PersonResults.Select(x => {
                return new PersonResultResponse() {
                    VariantAmount = x.VariantAmount,
                    BaseAmount = x.BaseAmount,
                    Person = Convert((MaternityBenefitsPerson)x.Person)
                };
            }).ToList();

            return new SimulationResultResponse() {
                PersonResults = personResults
            };
        }

        public static PersonResponse Convert(MaternityBenefitsPerson person) {
            return new PersonResponse() {
                Id = person.Id,
                AverageIncome = person.AverageIncome,
                Age = person.Age,
                EducationLevel = person.EducationLevel,
                Province = person.Province,
                SpokenLanguage = person.SpokenLanguage
            };
        }

        public static Simulation<MaternityBenefitsCase> Convert(CreateSimulationRequest request) {
            return new Simulation<MaternityBenefitsCase>() {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now,
                Name = request.SimulationName,
                BaseCase = Convert(request.BaseCaseRequest),
                VariantCase = Convert(request.VariantCaseRequest)
            };
        }

        public static MaternityBenefitsCase Convert(CaseRequest caseModel) {
            return new MaternityBenefitsCase() {
                Id = Guid.NewGuid(),
                NumWeeks = caseModel.NumWeeks,
                MaxWeeklyAmount = caseModel.MaxWeeklyAmount,
                Percentage = caseModel.Percentage
            };
        }

        public static CaseRequest Convert(MaternityBenefitsCase caseModel) {
            return new CaseRequest() {
                NumWeeks = caseModel.NumWeeks,
                MaxWeeklyAmount = caseModel.MaxWeeklyAmount,
                Percentage = caseModel.Percentage
            };
        }
    }
}