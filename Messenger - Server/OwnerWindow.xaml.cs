using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messenger___Server
{
    public partial class OwnerWindow : Window
    {
        TcpServer _tcpServer;
        public OwnerWindow()
        {
            InitializeComponent();
            InitializeServer();
        }

        private async Task InitializeServer()
        {
            _tcpServer = new TcpServer(messageChatLbx);
            await _tcpServer.StartServer();
        }

        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            string message = inputMessageTbx.Text;

            if (message.Trim().ToLower() == "/disconnect")
            {
                Close();
            }
            else
            {
                foreach (var client in _tcpServer.GetConnectedClients())
                {
                    _tcpServer.SendMessage(client, message);
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _tcpServer.StopServer();
            base.OnClosing(e);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void logChatBtn_Click(object sender, RoutedEventArgs e)
        {
            LogWindow logWindow = new LogWindow();
            logWindow.Show();
        }
    }
}