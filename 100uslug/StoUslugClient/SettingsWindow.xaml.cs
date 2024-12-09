using StoUslug.Client.Services;
using System;
using System.Collections.Generic;
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

namespace StoUslugClient
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ISettingsDataService _settingsDataService;

        public SettingsWindow(ISettingsDataService settingsDataService)
        {
            InitializeComponent();
            _settingsDataService = settingsDataService;
            LoadData();
        }

        private void LoadData()
        {
            var settings = _settingsDataService.GetSettings();
            this.LoginTextBox.Text = settings.Login;
            this.ServerTextBox.Text = settings.Server;
            this.PasswordTextBox.Password = settings.Password;
        }
    }
}
