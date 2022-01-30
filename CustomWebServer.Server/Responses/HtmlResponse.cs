using CustomWebServer.Core;
using CustomWebServer.Server.HTTP;

namespace CustomWebServer.Server.Responses;

public class HtmlResponse : ContentResponse
{
    public HtmlResponse(string text)
        : base(text, Constants.ContentType.Html)
    {
    }
}
