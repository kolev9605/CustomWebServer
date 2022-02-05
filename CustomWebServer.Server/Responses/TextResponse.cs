using CustomWebServer.Server.Common;

namespace CustomWebServer.Server.Responses;

public class TextResponse : ContentResponse
{
    public TextResponse(string text) 
        : base(text, Constants.ContentType.PlainText)
    {
    }
}
