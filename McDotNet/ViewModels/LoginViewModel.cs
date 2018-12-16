using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.ViewModels
{
    public class LoginViewModel : PropertyChangedObject
    {
        public LoginViewModel() : this(null)
        {
        }

        public LoginViewModel(UserCredentials model = null)
        {
            Credentials = new UserCredentialsViewModel(model);
            LoginCommand = new DelegateCommand(ProcessLogin);
        }

        public UserCredentialsViewModel Credentials { get; }

        private bool _rememberMe;

        public bool RememberMe
        {
            get { return _rememberMe; }
            set { _rememberMe = value; OnPropertyChanged(); }
        }
        private bool isOfflineMode;

        public bool IsOfflineMode
        {
            get { return isOfflineMode; }
            set { isOfflineMode = value; OnPropertyChanged();}
        }

        public DelegateCommand LoginCommand { get; private set; }

        public void ProcessLogin(object pass)
        {
            if (RememberMe)
            {
                var profile = new UserProfileViewModel
                {
                    Credentials =
                    {
                        IsOffline = LoginData.IsOfflineMode = IsOfflineMode,
                        Password = LoginData.Password = pass as string,
                        Username = LoginData.Username = Credentials.Username
                    }
                };
                Properties.Settings.Default.remember = RememberMe;
            }
            else
            {
                LoginData.IsOfflineMode = IsOfflineMode;
                LoginData.Password = pass as string;
                LoginData.Username = Credentials.Username;
            }
        }
    }
}
