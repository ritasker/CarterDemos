using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Carter;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CarterDemos.Tests.Features.GetById
{
    public class WhenAnActorDoesNotExist
    {
        private readonly HttpResponseMessage response;
        
        public WhenAnActorDoesNotExist()
        {
            var actorProvider = new TestingActorProvider();

            var client = new TestServer(new WebHostBuilder()
                .ConfigureServices(s =>
                {
                    s.AddSingleton<IActorProvider>(actorProvider);
                    s.AddCarter();
                })
                .Configure(app => { app.UseRouting();
                    app.UseEndpoints(cfg => cfg.MapCarter());
                })
            ).CreateClient();

            response = client.GetAsync($"/actors/99").GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn404NotFound()
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}