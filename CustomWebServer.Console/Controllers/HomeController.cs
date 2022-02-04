using CustomWebServer.Console.Models;
using CustomWebServer.Core;
using CustomWebServer.Server.Controllers;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.HTTP.Collections;
using CustomWebServer.Server.Responses;
using System.Text;
using System.Web;

namespace CustomWebServer.Console.Controllers;

public class HomeController : Controller
{
    private const string FileName = "content.txt";

    public HomeController(Request request)
        : base(request)
    {
    }

    public Response Index()
    {
        return Text("Hello from the Home controller!");
    }

    public Response Redirect()
    {
        return Redirect("https://softuni.org");
    }

    public Response Html() => View();

    public Response HtmlFormPost()
    {
        var name = Request.Form["Name"];
        var age = Request.Form["Age"];
        var model = new FormViewModel()
        {
            Name = name,
            Age = int.Parse(age),
        };

        return View(model: model);
    }

    public Response Content() => View();

    public Response ContentPost()
    {
        DownloadSitesAsTextFile(FileName, new string[] { "https://softuni.org", "https://judge.softuni.org" })
            .Wait();

        return File(FileName);
    }

    public Response Cookies()
    {
        var bodyText = string.Empty;
        if (Request.Cookies.Any(c => c.Name != Server.HTTP.Session.SessionCookieName))
        {
            var cookieText = new StringBuilder();
            cookieText.AppendLine("<h1>Cookies</h1>");
            cookieText.AppendLine("<table border='1'><tr><th>Name</th><th>Value</th></tr>");
            foreach (var cookie in Request.Cookies)
            {
                cookieText.Append("<tr>");
                cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                cookieText.AppendLine("</tr>");
            }

            cookieText.AppendLine("</table>");

            return Html(cookieText.ToString());
        }

        var cookies = new CookieCollection();
        cookies.Add("My Cookie v2", "My-Cookie-Value");

        return Html("<h1>Cookies Set!</h1>", cookies);

    }

    public Response Session()
    {
        var sessionExists = Request.Session
            .ContainsKey(Server.HTTP.Session.SessionCurrentDateKey);

        var bodyText = string.Empty;
        if (sessionExists)
        {
            var currentDate = Request.Session[Server.HTTP.Session.SessionCurrentDateKey];
            bodyText = $"Stored date: {currentDate}";
        }
        else
        {
            bodyText = "Current date stored!";
        }

        return Html(bodyText);
    }

    private async Task<string> DownloadWebSiteContent(string url)
    {
        var httpClient = new HttpClient();
        using (httpClient)
        {
            var response = await httpClient.GetAsync(url);

            var html = await response.Content.ReadAsStringAsync();

            return html.Substring(0, 2000);
        }
    }

    private async Task DownloadSitesAsTextFile(string fileName, string[] urls)
    {
        var downloads = new List<Task<string>>();

        foreach (var url in urls)
        {
            downloads.Add(DownloadWebSiteContent(url));
        }

        var response = await Task.WhenAll(downloads);

        var responseString = string.Join(Environment.NewLine + new string('-', 100), response);

        await System.IO.File.WriteAllTextAsync(fileName, responseString);
    }
}

