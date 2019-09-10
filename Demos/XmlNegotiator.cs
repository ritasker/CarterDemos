namespace Demos
{
    using Carter;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using Carter.Response;

    public class XmlNegotiator : IResponseNegotiator
    {
        private readonly string[] validMediaTypes = {"text/xml", "application/xml"};
        
        public bool CanHandle(MediaTypeHeaderValue accept)
        {
            return validMediaTypes.Contains(accept.ToString(), StringComparer.OrdinalIgnoreCase) ||
                   accept.SubType.EndsWith("xml", StringComparison.OrdinalIgnoreCase);
        }

        public Task Handle(HttpRequest req, HttpResponse res, object model, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream())  
            {  
                var xmlSerializer = new XmlSerializer(model.GetType());  
                xmlSerializer.Serialize(XmlWriter.Create(stream), model);
                stream.Flush();  
                stream.Seek(0, SeekOrigin.Begin);
                return res.FromStream(stream, "application/xml");
            }  
        }
    }
}