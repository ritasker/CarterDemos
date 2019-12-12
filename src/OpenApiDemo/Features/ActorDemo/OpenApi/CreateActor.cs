using System;
using Carter.OpenApi;

namespace OpenApiDemo.Features.ActorDemo.OpenApi
{
    public class CreateActor : RouteMetaData
    {
        public override string Description => "Create an actor in the system";

        public override RouteMetaDataRequest[] Requests => new[]
        {
            new RouteMetaDataRequest
            {
                Request = typeof(Actor)
            }
        };

        public override RouteMetaDataResponse[] Responses => new[]
        {
            new RouteMetaDataResponse { Code = 201, Description = "Created Actors", Response = typeof(Actor)},
            new RouteMetaDataResponse { Code = 422, Description = "Validation Errors" }
        };

        public override string Tag => "Actors";

        public override string OperationId => "createActor";
    }
}