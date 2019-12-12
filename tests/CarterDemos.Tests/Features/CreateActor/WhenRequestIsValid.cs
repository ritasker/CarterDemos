using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Carter;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace CarterDemos.Tests.Features.CreateActor
{
    public class WhenRequestIsValid
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;
        private readonly TestingActorProvider actorProvider;
        private Actor _actor;

        public WhenRequestIsValid()
        {
            _actor = fixture.Build<Actor>()
                .Without(x => x.Id)
                .Create();
            
            actorProvider = new TestingActorProvider();

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

            var content = new StringContent(JsonConvert.SerializeObject(_actor), Encoding.UTF8, "application/json");
            response = client.PostAsync("/actors", content).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn201Created()
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
        
        [Fact]
        public async Task ShouldReturnTheActor()
        {
            var actor = await response.Content.ToModel<Actor>();
            actor.Should().NotBeNull();
        }
        
        [Fact]
        public void ShouldHaveSavedTheActor()
        {
            actorProvider.HasMatch(x => x.Age == _actor.Age && x.Name == _actor.Name).Should().BeTrue();
        }
    }
}