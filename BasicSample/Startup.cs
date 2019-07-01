namespace BasicSample
{
    using Carter;
    using Features;
    using Microsoft.AspNetCore.Builder;
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
            app.UseCarter();
        }
    }
}
