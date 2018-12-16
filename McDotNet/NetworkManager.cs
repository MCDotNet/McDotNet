using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace McDotNet
{
    internal static class NetworkManager
    {
        public static HttpClient NetworkClient { get; } = new HttpClient();

        public static async Task DownloadToFileAsync(string url, string filePath)
        {
            var stream = await NetworkClient.GetStreamAsync(url);
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await stream.CopyToAsync(fileStream);
            }
        }
    }
}
