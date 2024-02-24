using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Template.Api.IntegrationTests;

internal class TemplateApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;

    public TemplateApplication(string environment = "Development")
    {
        _environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
        });

        return base.CreateHost(builder);
    }
}