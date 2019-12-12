using System.Collections.Generic;
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

namespace CarterDemos.Tests.Features.GetAllActors
{
    public class WhenActorsDoNotExist
    {
        private readonly HttpResponseMessage response;

        public WhenActorsDoNotExist()
        {
            var actorProvider = new TestingActorProvider(new Dictionary<int, Actor>());
            
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

            response = client.GetAsync("/actors").GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn200OK()
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task ShouldReturnAnEmptyListOfActors()
        {
            var actors = await response.Content.ToModel<IEnumerable<Actor>>();
            actors.Should().NotBeNull().And.BeEmpty();
        }
    }
}