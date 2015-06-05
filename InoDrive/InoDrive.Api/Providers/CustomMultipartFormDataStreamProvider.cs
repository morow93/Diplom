using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace InoDrive.Api.Providers
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path)
        { }

        public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
        {
            var guid = Guid.NewGuid().ToString();
            var extention = headers.ContentDisposition.FileName.Split('.').Last();
            //headers.ContentType.MediaType.Split('/').Last()
            var name = "BodyPart_" + guid + "." + extention;
            return name.Replace("\"", string.Empty);
            //this is here because Chrome submits files in quotation marks 
            //which get treated as part of the filename and get escaped
        }
    }
}