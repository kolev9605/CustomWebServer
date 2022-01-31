using System.Collections;

namespace CustomWebServer.Server.HTTP.Collections;

public class HeaderCollection : IEnumerable<Header>
{
    private readonly IDictionary<string, Header> _headers;

    public HeaderCollection()
    {
        _headers = new Dictionary<string, Header>();
    }

    public int Count => _headers.Count;

    public string this[string name]
    {
        get
        {
            return _headers[name].Value;
        }
    }

    public bool Contains(string name)
    {
        return _headers.ContainsKey(name);
    }

    public void Add(string name, string value)
    {
        _headers[name] = new Header(name, value);
    }

    public IEnumerator<Header> GetEnumerator()
    {
        return _headers.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
