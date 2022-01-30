using CustomWebServer.Server.HTTP;

namespace CustomWebServer.Server.Responses;
public class NotFoundResponse : Response
{
    public NotFoundResponse() :
        base(StatusCode.NotFound)
    {
    }
}
