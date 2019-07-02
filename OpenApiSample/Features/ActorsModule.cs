namespace OpenApiSample.Features
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;
    using OpenApi;

    public class ActorsModule : CarterModule
    {
        public ActorsModule(ActorProvider actorProvider) : base("/actors")
        {
            Get<GetActors>("", (req, res, routeData) =>
            {
                var people = actorProvider.Get();
                return res.Negotiate(people);
            });

            Get<GetActorsById>("{id:int}", (req, res, routeData) =>
            {
                try
                {
                    var person = actorProvider.Get(routeData.As<int>("id"));
                    return res.Negotiate(person);
                }
                catch (KeyNotFoundException)
                {
                    res.StatusCode = 404;
                    return Task.CompletedTask;
                }
            });
            
            Post<CreateActor>("", (req, res, routeData) =>
            {
                var result = req.BindAndValidate<Actor>();

                if (!result.ValidationResult.IsValid)
                {
                    res.StatusCode = 422;
                    return res.Negotiate(result.ValidationResult.GetFormattedErrors());
                }

                var actor = actorProvider.Create(result.Data);

                res.StatusCode = 201;
                return res.Negotiate(actor);
            });

            Put<UpdateActor>("{id:int}", (req, res, routeData) =>
            {
                var actor = req.Bind<Actor>();
                actor.Id = routeData.As<int>("id");
                
                var result = req.Validate(actor);

                if (!result.IsValid)
                {
                    res.StatusCode = 422;
                    return res.Negotiate(result.GetFormattedErrors());
                }

                actorProvider.Update(actor);
                res.StatusCode = 204;
                return res.Negotiate(actor);
            });

            Delete<DeleteActor>("{id:int}", (req, res, routeData) =>
            {
                actorProvider.Delete(routeData.As<int>("id"));
                res.StatusCode = 204;
                return Task.CompletedTask;
            });
        }
    }
}