using System.Collections.Generic;
using Xunit;
using FakeItEasy;

using esdc_simulation_base.Src.Lib;
using esdc_simulation_base.Src.Classes;

namespace esdc_simulation_base.Tests.Lib
{
    public class SimulationRunnerTests
    {
        [Fact]
        public void ShouldWorkNormally()
        {
            // Arrange
            var caseRunner = A.Fake<IRunCases<ISimulationCase, IPerson>>();
            var joiner = A.Fake<IJoinResults>();

            var personResults = new List<PersonResult>() {
                new PersonResult()
            };
            A.CallTo(() => joiner.Join(A<SimulationCaseResult>._, A<SimulationCaseResult>._))
                .Returns(personResults);
            

            // Act
            var sut = new SimulationRunner<ISimulationCase, IPerson>(caseRunner, joiner);

            var simulation = A.Fake<Simulation<ISimulationCase>>();
            var persons = new List<IPerson>() {
                A.Fake<IPerson>()
            };

            var result = sut.Run(simulation, persons);

            // Assert
            A.CallTo(() => caseRunner.Run(A<ISimulationCase>._, A<IEnumerable<IPerson>>._)).MustHaveHappenedTwiceExactly();
            A.CallTo(() => joiner.Join(A<SimulationCaseResult>._, A<SimulationCaseResult>._)).MustHaveHappenedOnceExactly();

            Assert.Single(result.PersonResults);
        }
    }
}
