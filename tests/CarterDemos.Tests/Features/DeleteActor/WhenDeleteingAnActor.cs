using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AutoFixture;
using Carter;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CarterDemos.Tests.Features.DeleteActor
{
    public class WhenDeletingAnActor
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;
        private readonly TestingActorProvider actorProvider;
        private Actor _actor;

        public WhenDeletingAnActor()
        {
            _actor = fixture.Build<Actor>()
                .With(x => x.Id, 1)
                .Create();
            
            actorProvider = new TestingActorProvider(new Dictionary<int, Actor>
            {
                [1] = _actor
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

            response = client.DeleteAsync("/actors/1").GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn204NoContent()
        {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public void ShouldDeleteTheActor()
        {
            actorProvider.HasNoMatch(x => x.Id == _actor.Id).Should().BeTrue();
        }
    }
}