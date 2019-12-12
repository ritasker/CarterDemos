using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using Carter;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CarterDemos.Tests.Features.GetAllActors
{
    public class WhenActorsExist
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;

        public WhenActorsExist()
        {
            var actors = fixture.CreateMany<Actor>();
            var actorProvider = new TestingActorProvider(actors.ToDictionary(k => k.Id));

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
        public void ShouldReturn200Ok()
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task ShouldReturnAListOfActors()
        {
            var actors = await response.Content.ToModel<IEnumerable<Actor>>();
            actors.Should().NotBeNullOrEmpty();
        }
    }
}