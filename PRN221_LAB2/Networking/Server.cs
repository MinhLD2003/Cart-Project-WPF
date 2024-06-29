using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Networking
{
    public class Server : IDisposable
    {
        private const int BUFF_SIZE = 99999;
        private const int PORT_NUMBER = 8080;
        private IPEndPoint _ipEndpoint;
        private Socket _serverSocket;
        private bool _disposed = false;
        private readonly RoutingHandler _routingHandler;

        public Server(RoutingHandler routingHandler)
        {
            _routingHandler = routingHandler;
            try
            {
                _ipEndpoint = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(_ipEndpoint);
                _serverSocket.Listen(100);
                Task.Run(() => HandleRequests());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _serverSocket?.Close();
            }
        }

        public async Task HandleRequests()
        {
            while (true)
            {
                try
                {
                    var clientSocket = await _serverSocket.AcceptAsync();
                    _ = Task.Run(() => HandleClientAsync(clientSocket));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private async Task HandleClientAsync(Socket clientSocket)
        {
            byte[] buffer = new byte[BUFF_SIZE];
            try
            {
                while (true)
                {
                    var receivedBytes = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    if (receivedBytes == 0)
                    {
                        break;
                    }

                    var requestString = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                    Request request = JsonConvert.DeserializeObject<Request>(requestString);
                    var controller = _routingHandler.MatchRoute(request);

                    var result = controller.Execute(request);
                    string responseString = JsonConvert.SerializeObject(result);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(responseString);
                    await clientSocket.SendAsync(new ArraySegment<byte>(responseBytes), SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
            finally
            {
                try
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during socket shutdown: {ex.Message}");
                }
                clientSocket.Close();
            }
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                _serverSocket?.Close();
                _serverSocket?.Dispose();
                _disposed = true;
            }
        }
    }
}
