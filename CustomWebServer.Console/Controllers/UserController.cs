using CustomWebServer.Console.Services;
using CustomWebServer.Server.Attributes;
using CustomWebServer.Server.Controllers;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.HTTP.Collections;
using CustomWebServer.Server.Responses;

namespace CustomWebServer.Console.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(Request request, IUserService userService) 
        : base(request)
    {
        _userService = userService;
    }

    public Response Login() => View();

    [HttpPost]
    public Response LoginUser(string username, string password)
    {
        Request.Session.Clear();

        if (_userService.IsLoginCorrect(username, password))
        {
            if (!Request.Session.ContainsKey(Session.SessionUserKey))
            {
                SignIn(Guid.NewGuid().ToString());

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
        SignOut();

        return Html("<h3>Logget out</h3>");
    }

    public Response GetUserData()
    {
        return Html($"<h3>Currently logged-in user is with id '{User.Id}'</h3>");
    }
}

