using System.Collections.Generic;
using System.Threading.Tasks;
using Carter;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;

namespace CarterDemos.ActorDemo
{
    public class ActorModule : CarterModule
    {
        public ActorModule(IActorProvider actorProvider)
        {
            Get("/actors", (req, res) =>
            {
                var actors = actorProvider.Get();
                return res.Negotiate(actors);
            });

            Get("/actors/{id:int}", (req, res) =>
            {
                try
                {
                    var actor = actorProvider.Get(req.RouteValues.As<int>("id"));
                    return res.Negotiate(actor);
                }
                catch (KeyNotFoundException)
                {
                    res.StatusCode = 404;
                    return Task.CompletedTask;
                }
            });
            
            Post("/actors", async (req, res) =>
            {
                var result = await req.BindAndValidate<Actor>();

                if (!result.ValidationResult.IsValid)
                {
                    res.StatusCode = 422;
                    await res.Negotiate(result.ValidationResult.GetFormattedErrors());
                    return;
                }

                var actor = actorProvider.Create(result.Data);

                res.StatusCode = 201;
                await res.Negotiate(actor);
            });

            Put("/actors/{id:int}", async (req, res) =>
            {
                var result = await req.BindAndValidate<Actor>();

                if (!result.ValidationResult.IsValid)
                {
                    res.StatusCode = 422;
                    await res.Negotiate(result.ValidationResult.GetFormattedErrors());
                    return;
                }

                result.Data.Id = req.RouteValues.As<int>("id");
                actorProvider.Update(result.Data);
                res.StatusCode = 204;
            });
            
            Post("/actors/{id:int}/avatar", async (req, res) =>
            {
                try
                {
                    var actor = actorProvider.Get(req.RouteValues.As<int>("id"));
                    var file = await req.BindFile();

                    actor.Avatar = file.FileName;
                    
                    actorProvider.Update(actor);
                    
                    await res.Negotiate(actor);
                }
                catch (KeyNotFoundException)
                {
                    res.StatusCode = 404;
                }
            });

            Delete("/actors/{id:int}", (req, res) =>
            {
                actorProvider.Delete(req.RouteValues.As<int>("id"));
                res.StatusCode = 204;
                return Task.CompletedTask;
            });
        }
    }
}
