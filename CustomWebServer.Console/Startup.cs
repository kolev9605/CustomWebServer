using CustomWebServer.Core;
using CustomWebServer.Server;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;
using System.Text;
using System.Web;

public static class Startup
{
    public static async Task Main()
    {
        await DownloadSitesAsTextFile("content.txt", new string[] { "https://softuni.org", "https://judge.softuni.org/" });

        HttpServer server = new HttpServer(routes => routes
            .MapGet("/", new TextResponse("Hello from the web server!"))
            .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
            .MapGet("/Html", new HtmlResponse(Constants.Forms.HtmlForm))
            .MapPost("/Html", new TextResponse("empty"))
            .MapGet("/Content", new HtmlResponse(Constants.Forms.DownloadForm))
            .MapPost("/Content", new TextFileResponse("content.txt"))
            .MapPost("/Content", new TextFileResponse("content.txt"))
            .MapGet("/Cookies", new HtmlResponse("", AddCookiesAction))
            .MapGet("/Session", new TextResponse("", DisplaySessionInfoAction))
        );

        await server.Start();
    }

    private static async Task<string> DownloadWebSiteContent(string url)
    {
        var httpClient = new HttpClient();
        using (httpClient)
        {
            var response = await httpClient.GetAsync(url);

            var html = await response.Content.ReadAsStringAsync();

            return html.Substring(0, 2000);
        }
    }

    private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
    {
        var downloads = new List<Task<string>>();

        foreach (var url in urls)
        {
            downloads.Add(DownloadWebSiteContent(url));
        }

        var response = await Task.WhenAll(downloads);

        var responseString = string.Join(Environment.NewLine + new string('-', 100), response);

        await File.WriteAllTextAsync(fileName, responseString);
    }

    private static void AddCookiesAction(Request request, Response response)
    {
        var bodyText = string.Empty;
        if (request.Cookies.Any(c => c.Name != Constants.Session.SessionCookieName))
        {
            var cookieText = new StringBuilder();
            cookieText.AppendLine("<h1>Cookies</h1>");
            cookieText.AppendLine("<table border='1'><tr><th>Name</th><th>Value</th></tr>");
            foreach (var cookie in request.Cookies)
            {
                cookieText.Append("<tr>");
                cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                cookieText.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                cookieText.AppendLine("</tr>");
            }

            cookieText.AppendLine("</table>");
            
            bodyText = cookieText.ToString();
        }
        else
        {
            bodyText = "<h1>Cookies Set!</h1>";
        }

        if (!request.Cookies.Any())
        {
            response.Cookies.Add("My-Cookie", "My-Value");
        }

        response.Body = bodyText;
    }

    private static void DisplaySessionInfoAction(Request request, Response response)
    {
        var sessionExists = request.Session
            .ContainsKey(Constants.Session.SessionCurrentDateKey);

        var bodyText = string.Empty;
        if (sessionExists)
        {
            var currentDate = request.Session[Constants.Session.SessionCurrentDateKey];
            bodyText = $"Stored date: {currentDate}";
        }
        else
        {
            bodyText = "Current date stored!";
        }

        response.Body = bodyText;
    }
}

