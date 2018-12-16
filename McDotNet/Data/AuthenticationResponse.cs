namespace McDotNet.Data
{
    public class AuthenticationResponse
    {
        public string AccessToken { get; set; }
        public string ClientToken { get; set; }
        public Profile SelectedProfile { get; set; }
        public Profile[] AvailableProfiles { get; set; }
    }
}
