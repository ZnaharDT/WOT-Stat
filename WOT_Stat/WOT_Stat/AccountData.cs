using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WOT_Stat
{
    class AccountData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private String nick = "";
        private Int32 battleCount = 0;
        private Int32 rating = 0;
        private Int32 wins = 0;
        public ObservableCollection<Vehicle> Hangar { get; set; } = new ObservableCollection<Vehicle>();

        public string Nick
        {
            get
            {
                return nick;
            }

            set
            {
                nick = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Nick"));
            }
        }

        public int BattleCount
        {
            get
            {
                return battleCount;
            }

            set
            {
                battleCount = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("BattleCount"));
            }
        }

        public int Rating
        {
            get
            {
                return rating;
            }

            set
            {
                rating = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Rating"));
            }
        }

        public String WinsPerc
        {
            get
            {
                if (battleCount == 0)
                    return "0";
                return Math.Round(((wins * 100.0 )/ battleCount), 2).ToString() + "%";
            }
        }

        public int Wins
        {
            get
            {
                return wins;
            }

            set
            {
                wins = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WinsPerc"));
            }
        }

        public void SetStats(JToken playerStats)
        {
            BattleCount = (Int32)playerStats.SelectToken("statistics").SelectToken("all").SelectToken("battles");
            Wins = (Int32)playerStats.SelectToken("statistics").SelectToken("all").SelectToken("wins");
            Rating = (Int32)playerStats.SelectToken("global_rating");
            Nick = (String)playerStats.SelectToken("nickname");
        }

        private async void CheckVehicle(JToken vehicle, Boolean exists)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync("https://api.worldoftanks.ru/wot/encyclopedia/tankinfo/?application_id=9b148aed6a18f53e7861c65bd070a97e&fields=name&tank_id=" + vehicle.SelectToken("tank_id"));
            string toCheck = $"{JObject.Parse(result).SelectToken("data").SelectToken($"{vehicle.SelectToken("tank_id")}")}";
            if (toCheck == "null")
                exists = false;
            else exists = true;
        }

        public void SetHangar(JToken vehicles)
        {
            Boolean exists = true;
            for(int i = 0; i < vehicles.Count(); i++)
            {
                CheckVehicle(vehicles[i], exists);
                if (exists)
                    Hangar.Add(new Vehicle(vehicles[i]));
            }
        }
    }
}
