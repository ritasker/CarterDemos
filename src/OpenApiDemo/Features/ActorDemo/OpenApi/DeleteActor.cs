using Carter.OpenApi;

namespace OpenApiDemo.Features.ActorDemo.OpenApi
{
    public class DeleteActor: RouteMetaData
    {
        public override string Description => "Delete an actor";

        public override RouteMetaDataResponse[] Responses => new[]
        {
            new RouteMetaDataResponse { Code = 204, Description = "Deleted Actor" },
        };

        public override string Tag => "Actors";

        public override string OperationId => "deleteActor";
    }
}