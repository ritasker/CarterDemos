namespace SampleTests.Features.GetAllActors
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Demos.Features.Simple;
    using Carter;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Xunit;

    public class WhenActorsDoNotExist
    {
        private readonly HttpResponseMessage response;

        public WhenActorsDoNotExist()
        {
            var actorProvider = A.Fake<IActorProvider>();
            A.CallTo(() => actorProvider.Get()).Returns(new List<Actor>());
            
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