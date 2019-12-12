using System;
using System.Collections.Generic;
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

namespace CarterDemos.Tests.Features.UpdateActor
{
    public class WhenTheRequestIsInvalid
    {
        private readonly Fixture fixture = new Fixture();
        private readonly HttpResponseMessage response;
        private readonly TestingActorProvider actorProvider;
        private Actor _actor;

        public WhenTheRequestIsInvalid()
        {
            _actor = fixture.Build<Actor>()
                .Without(x => x.Name)
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
            response = client.PutAsync($"/actors/{_actor.Id}", content).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void ShouldReturn422UnprocessableEntity()
        {
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        }
        
        [Fact]
        public async Task ShouldReturnTheValidationErrors()
        {
            var errors = await response.Content.ToModel<IEnumerable<ValidationError>>();
            errors.Should().Contain(x => x.PropertyName.Equals("Name", StringComparison.OrdinalIgnoreCase));
        }
        
        [Fact]
        public void ShouldNotSaveTheActor()
        {
            actorProvider.HasNoMatch(x => x.Id == _actor.Id)
                .Should().BeTrue();
        }
    }
}