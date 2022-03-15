using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace ShoppingCart.BFF.Tests;

class ShoppingCartBffApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        return host;
    }
}

public class ApplicationTests
{
    [Fact]
    public async void GivenTheApplicationIsHealthy_WhenTheUserRequestsAHealthcheck_ThenItShouldReturnHealthyAnd200Status()
    {
        var response = await CreateClient().GetAsync("/healthcheck");
        
        Assert.NotNull(response);

        var result = await response.Content.ReadAsStringAsync();
        Assert.Equal("Healthy", result);
    }
    
    // [Fact]
    // public async void GivenCorsHasBeenSetUp_WhenTheChecksCors_ThenItShouldReturnOptionsForCors()
    // {
    //     var request = new HttpRequestMessage(HttpMethod.Options, "/healthcheck");
    //     var response = await CreateClient().SendAsync(request);
    //     
    //     Assert.NotNull(response);
    //     
    //     Assert.Equal("Healthy", response.Headers.ToString());
    // }

    private HttpClient CreateClient()
    {
        var app = new ShoppingCartBffApplication();
        return app.CreateClient();
    }
}