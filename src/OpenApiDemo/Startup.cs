using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenApiDemo.Features.ActorDemo;

namespace OpenApiDemo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCarter(options =>
            {
                options.OpenApi.DocumentTitle = "Carter OpenAPI Sample"; 
                options.OpenApi.ServerUrls = new[] { "http://localhost:5000" };
            });
            services.AddSingleton<IActorProvider, ActorProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseSwaggerUI(opt =>
            {
                opt.RoutePrefix = "openapi/ui";
                opt.SwaggerEndpoint("/openapi", "Carter OpenAPI Sample");
            });
            app.UseEndpoints(builder => builder.MapCarter());
        }
    }
}