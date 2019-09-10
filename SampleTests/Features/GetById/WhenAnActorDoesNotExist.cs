namespace SampleTests.Features.GetById
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Carter;
    using Demos.Features.Simple;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class WhenAnActorDoesNotExist
    {
        private readonly HttpResponseMessage response;
        
        public WhenAnActorDoesNotExist()
        {
            var actorProvider = A.Fake<IActorProvider>();
            A.CallTo(() => actorProvider.Get(A<int>.Ignored)).Throws<KeyNotFoundException>();

            var client = new TestServer(new WebHostBuilder()
                .ConfigureServices(s =>
                {
                    s.AddSingleton(actorProvider);
                    s.AddCarter();
                })
                .Configure(app => { app.UseCarter(); })
            ).CreateClient();

            response = client.GetAsync($"/actors/{99}").GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn404NotFound()
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        
        [Fact]
        public async Task ShouldReturnAMessage()
        {
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be("No resource found at /actors/99");
        }
    }
}