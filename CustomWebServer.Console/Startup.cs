using CustomWebServer.Console.Services;
using CustomWebServer.Server;
using CustomWebServer.Server.Routing;

public static class Startup
{
    public static async Task Main()
    {
        var server = new HttpServer(routes =>
            routes.MapControllers());

        server.SeviceCollection.Add<IUserService, UserService>();

        await server.Start();
    }
}

