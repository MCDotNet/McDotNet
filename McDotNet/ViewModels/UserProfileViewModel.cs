namespace McDotNet.ViewModels
{
    public class UserProfileViewModel : ViewModelBase<UserProfile>
    {
        public UserProfileViewModel() : this(null)
        {
        }

        public UserProfileViewModel(UserProfile model = null) : base(model)
        {
        }

        public string MinecraftVersion
        {
            get => Model.MinecraftVersion;
            set
            {
                Model.MinecraftVersion = value;
                OnPropertyChanged(nameof(MinecraftVersion));
            }
        }
        private UserCredentialsViewModel _credentials;

        public UserCredentialsViewModel Credentials
        {
            get => _credentials ?? (_credentials = new UserCredentialsViewModel(Model.Credentials));
            set => _credentials = value;
        }

    }
}