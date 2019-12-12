using Carter;
using Carter.Request;
using Carter.Response;

namespace CarterDemos.ContentNegotiationDemo
{
    public class SayHelloModule : CarterModule
    {
        public SayHelloModule()
        {
            Get("hello/{name}", (req, res) =>
            {
                var name = req.RouteValues.As<string>("name");
                return res.Negotiate(new HelloModel {Message = $"Hello {name}"});
            });
        }
    }
}