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
using McDotNet;
using System.IO;
using Newtonsoft.Json.Linq;

namespace McDotNet
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\")) Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\");
            InitializeComponent();
            var Thingie = new Login();
            Thingie.Show();
        }
        private string Version { get; set; } = "1.12.2";
        private Data.MinecraftVersion VersionData { get; set; }
        private bool isWorking;
        //java is  a meanie so it want to donwload mor e ram
        private string Arguments { get; set; } = "";
        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {

            WebClient downloader = new WebClient();
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
                var path = Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData)
                    + "\\.mcdotnet\\versions\\" + Version + "\\libs\\";
                var pathbutimsad = Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData)
                    + "\\.mcdotnet\\versions\\" + Version + "\\libs";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                int downloadAdd = 80;
                float incrementValue =  downloadAdd / VersionData.Libraries.Count; // progress bar goes 90
                Arguments += " -Djava.library.path=" + pathbutimsad;
                Arguments += " -Dminecraft.client.jar=" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\" + Version + ".jar";
                Arguments += " -cp ";
                foreach (var library in VersionData.Libraries)
                {
                    
                    Uri uri = new Uri(library.Download.Artifact.Url);
                    var completePath = path + System.IO.Path.GetFileName(uri.LocalPath);
                    var newBarValue = StatusBar.Value + incrementValue;
                    Arguments += completePath + ";";
                    if (!File.Exists(completePath))
                    {
                        await ChangeProgress("Downloading " + System.IO.Path.GetFileName(uri.LocalPath), StatusBar.Value + incrementValue);
                        await downloader.DownloadFileTaskAsync(library.Download.Artifact.Url, completePath);
                    } 
                    else
                    {
                        await ChangeProgress(progress: newBarValue);
                    }
                    Arguments += Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\" + Version + ".jar";
                }
                Arguments += " -Xmx1G -XX:+UseConcMarkSweepGC -XX:+CMSIncrementalMode -XX:-UseAdaptiveSizePolicy -Xmn128M";
                await downloader.DownloadFileTaskAsync("https://launchermeta.mojang.com/mc/log_configs/client-1.12.xml/ef4f57b922df243d0cef096efe808c72db042149/client-1.12.xml" + Version + "/" + Version + ".jar",
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\logger\\" + Version + ".xml");
                Arguments += " -Dlog4j.configurationFile=" + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\logger\\" + Version + ".xml";
                await ChangeProgress("Downloading Minecraft...", StatusBar.Value+5);
                await downloader.DownloadFileTaskAsync("http://s3.amazonaws.com/Minecraft.Download/versions/" + Version + "/" + Version + ".jar",
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\" + Version + "\\" + Version + ".jar");
                await ChangeProgress("Connecting...", StatusBar.Value + 5);
                Arguments += " net.minecraft.client.main.Main --version " + Version;
                if (!Properties.Settings.Default.offline_mode) { //wait ur offline lol
                    JArray auth = await Authentication.Login(Properties.Settings.Default.username, Properties.Settings.Default.password);
                    //got to sleep bye
                } else {
                    Arguments += " --username " + Properties.Settings.Default.username;
                }
                Arguments += " --gameDir " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet";
                Arguments += " --userType mojang --versionType release";

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
