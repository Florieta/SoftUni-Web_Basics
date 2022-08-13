using HttpWebServer.HTTP;
using HttpWebServer.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HttpWebServer
{
    public  class HttpServer
    {
        private readonly IPAddress ipAddress;

        private readonly int port;

        private readonly TcpListener serverListener;

        private readonly RoutingTable routingTable;

        public HttpServer(string _ipAddress, int _port, Action<IRoutingTable> routingTableConfiguration)
        {
            ipAddress = IPAddress.Parse(_ipAddress);
            port = _port;

            serverListener = new TcpListener(ipAddress, port);

            routingTableConfiguration(this.routingTable = new RoutingTable());
        }

        public HttpServer(int _port, Action<IRoutingTable> routingTable)
            : this("127.0.0.1", _port, routingTable)
        {

        }

        public HttpServer(Action<IRoutingTable> routingTable)
            : this(8080, routingTable)
        {

        }
        public async Task Start()
        {
            serverListener.Start();
            
            Console.WriteLine($"Server is listening {port} port");

            Console.WriteLine("Listening for requests");

            while (true)
            {
                var connection = await serverListener.AcceptTcpClientAsync();
                _ = Task.Run(async () =>
                {
                    var networkStream = connection.GetStream();
                    string requestText = await ReadRequest(networkStream);
                    Console.WriteLine(requestText);

                    var request = Request.Parse(requestText);
                    var response = this.routingTable.MatchRequest(request);

                    if (response.PreRenderAction != null)
                    {
                        response.PreRenderAction(request, response);
                    }

                    AddSession(request, response);
                    await WriteResponse(networkStream, response);

                    connection.Close();
                });
                

            }

        }
        private static void AddSession(Request request, Response response)
        {
            var sessionExists = request.Session.ContainsKey(Session.SessionCurrentDateKey);
            if(!sessionExists)
            {
                request.Session[Session.SessionCurrentDateKey] = DateTime.Now.ToString();
                response.Cookies.Add(Session.SessionCookieName, request.Session.Id);
            }
        }
        private async Task WriteResponse(NetworkStream networkStream, string content)
        {
            
            var responseBytes = Encoding.UTF8.GetBytes(content.ToString());

            await networkStream.WriteAsync(responseBytes);
        }

        private async Task<string> ReadRequest(NetworkStream networkStream)
        {
            byte[] buffer = new byte[1024];
            StringBuilder request = new StringBuilder();
            var totalBytes = 0;

            do
            {
               var bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                totalBytes += bytesRead;
                if(totalBytes > 10 * 1024)
                {
                    throw new InvalidOperationException("Request is too large");
                }
                request.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            } while (networkStream.DataAvailable);

            return request.ToString();
        }
    }
}
