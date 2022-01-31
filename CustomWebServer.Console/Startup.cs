using CustomWebServer.Server;
using CustomWebServer.Console.Controllers;
using CustomWebServer.Server.Routing;

public static class Startup
{
    public static async Task Main()
    {
        await new HttpServer(routes => routes
            .MapGet<HomeController>("/", c => c.Index())
            .MapGet<HomeController>("/Redirect", c => c.Redirect())
            .MapGet<HomeController>("/Html", c => c.Html())
            .MapPost<HomeController>("/Html", c => c.HtmlPost())
            .MapGet<HomeController>("/Content", c => c.Content())
            .MapPost<HomeController>("/Content", c => c.ContentPost())
            .MapGet<HomeController>("/Cookies", c => c.Cookies())
            .MapGet<HomeController>("/Session", c => c.Session())
            .MapGet<UserController>("/Login", c => c.Login())
            .MapPost<UserController>("/Login", c => c.LoginUser())
            .MapGet<UserController>("/Logout", c => c.Logout())
            .MapGet<UserController>("/UserProfile", c => c.GetUserData())
        ).Start();
    }
}

