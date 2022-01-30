using CustomWebServer.Server.HTTP;

namespace CustomWebServer.Server.Responses;

public class UnauthorizedResponse : Response
{
    public UnauthorizedResponse() 
        : base(StatusCode.Unauthorized)
    {
    }
}
