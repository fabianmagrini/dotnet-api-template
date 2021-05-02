
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Template.Api;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;


namespace Template.Api.IntegrationTests
{
    public class WeatherForecastControllerTests: IClassFixture<WebApplicationFactory<Api.Startup>>
    {
        readonly HttpClient _client;

        public WeatherForecastControllerTests(WebApplicationFactory<Api.Startup> fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task Get_Should_Retrieve_Forecast()
        {
            var response = await _client.GetAsync("/weatherforecast");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
    
            var forecast = JsonSerializer.Deserialize<WeatherForecast[]>(await response.Content.ReadAsStringAsync());
            forecast.Should().HaveCount(5);
        }
    }
}
