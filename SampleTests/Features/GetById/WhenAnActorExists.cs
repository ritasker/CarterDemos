namespace SampleTests.Features.GetById
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AutoFixture;
    using Carter;
    using Demos.Features.Simple;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class WhenAnActorExists
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;
        
        public WhenAnActorExists()
        {
            var actor = fixture.Create<Actor>();
            var actorProvider = A.Fake<IActorProvider>();
            A.CallTo(() => actorProvider.Get(actor.Id)).Returns(actor);

            var client = new TestServer(new WebHostBuilder()
                .ConfigureServices(s =>
                {
                    s.AddSingleton(actorProvider);
                    s.AddCarter();
                })
                .Configure(app => { app.UseCarter(); })
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