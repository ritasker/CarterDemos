namespace BasicSample
{
    using System.Threading.Tasks;
    using Carter;
    using Microsoft.AspNetCore.Http;

    public class NotFoundHandler : IStatusCodeHandler
    {
        public bool CanHandle(int statusCode)
        {
            return statusCode == 404;
        }

        public Task Handle(HttpContext ctx)
        {
            return ctx.Response.WriteAsync($"No resource found at {ctx.Request.Path.Value}");
        }
    }
}