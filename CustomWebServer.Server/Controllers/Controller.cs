using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.HTTP.Collections;
using CustomWebServer.Server.Responses;
using System.Runtime.CompilerServices;

namespace CustomWebServer.Server.Controllers;

public class Controller
{
    public Controller(Request request)
    {
        Request = request;
    }

    protected Request Request { get; set; }

    protected Response Text(string text) => new TextResponse(text);

    protected Response Html(string text, CookieCollection cookies = null)
    {
        var response = new HtmlResponse(text);

        if (cookies != null)
        {
            foreach (var cookie in cookies)
            {
                response.Cookies.Add(cookie.Name, cookie.Value);
            }
        }

        return response;
    }

    protected Response BadRequest() => new BadRequestResponse();

    protected Response Unauthorized() => new UnauthorizedResponse();

    protected Response NotFound() => new NotFoundResponse();

    protected Response Redirect(string location) => new RedirectResponse(location);

    protected Response File(string fileName) => new TextFileResponse(fileName);

    protected Response View([CallerMemberName] string viewName = "", object model = null)
        => new ViewResponse(viewName, GetControllerName(), model);

    private string GetControllerName()
        => GetType().Name.Replace(nameof(Controller), string.Empty);
}

