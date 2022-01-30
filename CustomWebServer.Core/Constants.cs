namespace CustomWebServer.Core;

public static class Constants
{
    public static class HeaderNames
    {
        public const string ContentType = "Content-Type";
        public const string ContentLength = "Content-Length";
        public const string ContentDisposition = "Content-Disposition";
        public const string Date = "Date";
        public const string Location = "Location";
        public const string Server = "Server";
        public const string Cookie = "Cookie";
        public const string SetCookie = "Set-Cookie";
    }

    public static class Session
    {
        public const string SessionCookieName = "MyWebServerSID";
        public const string SessionCurrentDateKey = "CurrentDate";
    }

    public static class ContentType
    {
        public const string PlainText = "text/plain; charset=UTF-8";
        public const string Html = "text/html; charset=UTF-8";
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";
    }

    public static class Server
    {
        public const string LocalhostIp = "127.0.0.1";
    }

    public static class Forms
    {
        public const string HtmlForm = @"
<form action='/HTML' method='POST'>
    Name: <input type='text' name='Name'/>
    Age: <input type='number' name='Age'/>
    <input type='submit' value = 'Save'/>
</form>";

        public const string DownloadForm = @"
<form action ='/Content' method='Post'>
    <input type='submit' value='Download Sites Content' />
</form>";
    }
}
