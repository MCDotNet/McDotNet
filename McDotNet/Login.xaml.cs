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

namespace McDotNet
{
    /// <summary>
    /// Logique d'interaction pour Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            
            InitializeComponent();
            if (Properties.Settings.Default.remember)
            {
                // this.Close();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RememberMe.IsChecked.Value) {
                Properties.Settings.Default.offline_mode = LoginData.IsOfflineMode = OfflineMode.IsChecked.Value;
                Properties.Settings.Default.password = LoginData.Password = Pass.Password;
                Properties.Settings.Default.username = LoginData.Username = User.Text;
                Properties.Settings.Default.remember = RememberMe.IsChecked.Value;
            }
            else
            {
                LoginData.IsOfflineMode = OfflineMode.IsChecked.Value;
                LoginData.Password = Pass.Password;
                LoginData.Username = User.Text;
            }
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
