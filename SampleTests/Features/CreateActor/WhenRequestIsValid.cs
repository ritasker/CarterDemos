namespace SampleTests.Features.CreateActor
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using AutoFixture;
    using Carter;
    using Demos.Features.Simple;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Xunit;

    public class WhenRequestIsValid
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;
        private readonly IActorProvider actorProvider;

        public WhenRequestIsValid()
        {
            var actor = fixture.Build<Actor>()
                .Without(x => x.Id)
                .Create();
            
            actorProvider = A.Fake<IActorProvider>();

            var client = new TestServer(new WebHostBuilder()
                .ConfigureServices(s =>
                {
                    s.AddSingleton(actorProvider);
                    s.AddCarter();
                })
                .Configure(app => { app.UseCarter(); })
            ).CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(actor), Encoding.UTF8, "application/json");
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
            A.CallTo(() => actorProvider.Create(A<Actor>.Ignored)).MustHaveHappened();
        }
    }
}