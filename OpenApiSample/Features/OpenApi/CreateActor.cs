namespace OpenApiSample.Features.OpenApi
{
    using System;
    using Carter.OpenApi;

    public class CreateActor : RouteMetaData
    {
        public override string Description { get; } = "Create an actor in the system";

        public override Type Request { get; } = typeof(Actor);

        public override RouteMetaDataResponse[] Responses { get; } = { new RouteMetaDataResponse { Code = 201, Description = "Created Actors" } };

        public override string Tag { get; } = "Actors";

        public override string OperationId { get; } = "Actors_CreateActor";
    }
}