using CustomWebServer.Server.Attributes;
using CustomWebServer.Server.Controllers;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.HTTP.Collections;
using CustomWebServer.Server.Responses;

namespace CustomWebServer.Console.Controllers;

public class UserController : Controller
{
    private const string Username = "user";
    private const string Password = "user123";

    public UserController(Request request) 
        : base(request)
    {
    }

    public Response Login() => View();

    [HttpPost]
    public Response LoginUser(string username, string password)
    {
        Request.Session.Clear();

        var usernameMatches = username == Username;
        var passwordMatches = password == Password;

        if (usernameMatches && passwordMatches)
        {
            if (!Request.Session.ContainsKey(Session.SessionUserKey))
            {
                Request.Session[Session.SessionUserKey] = "MyUserId";

                var cookies = new CookieCollection();
                cookies.Add(Session.SessionCookieName, Request.Session.Id);

                return Html("<h3>Logged successfully!</h3>", cookies);
            }

            return Html("<h3>Logged successfully!</h3>");
        }

        return Redirect("/Login");
    }

    public Response Logout()
    {
        Request.Session.Clear();

        return Html("<h3>Logget out</h3>");
    }

    public Response GetUserData()
    {
        if (Request.Session.ContainsKey(Session.SessionUserKey))
        {
            return Text($"Current logged in user: {Username}.");
        }

        return Redirect("/Login");
    }
}

