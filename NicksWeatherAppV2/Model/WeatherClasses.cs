using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NicksWeatherAppV2.Model
{

    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class City
    {
        //public int id { get; set; }
        public string name { get; set; }
        //public Coord coord { get; set; }
        //public string country { get; set; }
        //public int population { get; set; }
    }

    public class Temp
    {
        public double day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Day
    {
        private int _dt;

        public int dt
        {
            get { return _dt; }
            set
            {
                _dt = value;
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                Time = epoch.AddSeconds(value);
            }
        }

        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        public Temp temp { get; set; }
        public double pressure { get; set; }
        public int humidity { get; set; }
        public List<Weather> weather { get; set; }
        public double speed { get; set; }
        public int deg { get; set; }
        public int clouds { get; set; }
        public double? rain { get; set; }
        public double? snow { get; set; }

        public string IconPath
        {
            get
            {
                return "/Icons/" + weather[0].icon.Replace("d", "").Replace("n", "") + ".png";
            }
        }

        public Color TempColor
        {
            get
            {
                return Color.FromArgb(255, (byte)(255 * (temp.day / 30)), 0, (byte)(255 - (255 * (temp.day / 30))));
            }
        }
    }

    public class RootObject
    {
        public string cod { get; set; }
        public double message { get; set; }
        public City city { get; set; }
        public int cnt { get; set; }
        public List<Day> list { get; set; }
    }


}

