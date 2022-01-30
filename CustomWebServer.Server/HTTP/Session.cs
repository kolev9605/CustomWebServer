using CustomWebServer.Server.Common;

namespace CustomWebServer.Server.HTTP;

public class Session
{
    private Dictionary<string, string> _data;

    public Session(string id)
    {
        Guard.AgainstNull(id, nameof(id));
        Id = id;
        _data = new Dictionary<string, string>();
    }

    public string Id { get; set; }

    public string this[string key]
    {
        get => _data[key];
        set => _data[key] = value;
    }

    public bool ContainsKey(string key)
        => _data.ContainsKey(key);

    public void Clear()
        => _data.Clear();
}
