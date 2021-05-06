using LocalBD;
using StoUslugClient.Service;
using System.Linq;
using System.Windows;

namespace StoUslugClient
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class AuthForm : Window
    {
        public string UserName { get; set; }

        public AuthForm()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            LocalBD.Model.AuthResponse resp = Auth.AuthRequest(new LocalBD.Model.AuthRequest()
            {
                Login = loginTextBox.Text,
                Password = passwordBox.Password
            });

            if (resp.IsAuth)
            {
                Session.SaveSession(resp.User.Name, resp.User.Password, resp.User.Servers.Select(s => new Server()
                {
                    Url = s.Url,
                    Login = s.Login,
                    Name = s.Name,
                    Password = s.Password
                }).ToArray());
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль пользователя", "Неверные учетные данные", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
