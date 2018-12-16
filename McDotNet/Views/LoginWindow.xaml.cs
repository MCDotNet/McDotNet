using System.Windows;
using McDotNet.ViewModels;

namespace McDotNet.Views
{
    /// <summary>
    /// Logique d'interaction pour LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginViewModel _vm = new LoginViewModel();
        public LoginWindow()
        {
            DataContext = _vm;
            InitializeComponent();
            PropertyChangedObject.RegisterWindow(this);
        }

        private void RememberMe_Checked(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void OfflineMode_Checked(object sender, RoutedEventArgs e)
        {
            
        } 
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _vm.ProcessLogin(Pass.Password);
            Properties.Settings.Default.Save();
            SettingsManager.SaveSettings();
            this.Close();
        }
    }
}
