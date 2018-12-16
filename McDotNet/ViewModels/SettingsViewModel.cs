
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
            Profiles = new ObservableViewModelCollection<UserProfileViewModel, UserProfile>(Model.Profiles, p => new UserProfileViewModel(p),
                p =>
                {
                    if (Logins.All(c => c != p.Credentials)) Logins.Add(p.Credentials);
                });
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
        public UserProfileViewModel TemporaryUserProfile { get; set; }
        private UserProfileViewModel _preferredProfileViewModel;
        public UserProfileViewModel PreferredProfile
        {
            get
            {
                if (_preferredProfileViewModel is null)
                {
                    _preferredProfileViewModel = new UserProfileViewModel(Model.PreferredProfile ?? Model.Profiles.FirstOrDefault());
                }
                    return TemporaryUserProfile ?? _preferredProfileViewModel;
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