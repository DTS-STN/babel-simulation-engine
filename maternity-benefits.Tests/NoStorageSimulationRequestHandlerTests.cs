using System;
using System.Collections.Generic;

using Xunit;
using FakeItEasy;

using esdc_simulation_base.Src.Lib;
using esdc_simulation_base.Src.Classes;
using esdc_simulation_base.Src.Storage;
using maternity_benefits;

namespace maternity_benefits.Tests
{
    public class NoStorageSimulationRequestHandlerTests
    {
        [Fact]
        public void ShouldCreateSimulationNoStorage()
        {
            // Arrange
            var personStore = A.Fake<IStorePersons<MaternityBenefitsPerson>>();
            var runner = A.Fake<IRunSimulations<MaternityBenefitsCase, MaternityBenefitsPerson>>();

            var testId = Guid.NewGuid();
            var simulation = new Simulation<MaternityBenefitsCase>() {
                Id = testId
            };

            var mockResult = new SimulationResult() {
                PersonResults = new List<PersonResult>(){
                    A.Fake<PersonResult>(),
                    A.Fake<PersonResult>()
                }
            };

            A.CallTo(() => runner.Run(A<Simulation<MaternityBenefitsCase>>._, A<IEnumerable<MaternityBenefitsPerson>>._)).Returns(mockResult);
            
            // Act
            var sut = new NoStorageSimulationRequestHandler(runner, personStore);
            var result = sut.CreateSimulation(simulation);

            // Assert
            A.CallTo(() => personStore.GetAllPersons()).MustHaveHappenedOnceExactly();
            A.CallTo(() => runner.Run(A<Simulation<MaternityBenefitsCase>>._, A<IEnumerable<MaternityBenefitsPerson>>._)).MustHaveHappenedOnceExactly();
            
            Assert.Equal(mockResult.PersonResults.Count, result.PersonResults.Count);
        }

    }
}
