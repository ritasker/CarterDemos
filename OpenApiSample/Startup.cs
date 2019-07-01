namespace OpenApiSample
{
    using System.Collections.Generic;
    using Carter;
    using Features;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ActorProvider, ActorProvider>();
            services.AddCarter();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwaggerUI(opt =>
            {
                opt.RoutePrefix = "openapi/ui";
                opt.SwaggerEndpoint("/openapi", "Carter OpenAPI Sample");
            });


            app.UseCarter(new CarterOptions(openApiOptions:
                new OpenApiOptions("Carter <3 OpenApi", app.ServerFeatures.Get<IServerAddressesFeature>().Addresses,
                    new Dictionary<string, OpenApiSecurity>())
            ));
        }
    }
}
