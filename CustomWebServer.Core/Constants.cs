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
        public const string SessionUserKey = "Authenticate";
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
}
