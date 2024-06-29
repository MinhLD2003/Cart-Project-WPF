using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Networking
{
    public interface IClient : IDisposable
    {
        Task SendRequest(Request request);
        Task<string> ReceiveResponse();
    }

    public class Client : IClient
    {
        private const int BUFF_SIZE = 99999;
        private const int PORT_NUMBER = 8080;
        private IPEndPoint _ipEndpoint;
        private Socket _clientSocket;
        private bool IsDisposed;

        public Client()
        {
            try
            {
                _ipEndpoint = new IPEndPoint(IPAddress.Loopback, PORT_NUMBER);
                _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _clientSocket.Connect(_ipEndpoint);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Dispose();
            }
        }

        public async Task SendRequest(Request request)
        {
            if (IsDisposed) return;

            try
            {
                string jsonString = JsonConvert.SerializeObject(request);
                var messageBytes = Encoding.UTF8.GetBytes(jsonString);
                await _clientSocket.SendAsync(new ArraySegment<byte>(messageBytes), SocketFlags.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Dispose();
            }
        }

        public async Task<string> ReceiveResponse()
        {
            if (IsDisposed) return null;

            byte[] buffer = new byte[BUFF_SIZE];
            try
            {
                int totalReceived = 0;
                do
                {
                    var received = await _clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer, totalReceived, BUFF_SIZE - totalReceived), SocketFlags.None);
                    if (received == 0) break;
                    totalReceived += received;
                }
                while (_clientSocket.Available > 0);

                if (totalReceived == 0) return null;

                var response = Encoding.UTF8.GetString(buffer, 0, totalReceived);
                Console.WriteLine(response);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Dispose();
                return null;
            } 
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                _clientSocket?.Close();
                _clientSocket?.Dispose();
                IsDisposed = true;
            }
        }
    }
}
