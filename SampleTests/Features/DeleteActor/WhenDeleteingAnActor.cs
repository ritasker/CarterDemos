namespace SampleTests.Features.DeleteActor
{
    using System.Net;
    using System.Net.Http;
    using BasicSample.Features;
    using Carter;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class WhenDeletingAnActor
    {
        private readonly HttpResponseMessage response;
        private readonly IActorProvider actorProvider;
        
        public WhenDeletingAnActor()
        {
            actorProvider = A.Fake<IActorProvider>();

            var client = new TestServer(new WebHostBuilder()
                .ConfigureServices(s =>
                {
                    s.AddSingleton<IActorProvider>(actorProvider);
                    s.AddCarter();
                })
                .Configure(app => { app.UseCarter(); })
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
            A.CallTo(() => actorProvider.Delete(A<int>.Ignored)).MustHaveHappened();
        }
    }
}