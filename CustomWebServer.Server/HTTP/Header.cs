using CustomWebServer.Server.Common;

namespace CustomWebServer.Server.HTTP;

public class Header
{
    public Header(string name, string value)
    {
        Guard.AgainstNull(name, nameof(name));
        Guard.AgainstNull(value, nameof(value));

        Name = name;
        Value = value;
    }

    public string Name { get; set; }

    public string Value { get; set; }

    public override string ToString()
    {
        return $"{Name}: {Value}";
    }
}
