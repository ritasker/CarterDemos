using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Carter;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace CarterDemos.ContentNegotiationDemo
{
    public class XmlNegotiator : IResponseNegotiator
    {
        private readonly List<string> _validMediaTypes = new List<string> {"text/xml", "application/xml"};
        
        public bool CanHandle(MediaTypeHeaderValue accept)
        {
            return _validMediaTypes.Contains(accept.ToString(), StringComparer.OrdinalIgnoreCase) ||
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