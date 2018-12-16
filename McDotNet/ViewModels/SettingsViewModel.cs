
using System;
using System.Linq;

namespace McDotNet.ViewModels
{
    public class SettingsViewModel : ViewModelBase<SettingsModel>
    {
        public SettingsViewModel() : this(null)
        {
        }

        public SettingsViewModel(SettingsModel model = null) : base(model)
        {
            Logins = new ObservableViewModelCollection<UserCredentialsViewModel, UserCredentials>(Model.Logins, u => new UserCredentialsViewModel(u));
            Profiles = new ObservableViewModelCollection<UserProfileViewModel, UserProfile>(Model.Profiles, p => new UserProfileViewModel(p));
        }
        public ObservableViewModelCollection<UserCredentialsViewModel, UserCredentials> Logins { get; } 
        public ObservableViewModelCollection<UserProfileViewModel, UserProfile> Profiles { get; }
        public bool IsOfflineMode
        {
            get => Model.IsOfflineMode;
            set
            {
                Model.IsOfflineMode = value;
                OnPropertyChanged(nameof(IsOfflineMode));
            }
        }

        private UserProfileViewModel _preferredProfileViewModel;
        public UserProfileViewModel PreferredProfile
        {
            get
            {
                if (_preferredProfileViewModel is null)
                {
                    _preferredProfileViewModel = new UserProfileViewModel(Model.PreferredProfile ?? Model.Profiles.FirstOrDefault());
                }
                    return _preferredProfileViewModel;
            }
            set
            {
                _preferredProfileViewModel = value;
                Model.PreferredProfile = value.Model;
                OnPropertyChanged(nameof(PreferredProfile));
            }
        }
    }
}