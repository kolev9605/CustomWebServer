using CustomWebServer.Core;

namespace CustomWebServer.Server.Common;

public static class Guard
{
    public static void AgainstNull(object value, string name = "Value")
    {
        if (value == null)
        {
            throw new ArgumentNullException(string.Format(ErrorMessages.Validations.ValueCannotBeNull, name));
        }
    }
}
