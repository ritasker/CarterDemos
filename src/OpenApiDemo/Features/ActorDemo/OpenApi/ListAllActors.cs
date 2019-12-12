using System.Collections.Generic;
using Carter.OpenApi;

namespace OpenApiDemo.Features.ActorDemo.OpenApi
{
    public class ListAllActors : RouteMetaData
    {
        public override string Description => "Returns a list of actors";

        public override RouteMetaDataResponse[] Responses => new[]
        {
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = $"A list of {nameof(Actor)}s",
                Response = typeof(IEnumerable<Actor>)
            }
        };

        public override string Tag => "Actors";

        public override string OperationId => "listActors";
    }
}