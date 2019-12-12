using Carter.OpenApi;

namespace OpenApiDemo.Features.ActorDemo.OpenApi
{
    public class UpdateActor : RouteMetaData
    {
        public override string Description => "Update an existing actor";
        
        public override RouteMetaDataRequest[] Requests => new[]
        {
            new RouteMetaDataRequest
            {
                Request = typeof(Actor),
            }
        };

        public override RouteMetaDataResponse[] Responses => new[]
        {
            new RouteMetaDataResponse { Code = 204, Description = "Updated Actor" },
            new RouteMetaDataResponse { Code = 422, Description = "Validation Errors" }
        };
        
        public override string Tag => "Actors";
        
        public override string OperationId => "updateActor";

    }
}