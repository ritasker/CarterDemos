namespace SampleTests.Features.GetAllActors
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AutoFixture;
    using Demos.Features.Simple;
    using Carter;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class WhenActorsExist
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;

        public WhenActorsExist()
        {
            var actors = fixture.CreateMany<Actor>();
            var actorProvider = A.Fake<IActorProvider>();
            A.CallTo(() => actorProvider.Get()).Returns(actors);

            var client = new TestServer(new WebHostBuilder()
                .ConfigureServices(s =>
                {
                    s.AddSingleton(actorProvider);
                    s.AddCarter();
                })
                .Configure(app => { app.UseCarter(); })
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