using Carter.OpenApi;

namespace OpenApiDemo.Features.ActorDemo.OpenApi
{
    public class GetActorById : RouteMetaData
    {
        public override string Description => "Data for a specific actor";

        public override RouteMetaDataResponse[] Responses => new[]
        {
            new RouteMetaDataResponse
            {
                Code = 200, 
                Description = $"An {nameof(Actor)}",
                Response = typeof(Actor)
            },
            new RouteMetaDataResponse
            {
                Code = 404,
                Description = $"{nameof(Actor)} not found"
            }
        };

        public override string Tag => "Actors";

        public override string OperationId => "getActorById";
    }
}