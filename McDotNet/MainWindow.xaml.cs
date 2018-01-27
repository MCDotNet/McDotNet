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
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace McDotNet
{
    /// <summary>
    /// The magic happens here!
    /// also im the greatest u all r noobs
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.mcdotnet\\versions\\");
            }
            InitializeComponent();
            onStart();
            this.Loaded += ShowLoginPage;
            
        }

        private void ShowLoginPage(object sender, RoutedEventArgs e)
        {
            var loginWindow = new Login() { Owner = this };
            loginWindow.ShowDialog();
        }

        private string Version { get; set; } = "1.12.2";
        private Data.MinecraftVersion VersionData { get; set; }
        private bool isWorking;
        private string Arguments { get; set; } = "-Xmx1G -XX:+UseConcMarkSweepGC -XX:+CMSIncrementalMode -XX:-UseAdaptiveSizePolicy -Xmn128M";
        private async void onStart()
        {
            Welcometxt.Content = "Hi!";
        }
        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            onStart();
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
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = appData
                    + "\\.mcdotnet\\versions\\" + Version + "\\libs\\";
                var pathbutimsad = appData
                    + "\\.mcdotnet\\versions\\" + Version + "\\natives";
                CreateDirectoryIfNotPresent(path);
                CreateDirectoryIfNotPresent(pathbutimsad);
                float downloadAdd = 57.25F;
                float incrementValue = downloadAdd / VersionData.Libraries.Count; // progress bar goes smth
                Arguments += " -Djava.library.path=" + pathbutimsad;
                Arguments += " -Dminecraft.client.jar=" + appData + "\\.mcdotnet\\versions\\" + Version + "\\" + Version + ".jar";
                Arguments += " -cp ";
                string versionPath = appData + "\\.mcdotnet\\versions\\" + Version + "\\";
                foreach (var library in VersionData.Libraries)
                {
                    bool downloadingAlready = false;
                    foreach (var url in library.Download.GetUrlsToDownload())
                    {
                        var newBarValue = StatusBar.Value + incrementValue;
                        if (!url.IsNative)
                        {
                            int index = url.DownloadUrl.LastIndexOf("/");
                            var completePath = GetCompletePath(url);

                            Arguments += completePath + ";";
                            if (!File.Exists(completePath))
                            {
                                if (!downloadingAlready)
                                {
                                    await ChangeProgress("Downloading " + url.DownloadUrl.Substring(index + 1, (url.DownloadUrl.Length - index - 1)), StatusBar.Value + incrementValue);
                                    downloadingAlready = true;
                                }
                                await downloader.DownloadFileTaskAsync(url, completePath);
                            }
                            else
                            {
                                await ChangeProgress(progress: newBarValue);
                            }
                            
                        }
                        else
                        {
                            await ChangeProgress(progress: newBarValue);
                            /// <rant>
                            /// This is retarded, Why do you put DLLs in JAR files?!
                            /// RETARDE
                            /// </rant>
                            /// 
                            if (Directory.Exists(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\META-INF"))
                            {
                                Directory.Delete(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\META-INF", true);
                            }
                            int index = url.DownloadUrl.LastIndexOf("/");
                            CreateDirectoryIfNotPresent(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp");
                            await downloader.DownloadFileTaskAsync(url, appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp\\" + url.DownloadUrl.Substring(index + 1, (url.DownloadUrl.Length - index - 1)));
                            try
                            {
                                ZipFile.ExtractToDirectory(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp\\" + url.DownloadUrl.Substring(index + 1, (url.DownloadUrl.Length - index - 1)), appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\");
                            }
                            catch (IOException)
                            {
                                // dont care
                            }
                            if (Directory.Exists(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\META-INF"))
                            {
                                Directory.Delete(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\META-INF", true);
                            }

                        }
                    }
                }
                Arguments += versionPath + Version + ".jar";
                string loggerPath = appData + "\\.mcdotnet\\versions\\" + Version + "\\logger\\";
                CreateDirectoryIfNotPresent(loggerPath);
                await downloader.DownloadFileTaskAsync("https://launchermeta.mojang.com/mc/log_configs/client-1.12.xml/ef4f57b922df243d0cef096efe808c72db042149/client-1.12.xml",
                    loggerPath + Version + ".xml");
                Arguments += " -Dlog4j.configurationFile=" + loggerPath + Version + ".xml";
                if (Directory.Exists(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp"))
                {
                    Directory.Delete(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp", true);
                }
                await ChangeProgress("Downloading Minecraft...", StatusBar.Value + 0.25);
                string minecraftUrl = "http://s3.amazonaws.com/Minecraft.Download/versions/" + Version + "/" + Version + ".jar";
                if (!File.Exists(GetCompletePath(minecraftUrl,versionPath)))
                {
                    await downloader.DownloadFileTaskAsync(minecraftUrl,
                    versionPath + Version + ".jar");
                }
                await ChangeProgress("Logging In...", StatusBar.Value + 0.25);
                Arguments += " " + VersionData.MainClass + " --version " + Version;
                if (!LoginData.IsOfflineMode)
                {   //wait ur online lol haha xddd
                    var auth = await Authentication.Login(LoginData.Username, LoginData.Password); //Login
                    Arguments += " --accessToken " + auth.AccessToken; //not stealing your pass ;-)
                    await ChangeProgress("Logging In...", StatusBar.Value + 0.25);
                    Arguments += " --username " + auth.SelectedProfile.Name;
                    Arguments += " --uuid " + auth.SelectedProfile.Id;
                    await ChangeProgress("Logging In...", StatusBar.Value + 5);
                }
                else
                {
                    Arguments += " --username " + LoginData.Username;
                    await ChangeProgress(progress: StatusBar.Value + 5);
                }
                string assetsPath = appData + "\\.mcdotnet\\assets";
                CreateDirectoryIfNotPresent(assetsPath);

                await ChangeProgress("Setting Up...", StatusBar.Value + 5);
                Arguments += " --assetsDir " + assetsPath;

                await ChangeProgress(progress: StatusBar.Value + 5);
                Arguments += " --assetsIndex " + VersionData.AssetIndex.Id;
                Arguments += " --gameDir " + appData + "\\.mcdotnet";
                Arguments += " --userType mojang --versionType release";

                await ChangeProgress("Setting Up...", StatusBar.Value + 5);
                Process Minecraft = new Process();
                Minecraft.StartInfo.FileName = "javaw.exe"; //not the full application path
                Minecraft.StartInfo.Arguments = Arguments;
                await ChangeProgress("Starting!", StatusBar.Value + 7);
                Console.WriteLine(Arguments);
                Minecraft.Start();
                await ChangeProgress("Success, Enjoy your game", 100);

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

        private string GetCompletePath(string url, string customPath = null)
        {           
            var path = customPath ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + "\\.mcdotnet\\versions\\" + Version + "\\libs\\";
            int index = url.LastIndexOf("/");
            return path + url.Substring(index + 1, (url.Length - index - 1));
        }

        private string CreateDirectoryIfNotPresent(string d)
        {
            if (!Directory.Exists(d))
            {
                Directory.CreateDirectory(d);
            }
            return d;
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
