using CustomWebServer.Server.Common;
using CustomWebServer.Server.HTTP;

namespace CustomWebServer.Server.Responses;

public class RedirectResponse : Response
{
    public RedirectResponse(string location)
        : base(StatusCode.Found)
    {
        Headers.Add(Constants.HeaderNames.Location, location);
    }
}

