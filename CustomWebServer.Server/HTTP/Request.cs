using CustomWebServer.Server.Common;
using CustomWebServer.Server.HTTP.Collections;
using CustomWebServer.Server.Services;
using System.Web;

namespace CustomWebServer.Server.HTTP;

public class Request
{
    private static Dictionary<string, Session> _sessions = new();

    public Method Method { get; private set; }

    public string Url { get; private set; }

    public HeaderCollection Headers { get; private set; }

    public CookieCollection Cookies { get; private set; }

    public string Body { get; set; }

    public Session Session { get; set; }

    public IReadOnlyDictionary<string, string> Form { get; private set; }

    public IReadOnlyDictionary<string, string> Query { get; private set; }

    public static IServiceCollection ServiceCollection { get; private set; }

    public static Request Parse(string request, IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;

        var lines = request.Split("\r\n");
        var startLine = lines.First().Split(" ");
        var method = ParseMethod(startLine[0]);
        var parsedUrl = ParseUrl(startLine[1]);
        var headers = ParseHeaders(lines.Skip(1));
        var cookies = ParseCookies(headers);
        var session = GetSession(cookies);
        var bodyLines = lines.Skip(headers.Count + 2).ToArray();
        var body = string.Join("\r\n", bodyLines);
        var form = ParseForm(headers, body);

        return new Request
        {
            Method = method,
            Url = parsedUrl.url,
            Headers = headers,
            Cookies = cookies,
            Body = body,
            Session = session,
            Form = form,
            Query = parsedUrl.queryParameters
        };
    }

    private static (string url, Dictionary<string, string> queryParameters) ParseUrl(string fullUrl)
    {
        var urlParts = fullUrl.Split('?', 2);

        var path = urlParts[0];
        var query = new Dictionary<string, string>();
        if (urlParts.Length == 2)
        {
            query = ParseQuery(urlParts[1]);
        }

        return (path, query);
    }

    private static Dictionary<string, string> ParseQuery(string queryString)
    {
        return HttpUtility.UrlDecode(queryString)
            .Split('&')
            .Select(part => part.Split('='))
            .Where(part => part.Length == 2)
            .ToDictionary(
                part => part[0],
                part => part[1],
                StringComparer.InvariantCultureIgnoreCase);
    }

    private static Session GetSession(CookieCollection cookies)
    {
        var sessionId = cookies.Contains(Session.SessionCookieName)
            ? cookies[Session.SessionCookieName]
            : Guid.NewGuid().ToString();

        if (!_sessions.ContainsKey(sessionId))
        {
            _sessions[sessionId] = new Session(sessionId);
        }

        return _sessions[sessionId];
    }

    private static CookieCollection ParseCookies(HeaderCollection headers)
    {
        var cookieCollection = new CookieCollection();

        if (headers.Contains(Constants.HeaderNames.Cookie))
        {
            var cookieHeader = headers[Constants.HeaderNames.Cookie];
            var allCookies = cookieHeader.Split(';');
            foreach (var cookie in allCookies)
            {
                var parts = cookie.Split('=');
                var name = parts[0].Trim();
                var value = parts[1].Trim();

                cookieCollection.Add(name, value);
            }
        }

        return cookieCollection;

    }

    private static Dictionary<string, string> ParseForm(HeaderCollection headers, string body)
    {
        var formCollection = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        if (headers.Contains(Constants.HeaderNames.ContentType)
            && headers[Constants.HeaderNames.ContentType] == Constants.ContentType.FormUrlEncoded)
        {
            var parsedResult = ParseFormData(body);
            foreach (var item in parsedResult)
            {
                formCollection.Add(item.Key, item.Value);
            }
        }

        return formCollection;
    }

    private static IDictionary<string, string> ParseFormData(string bodyLines)
    {
        return HttpUtility.UrlDecode(bodyLines)
            .Split('&')
            .Select(part => part.Split('='))
            .Where(part => part.Length == 2)
            .ToDictionary(part => part[0], part => part[1], StringComparer.InvariantCultureIgnoreCase);
    }

    private static HeaderCollection ParseHeaders(IEnumerable<string> headerLines)
    {
        var headers = new HeaderCollection();

        foreach (var headerLine in headerLines)
        {
            if (headerLine == string.Empty)
            {
                break;
            }

            var headerParts = headerLine.Split(":", 2);
            if (headerParts.Length != 2)
            {
                throw new InvalidOperationException(ErrorMessages.Http.RequestIsNotValid);
            }

            var headerName = headerParts[0].Trim();
            var headerValue = headerParts[1].Trim();

            headers.Add(headerName, headerValue);
        }

        return headers;
    }

    private static Method ParseMethod(string method)
    {
        try
        {
            return Enum.Parse<Method>(method, true);
        }
        catch (Exception)
        {
            throw new InvalidOperationException(string.Format(ErrorMessages.Http.MethodNotSupported, method));
        }
    }
}