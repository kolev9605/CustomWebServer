using CustomWebServer.Core;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;

namespace CustomWebServer.Server.Controllers;

public class UserController : Controller
{
    private const string LoginForm = @"
<form action ='/Login' method='Post'>
    <input type='text' name='Username' />
    <input type='text' name='Password' />
    <input type='submit' value='Log In' />
</form>";

    private const string Username = "user";
    private const string Password = "user123";

    public UserController(Request request) 
        : base(request)
    {
    }

    public Response Login()
    {
        return Html(LoginForm);
    }

    public Response LoginUser()
    {
        Request.Session.Clear();

        var usernameMatches = Request.Form["Username"] == Username;
        var passwordMatches = Request.Form["Password"] == Password;

        if (usernameMatches && passwordMatches)
        {
            if (!Request.Session.ContainsKey(Constants.Session.SessionUserKey))
            {
                Request.Session[Constants.Session.SessionUserKey] = "MyUserId";

                var cookies = new CookieCollection();
                cookies.Add(Constants.Session.SessionCookieName, Request.Session.Id);

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
        if (Request.Session.ContainsKey(Constants.Session.SessionUserKey))
        {
            return Text($"Current logged in user: {Username}.");
        }

        return Redirect("/Login");
    }
}

