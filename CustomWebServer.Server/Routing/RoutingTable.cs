using CustomWebServer.Server.Common;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;

namespace CustomWebServer.Server.Routing;

public class RoutingTable : IRoutingTable
{
    private readonly Dictionary<Method, Dictionary<string, Func<Request, Response>>> _routes;

    public RoutingTable()
    {
        _routes = new Dictionary<Method, Dictionary<string, Func<Request, Response>>>()
        {
            { Method.Get, new (StringComparer.InvariantCultureIgnoreCase) },
            { Method.Post, new (StringComparer.InvariantCultureIgnoreCase) },
            { Method.Put, new (StringComparer.InvariantCultureIgnoreCase) },
            { Method.Delete, new (StringComparer.InvariantCultureIgnoreCase) },
        };
    }

    public IRoutingTable Map(Method method, string path, Func<Request, Response> responseFunction)
    {
        Guard.AgainstNull(path, nameof(path));
        Guard.AgainstNull(responseFunction, nameof(responseFunction));

        _routes[method][path] = responseFunction;

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

        var responseFunction = _routes[requestMethod][requestUrl];
        return responseFunction(request);
    }

    public IRoutingTable MapGet(string path, Func<Request, Response> responseFunction)
    {
        Guard.AgainstNull(path, nameof(path));
        Guard.AgainstNull(responseFunction, nameof(responseFunction));

        return Map(Method.Get, path, responseFunction);
    }

    public IRoutingTable MapPost(string path, Func<Request, Response> responseFunction)
    {
        Guard.AgainstNull(path, nameof(path));
        Guard.AgainstNull(responseFunction, nameof(responseFunction));

        return Map(Method.Post, path, responseFunction);
    }
}

