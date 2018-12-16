using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace McDotNet.Views
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
            var loginWindow = new LoginWindow() { Owner = this };
            loginWindow.ShowDialog();
        }

        private string Version { get; set; } = "1.12.2";
        private Data.MinecraftVersion VersionData { get; set; }
        private bool isWorking;
        private bool demo=false;
        private string Arguments { get; set; } = "-Xmx1G -XX:+UseConcMarkSweepGC -XX:+CMSIncrementalMode -XX:-UseAdaptiveSizePolicy -Xmn128M";
        private void onStart()
        {
            Welcometxt.Content = "Hi!";
        }
        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            onStart();
            PlayButton.IsEnabled = false;
            isWorking = true;
            StatusContainer.Visibility = Visibility.Visible;
            await ChangeProgress("Fetching Minecraft Data...", 2);
            var responsefornormal = await NetworkManager.NetworkClient.GetAsync("https://launchermeta.mojang.com/mc/game/version_manifest.json");
            var result = JsonConvert.DeserializeObject<Data.VersionManifest>(await responsefornormal.Content.ReadAsStringAsync());
            var versionISearchedFor = result.Versions.First(thing => thing.Id == Version);
            await ChangeProgress("Getting libraries...", 5);
           
            var client = new HttpClient();
            Console.WriteLine(versionISearchedFor.Url);
            var response = await client.GetAsync(versionISearchedFor.Url);
            if (response.IsSuccessStatusCode)
            {
                await ChangeProgress("Deserializing libraries data...", 10);
                VersionData = JsonConvert.DeserializeObject<Data.MinecraftVersion>(await response.Content.ReadAsStringAsync());
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = appData
                    + "\\.mcdotnet\\versions\\" + Version + "\\libs\\";
                var pathbutimsad = appData
                    + "\\.mcdotnet\\versions\\" + Version + "\\natives";
                CreateDirectoryIfNotPresent(path);
                CreateDirectoryIfNotPresent(pathbutimsad);
                var downloadAdd = 57.25F;
                var incrementValue = downloadAdd / VersionData.Libraries.Count; // progress bar goes smth
                Arguments += " -Djava.library.path=" + pathbutimsad;
                Arguments += " -Dminecraft.client.jar=" + appData + "\\.mcdotnet\\versions\\" + Version + "\\" + Version + ".jar";
                Arguments += " -cp ";
                var versionPath = appData + "\\.mcdotnet\\versions\\" + Version + "\\";
                foreach (var library in VersionData.Libraries)
                {
                    var downloadingAlready = false;
                    foreach (var url in library.Download.GetUrlsToDownload())
                    {
                        var newBarValue = StatusBar.Value + incrementValue;
                        if (!url.IsNative)
                        {
                            var index = url.DownloadUrl.LastIndexOf("/");
                            var completePath = GetCompletePath(url);

                            Arguments += completePath + ";";
                            if (!File.Exists(completePath))
                            {
                                if (!downloadingAlready)
                                {
                                    await ChangeProgress("Downloading " + url.DownloadUrl.Substring(index + 1, (url.DownloadUrl.Length - index - 1)), StatusBar.Value + incrementValue);
                                    downloadingAlready = true;
                                }
                                await NetworkManager.DownloadToFileAsync(url, completePath);
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
                            var index = url.DownloadUrl.LastIndexOf("/");
                            CreateDirectoryIfNotPresent(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp");
                            await NetworkManager.DownloadToFileAsync(url, appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp\\" + url.DownloadUrl.Substring(index + 1, (url.DownloadUrl.Length - index - 1)));
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
                var loggerPath = appData + "\\.mcdotnet\\versions\\" + Version + "\\logger\\";
                CreateDirectoryIfNotPresent(loggerPath);
                await NetworkManager.DownloadToFileAsync("https://launcher.mojang.com/v1/objects/ef4f57b922df243d0cef096efe808c72db042149/client-1.12.xml", //TODO: make minecraftversion parse this
                    loggerPath + Version + ".xml");
                Arguments += " -Dlog4j.configurationFile=" + loggerPath + Version + ".xml";
                if (Directory.Exists(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp"))
                {
                    Directory.Delete(appData + "\\.mcdotnet\\versions\\" + Version + "\\natives\\tmp", true);
                }
                await ChangeProgress("Downloading Minecraft...", StatusBar.Value + 0.25);
                var minecraftUrl = VersionData.Downloads.Client.Url;
                if (!File.Exists(GetCompletePath(minecraftUrl,versionPath)))
                {
                    await NetworkManager.DownloadToFileAsync(minecraftUrl, versionPath + Version + ".jar");
                }
                await ChangeProgress("Logging In...", StatusBar.Value + 0.25);
                Arguments += " " + VersionData.MainClass + " --version " + Version;
                if (!LoginData.IsOfflineMode)
                {   //wait ur online lol haha xddd
                    var auth = await Authentication.Login(LoginData.Username, LoginData.Password); //LoginWindow
                    Arguments += " --accessToken " + auth.AccessToken; //not stealing your pass ;-)
                    await ChangeProgress("Logging In...", StatusBar.Value + 0.25);
                    Arguments += " --username " + auth.SelectedProfile.Name;
                    if(demo) { Arguments += " --uuid 00000000000000000000000000000000"; } else { Arguments += " --uuid " + auth.SelectedProfile.Id; }
                    await ChangeProgress("Logging In...", StatusBar.Value + 5);
                }
                else
                {
                    if (demo) { Arguments += " --uuid 00000000000000000000000000000000"; } else { Arguments += " --uuid " + Guid.NewGuid().ToString().Replace("-", ""); }
                    Arguments += " --accessToken \" \" ";
                    Arguments += " --username " + LoginData.Username;
                    await ChangeProgress(progress: StatusBar.Value + 5);
                }
                var assetsPath = appData + "\\.mcdotnet\\assets";
                CreateDirectoryIfNotPresent(assetsPath);

                await ChangeProgress("Setting Up...", StatusBar.Value + 5);
                Arguments += " --assetsDir " + assetsPath;

                await ChangeProgress(progress: StatusBar.Value + 5);
                Arguments += " --assetsIndex " + VersionData.AssetIndex.Id;
                Arguments += " --gameDir " + appData + "\\.mcdotnet";
                Arguments += " --userType mojang --versionType release";
                if(demo)
                {
                    Arguments += " --demo";
                }
                await ChangeProgress("Setting Up...", StatusBar.Value + 5);
                var Minecraft = new Process {StartInfo = {FileName = "javaw.exe", Arguments = Arguments}};
                //not the full application path
                await ChangeProgress("Starting!", StatusBar.Value + 7);
                Console.WriteLine(Arguments);
                Minecraft.Start();
                await ChangeProgress("Success, Enjoy your game", 100);

            }
            PlayButton.IsEnabled = true;
            isWorking = false;
                await Task.Delay(2750);
                if (!isWorking)
                {
                    Dispatcher.Invoke(() =>
                    {
                        StatusContainer.Visibility = Visibility.Collapsed;
                    });
                }
        }
        /// <summary>
        /// Gets the complete path on the disk drive of a download <paramref name="url"/>.
        /// </summary>
        /// <param name="url">The URL to use</param>
        /// <param name="customPath">If the file isn't in library : put a custom path to verify</param>
        /// <returns>The complete path DUH CAN YOU READ THE FUNCTION NAME ?</returns>
        private string GetCompletePath(string url, string customPath = null)
        {           
            var path = customPath ?? Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + "\\.mcdotnet\\versions\\" + Version + "\\libs\\";
            var index = url.LastIndexOf("/");
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
