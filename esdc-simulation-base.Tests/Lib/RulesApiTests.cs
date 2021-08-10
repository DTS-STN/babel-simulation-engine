using Microsoft.Extensions.Options;
using Xunit;
using FakeItEasy;
using RestSharp;

using esdc_simulation_base.Src.Rules;

namespace esdc_simulation_base.Tests.Lib
{
    public class RulesApiTests
    {
        
        [Fact]
        public void ShouldWorkNormally()
        {
            // Arrange
            decimal testAmount = 1.56m;

            var client = A.Fake<IRestClient>();
            var options = Options.Create(new RulesOptions() {
                Url = "http://localhost:6000"
            });

            var postResult = A.Fake<RestResponse<decimal>>();
            postResult.Data = testAmount;
            postResult.StatusCode = System.Net.HttpStatusCode.OK;
            
            A.CallTo(() => client.Execute<decimal>(A<RestRequest>._, Method.POST))
                .Returns(postResult);

            // Act
            var sut = new RulesApi(client, options);
            var rulesRequest = A.Fake<object>();
            var result = sut.Execute<decimal>("fake_endpoint", rulesRequest);

            // Assert
            Assert.Equal(testAmount, result);
        }


    }
}
