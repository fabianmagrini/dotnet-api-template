
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;



namespace Template.Api.IntegrationTests
{
    public class EndpointsTests
    {
        [Fact]
        public async Task Get_Should_Retrieve_Health()
        {
            await using var application = new TemplateApplication();
            
            using var client = application.CreateClient();
            var response = await client.GetAsync("/health/live");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
