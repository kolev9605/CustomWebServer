using CustomWebServer.Core;
using CustomWebServer.Server.Common;
using CustomWebServer.Server.HTTP;
using System.Text;

namespace CustomWebServer.Server.Responses;

public class ContentResponse : Response
{
    public ContentResponse(string content, string contentType, Action<Request, Response> preRenderAction = null) 
        : base(StatusCode.OK)
    {
        Guard.AgainstNull(content);
        Guard.AgainstNull(contentType);

        PreRenderAction = preRenderAction;

        Headers.Add(Constants.HeaderNames.ContentType, contentType);
        if (Body != null)
        {
            var contentLength = Encoding.UTF8.GetByteCount(Body).ToString();
            Headers.Add(Constants.HeaderNames.ContentLength, contentLength);
        }

        Body = content;
    }
}
