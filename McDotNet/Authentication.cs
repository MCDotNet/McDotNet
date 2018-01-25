using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet
{
    class Authentication
    {
        public static async Task<JArray> Login(string user, string password)
        {
            string request = "{'agent': {'name': 'Minecraft','version': 1},'username': " + user + ",'password': '" + password + "'}";
            using (var AUTHSERVICE = new HttpClient())
            {
                var response = await AUTHSERVICE.PostAsync(
                    "https://authserver.mojang.com/authenticate",
                     new StringContent(request, Encoding.UTF8, "application/json"));
                return JArray.Parse(request);
            }
        }
    }
}
