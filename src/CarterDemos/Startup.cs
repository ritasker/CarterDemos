namespace CarterDemos
{
    using Carter;
    using CarterDemos.ActorDemo;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCarter();
            services.AddSingleton<IActorProvider, ActorProvider>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(builder => builder.MapCarter());
        }
    }
}