using CustomWebServer.Core;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.HTTP.Collections;
using System.Text;

namespace CustomWebServer.Server.Responses;

public class Response
{
    public Response(StatusCode statusCode)
    {
        StatusCode = statusCode;

        Headers.Add(Constants.HeaderNames.Server, "My Custom Web Server");
        Headers.Add(Constants.HeaderNames.Date, $"{DateTime.UtcNow:r}");
    }

    public StatusCode StatusCode { get; init; }

    public HeaderCollection Headers { get; } = new HeaderCollection();

    public CookieCollection Cookies { get; } = new CookieCollection();

    public string Body { get; set; }

    public override string ToString()
    {
        var result = new StringBuilder();
        result.AppendLine($"HTTP/1.1 {(int)StatusCode} {StatusCode}");
        foreach (var header in Headers)
        {
            result.AppendLine(header.ToString());
        }

        foreach (var cookie in Cookies)
        {
            result.AppendLine($"{Constants.HeaderNames.SetCookie}: {cookie}");
        }

        result.AppendLine();

        if (!string.IsNullOrEmpty(Body))
        {
            result.Append(Body);
        }

        return result.ToString();
    }
}
