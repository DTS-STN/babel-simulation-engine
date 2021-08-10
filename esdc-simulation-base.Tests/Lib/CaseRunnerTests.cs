using System;
using System.Collections.Generic;

using Xunit;
using FakeItEasy;

using esdc_simulation_base.Src.Lib;
using esdc_simulation_base.Src.Classes;

namespace esdc_simulation_base.Tests.Lib
{
    public class CaseRunnerTests
    {
        
        [Fact]
        public void ShouldWorkNormally()
        {
            // Arrange
            var executor = A.Fake<IExecuteRules<ISimulationCase, IPerson>>();
            Guid id1 = Guid.NewGuid();
            Guid id2 = Guid.NewGuid();
            decimal val1 = 1.11m;
            decimal val2 = 2.22m;

            var p1 = A.Fake<IPerson>();
            p1.Id = id1;
            var p2 = A.Fake<IPerson>();
            p2.Id = id2;

            A.CallTo(() => executor.Execute(A<ISimulationCase>._, A<IPerson>._))
                .Returns(val1)
                .Once()
                .Then
                .Returns(val2);

            // Act
            var sut = new CaseRunner<ISimulationCase, IPerson>(executor);
            
            var simulationCase = A.Fake<ISimulationCase>();
            var persons = new List<IPerson>() { p1, p2 };

            var result = sut.Run(simulationCase, persons);

            // Assert
            Assert.Equal(val1, result.ResultSet[id1].Amount);
            Assert.Equal(val2, result.ResultSet[id2].Amount);
        }


    }
}
