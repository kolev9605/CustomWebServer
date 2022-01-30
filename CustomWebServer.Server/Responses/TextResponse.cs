using CustomWebServer.Core;

namespace CustomWebServer.Server.Responses;

public class TextResponse : ContentResponse
{
    public TextResponse(string text) 
        : base(text, Constants.ContentType.PlainText)
    {
    }
}
