namespace SampleTests.Features.UpdateActor
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
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

    public class WhenTheRequestIsValid
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;
        private readonly IActorProvider actorProvider;
        
        public WhenTheRequestIsValid()
        {
            var actor = fixture.Create<Actor>();
            
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
            response = client.PutAsync($"/actors/{actor.Id}", content).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn204NoContent()
        {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        
        [Fact]
        public void ShouldHaveSavedTheActor()
        {
            A.CallTo(() => actorProvider.Update(A<Actor>.Ignored)).MustHaveHappened();
        }
    }
}