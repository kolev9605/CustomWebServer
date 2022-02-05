namespace CustomWebServer.Server.Common;

public static class ErrorMessages
{
    public static class Http
    {
        public static string MethodNotSupported = "Method '{0}' is not supported.";
        public static string RequestIsNotValid = "Request is not valid.";
        public static string RequestIsTooLarge = "Request is too large.";

    }

    public static class Validations
    {
        public static string ValueCannotBeNull = "{0} cannot be null.";
    }
}
