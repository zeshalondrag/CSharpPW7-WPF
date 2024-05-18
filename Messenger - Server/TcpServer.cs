using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Messenger___Server
{
    public class TcpServer
    {
        private ListBox _messageChatLbx;
        private Socket _socket;
        private List<Socket> _clients = new List<Socket>();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public event EventHandler<LogEventArgs> LogEvent;

        public TcpServer(ListBox messageChatLbx)
        {
            _messageChatLbx = messageChatLbx;
        }

        public async Task StartServer()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(new IPEndPoint(IPAddress.Any, 6565));
            _socket.Listen(100);

            ListenToClients(_cts.Token);
        }

        private async void ListenToClients(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var client = await _socket.AcceptAsync(token);
                    _clients.Add(client);
                    LogEvent?.Invoke(this, new LogEventArgs($"[{DateTime.Now:HH:mm:ss dd/MM/yyyy}] Новый клиент подключился."));
                    RecieveMessage(client, token);
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show("Сервер успешно остановлен!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            _socket.Close();
        }

        private async Task RecieveMessage(Socket c, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                byte[] bytes = new byte[1024];
                int bytesReceived = await c.ReceiveAsync(bytes, SocketFlags.None);
                if (bytesReceived > 0)
                {
                    string message = Encoding.UTF8.GetString(bytes, 0, bytesReceived);
                    foreach (var item in _clients)
                    {
                        SendMessage(item, message);
                    }
                }
            }
        }

        public async Task SendMessage(Socket c, string messageContent)
        {
            var message = new ChatMessage(messageContent);
            byte[] bytes = Encoding.UTF8.GetBytes(message.ToString());
            await c.SendAsync(bytes, SocketFlags.None);
            _messageChatLbx.Items.Add(message);
        }

        public List<Socket> GetConnectedClients()
        {
            return _clients;
        }

        public void StopServer()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}