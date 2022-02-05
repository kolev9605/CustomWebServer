using CustomWebServer.Server;
using CustomWebServer.Console.Controllers;
using CustomWebServer.Server.Routing;

public static class Startup
{
    public static async Task Main()
    {
        await new HttpServer(routes => 
            routes.MapControllers())
            .Start();
    }
}

