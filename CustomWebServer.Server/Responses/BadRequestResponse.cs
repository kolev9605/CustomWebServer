using CustomWebServer.Server.HTTP;

namespace CustomWebServer.Server.Responses;

public class BadRequestResponse : Response
{
    public BadRequestResponse() 
        : base(StatusCode.BadRequest)
    {
    }
}
