using CustomWebServer.Core;
using CustomWebServer.Server.HTTP;

namespace CustomWebServer.Server.Responses;

public class TextResponse : ContentResponse
{
    public TextResponse(string text, Action<Request, Response> preRenderAction = null) 
        : base(text, Constants.ContentType.PlainText, preRenderAction)
    {
    }
}
