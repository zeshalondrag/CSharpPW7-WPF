using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Messenger___Server
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void newChat_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                OwnerWindow ownerWindow = new OwnerWindow();
                ownerWindow.Show();
                Close();
            }
        }
    }
}