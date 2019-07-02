namespace SirenSample
{
    using Carter;
    using Features;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using SirenNegotiator;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ActorProvider, ActorProvider>();
            services.AddScoped<ISirenResponseGenerator, ActorResponseGenerator>();
            services.AddScoped<IResponseNegotiator, SirenResponseNegotiator>();
            services.AddCarter();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseCarter();
        }
    }
}
