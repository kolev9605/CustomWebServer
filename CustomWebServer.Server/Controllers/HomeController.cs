using CustomWebServer.Core;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;
using System.Text;
using System.Web;

namespace CustomWebServer.Server.Controllers;

public class HomeController : Controller
{
    private const string HtmlForm = @"
<form action='/HTML' method='POST'>
    Name: <input type='text' name='Name'/>
    Age: <input type='number' name='Age'/>
    <input type='submit' value = 'Save'/>
</form>";

    private const string DownloadForm = @"
<form action ='/Content' method='Post'>
    <input type='submit' value='Download Sites Content' />
</form>";

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

    public Response HtmlGet()
    {
        return Html(HtmlForm);
    }

    public Response HtmlPost()
    {
        string formData = string.Empty;
        foreach (var pair in Request.Form)
        {
            formData += $"{pair.Key} - {pair.Value}";
            formData += Environment.NewLine;
        }

        return Text(formData);
    }

    public Response Content()
    {
        return Html(DownloadForm);
    }

    public Response ContentPost()
    {
        DownloadSitesAsTextFile(FileName, new string[] { "https://softuni.org", "https://judge.softuni.org" })
            .Wait();

        return File(FileName);
    }

    public Response Cookies()
    {
        var bodyText = string.Empty;
        if (Request.Cookies.Any(c => c.Name != Constants.Session.SessionCookieName))
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
            .ContainsKey(Constants.Session.SessionCurrentDateKey);

        var bodyText = string.Empty;
        if (sessionExists)
        {
            var currentDate = Request.Session[Constants.Session.SessionCurrentDateKey];
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

    private void AddCookiesAction()
    {

    }
}

