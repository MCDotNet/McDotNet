using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Http;
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
            InitializeComponent();
        }
        private string VERSION = "1.12.2";
        private string LIBS = "missingno";
        public async Task Button_Click(object sender, RoutedEventArgs e)
        {
            xdded();

        }
        private async Task xdded()
        {
            HttpClient CLIENT = new HttpClient();
            HttpResponseMessage RESPONSE = await CLIENT.GetAsync("https://s3.amazonaws.com/Minecraft.Download/versions/" + VERSION + "/" + VERSION + ".json");
            if (RESPONSE.IsSuccessStatusCode)
            {
                LIBS = await RESPONSE.Content.ReadAsStringAsync();
            }
            var LIBS_PARSED = JObject.Parse(LIBS);
            Console.WriteLine(LIBS_PARSED["libraries"]);

        }
    }
}
