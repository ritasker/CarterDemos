using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using AutoFixture;
using Carter;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace CarterDemos.Tests.Features.UpdateActor
{
    public class WhenTheRequestIsValid
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;
        private readonly TestingActorProvider actorProvider;
        private Actor _actor;

        public WhenTheRequestIsValid()
        {
            _actor = fixture.Create<Actor>();
            actorProvider = new TestingActorProvider(new Dictionary<int, Actor>
            {
                [_actor.Id] = _actor
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

            _actor.Name = fixture.Create<string>();

            var content = new StringContent(JsonConvert.SerializeObject(_actor), Encoding.UTF8, "application/json");
            response = client.PutAsync($"/actors/{_actor.Id}", content).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn204NoContent()
        {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public void ShouldHaveSavedTheActor()
        {
            actorProvider.HasMatch(x =>
                    x.Id == _actor.Id &&
                    x.Name == _actor.Name &&
                    x.Age == _actor.Age)
                .Should().BeTrue();
        }
    }
}