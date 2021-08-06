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
    public class SimulationRequestHandlerTests
    {
        [Fact]
        public void ShouldCreateSimulation()
        {
            // Arrange
            var simulationStore = A.Fake<IStoreSimulations<MaternityBenefitsCase>>();
            var personStore = A.Fake<IStorePersons<MaternityBenefitsPerson>>();
            var resultStore = A.Fake<IStoreSimulationResults<MaternityBenefitsCase>>();
            var runner = A.Fake<IRunSimulations<MaternityBenefitsCase, MaternityBenefitsPerson>>();

            var testId = Guid.NewGuid();
            var simulation = new Simulation<MaternityBenefitsCase>() {
                Id = testId
            };
            
            // Act
            var sut = new SimulationRequestHandler(simulationStore, resultStore, runner, personStore);
            sut.CreateSimulation(simulation);

            // Assert
            A.CallTo(() => simulationStore.Save(A<Simulation<MaternityBenefitsCase>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => personStore.GetAllPersons()).MustHaveHappenedOnceExactly();
            A.CallTo(() => runner.Run(A<Simulation<MaternityBenefitsCase>>._, A<IEnumerable<MaternityBenefitsPerson>>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => resultStore.Save(testId, A<SimulationResult>._)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void ShouldGetSimulation()
        {
            // Arrange
            var simulationStore = A.Fake<IStoreSimulations<MaternityBenefitsCase>>();
            var personStore = A.Fake<IStorePersons<MaternityBenefitsPerson>>();
            var resultStore = A.Fake<IStoreSimulationResults<MaternityBenefitsCase>>();
            var runner = A.Fake<IRunSimulations<MaternityBenefitsCase, MaternityBenefitsPerson>>();

            var testId = Guid.NewGuid();
            var testName = "My Fake Simulation";
            var simulation = new Simulation<MaternityBenefitsCase>() {
                Id = testId,
                Name = testName
            };

            A.CallTo(() => simulationStore.Get(testId)).Returns(simulation);

            // Act
            var sut = new SimulationRequestHandler(simulationStore, resultStore, runner, personStore);
            var result = sut.GetSimulation(testId);

            // Assert
            A.CallTo(() => simulationStore.Get(testId)).MustHaveHappenedOnceExactly();
            Assert.Equal(testId, result.Id);
            Assert.Equal(testName, result.Name);
        }

    }
}
