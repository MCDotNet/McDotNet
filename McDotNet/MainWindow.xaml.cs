using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Net;

namespace McDotNet
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\")) System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\");
            InitializeComponent();
        }
        private string Version { get; set; } = "1.12.2";
        private Data.MinecraftVersion VersionData { get; set; }
        private bool isWorking;

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {

            WebClient DOWNLOADER = new WebClient();
            PlayButton.IsEnabled = false;
            isWorking = true;
            StatusContainer.Visibility = Visibility.Visible;
            await ChangeProgress("Getting libraries...", 5);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://s3.amazonaws.com/Minecraft.Download/versions/" + Version + "/" + Version + ".json");
            if (response.IsSuccessStatusCode)
            {
                await ChangeProgress("Deserializing libraries data...", 10);
                VersionData = JsonConvert.DeserializeObject<Data.MinecraftVersion>(await response.Content.ReadAsStringAsync());
                if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\libs\\")) System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\libs\\");
                foreach (var library in VersionData.Libraries)
                {
                    Uri uri = new Uri(library.Download.Artifact.Url);
                    await ChangeProgress("Downloading "+ System.IO.Path.GetFileName(uri.LocalPath), StatusBar.Value+0.80);
                    await DOWNLOADER.DownloadFileTaskAsync(library.Download.Artifact.Url, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\libs\\" + System.IO.Path.GetFileName(uri.LocalPath));
                }
                await ChangeProgress("Downloading Minecraft...", StatusBar.Value+5);
                await DOWNLOADER.DownloadFileTaskAsync("http://s3.amazonaws.com/Minecraft.Download/versions/" + Version + "/" + Version + ".jar", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\" + Version + ".jar");
                await ChangeProgress("Applying Stuff...", StatusBar.Value + 5);
                await ChangeProgress("Success !", 100);
                
            }
            PlayButton.IsEnabled = true;
            isWorking = false;
            await Task.Run(async () =>
            {
                await Task.Delay(2750);
                if (!isWorking)
                {
                    Dispatcher.Invoke(() =>
                    {
                        StatusContainer.Visibility = Visibility.Collapsed;
                    });
                }
            });
        }
        private async Task ChangeProgress(string p = null, double? progress = null)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                if (p != null)
                {
                    Status.Content = p;
                }
                if (progress != null)
                {
                    StatusBar.Value = (double)progress;
                }
            });
        }
    }
}
