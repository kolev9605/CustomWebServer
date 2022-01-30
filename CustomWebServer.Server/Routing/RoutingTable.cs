using CustomWebServer.Core;
using CustomWebServer.Server.Common;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;

namespace CustomWebServer.Server.Routing;

public class RoutingTable : IRoutingTable
{
    private readonly IDictionary<Method, IDictionary<string, Response>> _routes;

    public RoutingTable()
    {
        _routes = new Dictionary<Method, IDictionary<string, Response>>()
        {
            { Method.Get, new Dictionary<string, Response>(StringComparer.InvariantCultureIgnoreCase) },
            { Method.Post, new Dictionary<string,Response>(StringComparer.InvariantCultureIgnoreCase) },
            { Method.Put, new Dictionary<string, Response>(StringComparer.InvariantCultureIgnoreCase) },
            { Method.Delete, new Dictionary<string, Response>(StringComparer.InvariantCultureIgnoreCase) },
        };
    }

    public IRoutingTable Map(string url, Method method, Response response)
    {
        return method switch
        {
            Method.Get => MapGet(url, response),
            Method.Post => MapPost(url, response),
            _ => throw new InvalidOperationException(string.Format(ErrorMessages.Http.MethodNotSupported, method))
        };
    }

    public IRoutingTable MapGet(string url, Response response)
    {
        Guard.AgainstNull(url, nameof(url));
        Guard.AgainstNull(response, nameof(response));

        _routes[Method.Get][url] = response;

        return this;
    }

    public IRoutingTable MapPost(string url, Response response)
    {
        Guard.AgainstNull(url, nameof(url));
        Guard.AgainstNull(response, nameof(response));

        _routes[Method.Post][url] = response;

        return this;
    }

    public Response MatchRequest(Request request)
    {
        var requestMethod = request.Method;
        var requestUrl = request.Url;

        if (!_routes.ContainsKey(requestMethod)
            || !_routes[requestMethod].ContainsKey(requestUrl))
        {
            return new NotFoundResponse();
        }

        return _routes[requestMethod][requestUrl];
    }
}

