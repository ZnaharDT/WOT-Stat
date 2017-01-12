using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WOT_Stat
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            stats = new AccountData();
            DataContext = this.stats;
            this.InitializeComponent();
            
        }

        private String nick { get; set; } = "";
        private Int32 playerID { get; set; }
        private HttpClient client = new HttpClient();

        private AccountData stats;

        private async void GetStats()
        {
            var result = await client.GetStringAsync("https://api.worldoftanks.ru/wot/account/list/?application_id=9b148aed6a18f53e7861c65bd070a97e&language=ru&type=exact&search=" + nick);
            dynamic playerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            this.playerID = Int32.Parse($"{playerInfo.data[0].account_id}");

            result = await client.GetStringAsync("http://api.worldoftanks.ru/wot/account/info/?application_id=9b148aed6a18f53e7861c65bd070a97e&account_id=" + playerID);            
            stats.SetStats(JObject.Parse(result).SelectToken("data").SelectToken($"{playerID}"));

            result = await client.GetStringAsync("http://api.worldoftanks.ru/wot/tanks/stats/?application_id=9b148aed6a18f53e7861c65bd070a97e&fields=mark_of_mastery%2Cmark_of_mastery%2Cmax_xp%2Ctank_id%2Cfrags%2Call.battles%2Call.wins%2Call.xp&account_id=" + playerID);
            stats.SetHangar(JObject.Parse(result).SelectToken("data").SelectToken($"{playerID}"));
        }



        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            nick = NicknameBox.Text;
            GetStats();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(TankDetails), e.ClickedItem);
        }
    }
}
