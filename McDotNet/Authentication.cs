using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using McDotNet.Data;
using Newtonsoft.Json;

namespace McDotNet
{
    class Authentication
    {
        public static async Task<AuthenticationResponse> Login(string user, string password)
        {
            var request = JsonConvert.SerializeObject(new AuthenticationHeader(user, password).ConvertForJson());
            using (var authService = new HttpClient())
            {
                var response = await authService.PostAsync(
                    "https://authserver.mojang.com/authenticate",
                     new StringContent(request, Encoding.UTF8, "application/json"));
                var str = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Password isn't maybe right, error code : {response.StatusCode}");
                }
                else
                {
                    return JsonConvert.DeserializeObject<AuthenticationResponse>(str);
                }
            }
        }
    }
}
