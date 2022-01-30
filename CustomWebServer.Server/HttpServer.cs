using CustomWebServer.Core;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;
using CustomWebServer.Server.Routing;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CustomWebServer.Server;

public class HttpServer
{
    private readonly IPAddress _iPAddress;
    private readonly int _port;
    private readonly TcpListener _tcpListener;

    private readonly RoutingTable _routingTable;

    public HttpServer(string ipAddress, int port, Action<IRoutingTable> routingTableConfiguration)
    {
        _iPAddress = IPAddress.Parse(ipAddress);
        _port = port;

        _tcpListener = new TcpListener(_iPAddress, _port);

        routingTableConfiguration(_routingTable = new RoutingTable());
    }

    public HttpServer(int port, Action<IRoutingTable> routingTableConfiguration)
        : this(Constants.Server.LocalhostIp, port, routingTableConfiguration)
    {
    }

    public HttpServer(Action<IRoutingTable> routingTableConfiguration)
        : this(8080, routingTableConfiguration)
    {
    }

    public async Task Start()
    {
        _tcpListener.Start();

        Console.WriteLine($"Server started on port {_port}.");
        Console.WriteLine("Listening for requests...");

        while (true)
        {
            var connection = await _tcpListener.AcceptTcpClientAsync();

            _ = Task.Run(async () =>
            {
                var networkStream = connection.GetStream();

                var requestText = await ReadRequestAsync(networkStream);
                var request = Request.Parse(requestText);

                var response = _routingTable.MatchRequest(request);

                AddSession(request, response);

                await WriteResponseAsync(networkStream, response);
                connection.Close();
            });

        }
    }

    private void AddSession(Request request, Response response)
    {
        if (!request.Session.ContainsKey(Constants.Session.SessionCurrentDateKey))
        {
            request.Session[Constants.Session.SessionCurrentDateKey] = DateTime.Now.ToString();
            response.Cookies.Add(Constants.Session.SessionCookieName, request.Session.Id);
        }
    }

    private async Task WriteResponseAsync(NetworkStream networkStream, Response response)
    {
        var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

        await networkStream.WriteAsync(responseBytes);
    }

    private async Task<string> ReadRequestAsync(NetworkStream networkStream)
    {
        var bufferLength = 1024;
        var buffer = new byte[bufferLength];
        var totalBytes = 0;

        var requestBuilder = new StringBuilder();
        do
        {
            var bytesRead = await networkStream.ReadAsync(buffer, 0, bufferLength);
            totalBytes += bytesRead;

            if (totalBytes > 10 * 1024)
            {
                throw new InvalidOperationException(ErrorMessages.Http.RequestIsTooLarge);
            }

            requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
        }
        while (networkStream.DataAvailable);

        return requestBuilder.ToString();
    }
}
