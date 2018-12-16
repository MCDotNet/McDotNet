using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet.ViewModels
{
    public class UserCredentialsViewModel : ViewModelBase<UserCredentials>
    {
        public UserCredentialsViewModel() : this(null)
        {
        }

        public UserCredentialsViewModel(UserCredentials model = null) : base(model)
        {
        }

        public string Username
        {
            get => Model.Username;
            set
            {
                Model.Username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => Model.Password;
            set
            {
                Model.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public bool IsOffline
        {
            get => Model.IsOffline;
            set
            {
                Model.IsOffline = value;
                OnPropertyChanged(nameof(IsOffline));
            }
        }
    }
}
