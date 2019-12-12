using System.Collections.Generic;
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

namespace CarterDemos.Tests.Features.GetById
{
    public class WhenAnActorExists
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;
        
        public WhenAnActorExists()
        {
            var actor = fixture.Create<Actor>();
            var actorProvider = new TestingActorProvider(new Dictionary<int, Actor>
            {
                [actor.Id] = actor
            });

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

            response = client.GetAsync($"/actors/{actor.Id}").GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn200OK()
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        
        [Fact]
        public async Task ShouldReturnAnActor()
        {
            var actors = await response.Content.ToModel<Actor>();
            actors.Should().NotBeNull();
        }
    }
}