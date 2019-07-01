namespace OpenApiSample.Features.OpenApi
{
    using System.Collections.Generic;
    using Carter.OpenApi;

    public class GetActors : RouteMetaData
    {
        public override string Description { get; } = "Returns a list of actors";

        public override RouteMetaDataResponse[] Responses { get; } =
        {
            new RouteMetaDataResponse
            {
                Code = 200,
                Description = $"A list of {nameof(Actor)}s",
                Response = typeof(IEnumerable<Actor>)
            }
        };

        public override string Tag { get; } = "Actors";

        public override string OperationId { get; } = "Actors_GetActors";
    }
}