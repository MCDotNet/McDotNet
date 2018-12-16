using System.Collections.Generic;
using Newtonsoft.Json;

namespace McDotNet.Data
{
    public class AuthenticationHeader
    {
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        public static explicit operator Dictionary<string,object>(AuthenticationHeader auth)
        {
            return Convert(auth);
        }

        private static Dictionary<string,object> Convert(AuthenticationHeader auth)
        {
            return new Dictionary<string, object>
            {
                { "agent", new Dictionary<string,object>
                   {
                    { "name", "Minecraft"},
                    { "version", 1 }
                   }
                },
                { "username", auth.Username },
                { "password", auth.Password }
            };
        }
        public Dictionary<string, object> ConvertForJson() => Convert(this);
        public AuthenticationHeader(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
