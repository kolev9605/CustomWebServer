using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;

namespace CustomWebServer.Server.Routing;

public interface IRoutingTable
{
    IRoutingTable Map(string url, Method method, Response response);

    IRoutingTable MapGet(string url, Response response);

    IRoutingTable MapPost(string url, Response response);
}
