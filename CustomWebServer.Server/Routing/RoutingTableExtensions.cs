using CustomWebServer.Server.Controllers;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;

namespace CustomWebServer.Server.Routing;

public static class RoutingTableExtensions
{
    public static IRoutingTable MapGet<TController>(
        this IRoutingTable routingTable,
        string path,
        Func<TController, Response> controllerFunc)
        where TController : Controller
    {
        return routingTable.MapGet(path, request => controllerFunc(CreateController<TController>(request)));
    }

    public static IRoutingTable MapPost<TController>(
    this IRoutingTable routingTable,
    string path,
    Func<TController, Response> controllerFunc)
    where TController : Controller
    {
        return routingTable.MapPost(path, request => controllerFunc(CreateController<TController>(request)));
    }

    private static TController CreateController<TController>(Request request) 
        where TController : Controller
    {
        var controller = (TController)Activator.CreateInstance(typeof(TController), new[] { request });
        return controller;
    }
}
