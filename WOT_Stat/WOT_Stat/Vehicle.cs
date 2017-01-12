using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WOT_Stat
{
    public class Vehicle
    {
        private Int32 tankId;
        private String localized_name;
        private Int32 battles;
        private String mastery;
        private Int32 totalXP;
        private Int32 maxXP;
        private Int32 wins;
        private String winsPerc;
        private BitmapImage img;


        public int TankId
        {
            get
            {
                return tankId;
            }

            set
            {
                tankId = value;
            }
        }

        public int Battles
        {
            get
            {
                return battles;
            }

            set
            {
                battles = value;
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
            }
        }

        public BitmapImage Img
        {
            get
            {
                return img;
            }

            set
            {
                img = value;
            }
        }

        public string Localized_name
        {
            get
            {
                return localized_name;
            }

            set
            {
                localized_name = value;
            }
        }

        public String Mastery
        {
            get
            {
                return mastery;
            }

            set
            {
                mastery = value;
            }
        }

        public int TotalXP
        {
            get
            {
                return totalXP;
            }

            set
            {
                totalXP = value;
            }
        }

        public int MaxXP
        {
            get
            {
                return maxXP;
            }

            set
            {
                maxXP = value;
            }
        }

        public String WinsPerc
        {
            get
            {
                return winsPerc;
            }

            set
            {
                winsPerc = value;
            }
        }

        public Vehicle()
        {
            Battles = 0;
            Wins = 0;
            TankId = 0;
        }

        private async void GetLocalizedName()
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync("https://api.worldoftanks.ru/wot/encyclopedia/tankinfo/?application_id=9b148aed6a18f53e7861c65bd070a97e&fields=localized_name&tank_id=" + TankId);
            Localized_name = $"{JObject.Parse(result).SelectToken("data").SelectToken($"{TankId}").SelectToken("localized_name")}";
        }

        private async void GetTankImage()
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync("https://api.worldoftanks.ru/wot/encyclopedia/tankinfo/?application_id=9b148aed6a18f53e7861c65bd070a97e&fields=image&tank_id=" + TankId);
            if (TankId != 63505)
            Img = new BitmapImage(new Uri($"{JObject.Parse(result).SelectToken("data").SelectToken($"{TankId}").SelectToken("image")}"));
        }

        public Vehicle(JToken vehicleInfo)
        {
            Battles = (Int32)vehicleInfo.SelectToken("all").SelectToken("battles");
            Wins = (Int32)vehicleInfo.SelectToken("all").SelectToken("wins");
            if (Battles == 0)
                WinsPerc = "0";
            else
                WinsPerc = Math.Round(((Wins * 100.0) / Battles), 2).ToString() + "%";
            TotalXP = (Int32)vehicleInfo.SelectToken("all").SelectToken("xp");
            TankId = (Int32)vehicleInfo.SelectToken("tank_id");
            MaxXP = (Int32)vehicleInfo.SelectToken("max_xp");
            switch ((Int32)vehicleInfo.SelectToken("mark_of_mastery"))
            {
                case 0:
                    Mastery = "Отсутствует";
                    break;
                case 1:
                    Mastery = "3 степень";
                    break;
                case 2:
                    Mastery = "2 степень";
                    break;
                case 3:
                    Mastery = "1 степень";
                    break;
                case 4:
                    Mastery = "Мастер";
                    break;
            }
            GetTankImage();
            GetLocalizedName();
        }

        public void SetVehicle(JToken vehicleInfo)
        {
            Battles = (Int32)vehicleInfo.SelectToken("all").SelectToken("battles");
            Wins = (Int32)vehicleInfo.SelectToken("all").SelectToken("wins");
            TotalXP = (Int32)vehicleInfo.SelectToken("all").SelectToken("xp");
            TankId = (Int32)vehicleInfo.SelectToken("tank_id");
            MaxXP = (Int32)vehicleInfo.SelectToken("max_xp");
            switch ((Int32)vehicleInfo.SelectToken("mark_of_mastery"))
            {
                case 0:
                    Mastery = "Отсутствует";
                    break;
                case 1:
                    Mastery = "3 степень";
                    break;
                case 2:
                    Mastery = "2 степень";
                    break;
                case 3:
                    Mastery = "1 степень";
                    break;
                case 4:
                    Mastery = "Мастер";
                    break;
            };

            GetTankImage();
            GetLocalizedName();
        }
    }
}
