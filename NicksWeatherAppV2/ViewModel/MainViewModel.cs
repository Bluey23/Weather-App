using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using NicksWeatherAppV2.Model;
using Windows.Devices.Geolocation;

namespace NicksWeatherAppV2.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="DayList" /> property's name.
        /// </summary>
        public const string DayListPropertyName = "DayList";

        private ObservableCollection<Day> _dayList = null;
        Geolocator geo = null;
        DispatcherTimer refreshTimer = new DispatcherTimer();

        /// <summary>
        /// Sets and gets the DayList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Day> DayList
        {
            get
            {
                return _dayList;
            }

            set
            {
                if (_dayList == value)
                {
                    return;
                }

                //RaisePropertyChanging(DayListPropertyName);
                _dayList = value;
                RaisePropertyChanged(DayListPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CurrentDay" /> property's name.
        /// </summary>
        public const string CurrentDayPropertyName = "CurrentDay";

        private Day _currentDay = null;

        /// <summary>
        /// Sets and gets the CurrentDay property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Day CurrentDay
        {
            get
            {
                return _currentDay;
            }

            set
            {
                if (_currentDay == value)
                {
                    return;
                }

                //RaisePropertyChanging(CurrentDayPropertyName);
                _currentDay = value;
                RaisePropertyChanged(CurrentDayPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="City" /> property's name.
        /// </summary>
        public const string CityPropertyName = "City";

        private string _City = "";

        /// <summary>
        /// Gets the City property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                if (_City == value)
                {
                    return;
                }

                var oldValue = _City;
                _City = value;

                // Update bindings and broadcast change using GalaSoft.MvvmLight.Messenging
                RaisePropertyChanged(CityPropertyName, oldValue, value, true);
            }
        }

        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                DayList = new ObservableCollection<Day>();
                DayList.Add(new Day { temp = new Temp { day = 20, eve = 20, max = 20, min = 20, morn = 20, night = 20 }, Time = DateTime.Now, humidity = 1000, rain = 10, weather = new System.Collections.Generic.List<Weather> { new Weather { icon = "01d", description = "Rain", main = "Rain" } }, speed = 34, pressure = 100, deg = 23, clouds = 23 });
                DayList.Add(new Day { temp = new Temp { day = 20, eve = 20, max = 20, min = 20, morn = 20, night = 20 }, Time = DateTime.Now.AddDays(1), humidity = 1000, rain = 10, weather = new System.Collections.Generic.List<Weather> { new Weather { icon = "01d", description = "Rain", main = "Rain" } } });
                DayList.Add(new Day { temp = new Temp { day = 20, eve = 20, max = 20, min = 20, morn = 20, night = 20 }, Time = DateTime.Now.AddDays(2), humidity = 1000, rain = 10, weather = new System.Collections.Generic.List<Weather> { new Weather { icon = "01d", description = "Rain", main = "Rain" } } });
                DayList.Add(new Day { temp = new Temp { day = 20, eve = 20, max = 20, min = 20, morn = 20, night = 20 }, Time = DateTime.Now.AddDays(3), humidity = 1000, rain = 10, weather = new System.Collections.Generic.List<Weather> { new Weather { icon = "01d", description = "Rain", main = "Rain" } } });
                DayList.Add(new Day { temp = new Temp { day = 20, eve = 20, max = 20, min = 20, morn = 20, night = 20 }, Time = DateTime.Now.AddDays(4), humidity = 1000, rain = 10, weather = new System.Collections.Generic.List<Weather> { new Weather { icon = "01d", description = "Rain", main = "Rain" } } });
                CurrentDay = DayList[0];
            }
            else
            {

                geo = new Geolocator();
                getGeoLocation();

                refreshTimer.Interval = new TimeSpan(0, 10, 0);
                refreshTimer.Tick += refreshTimer_Tick;

                refreshTimer.Start();
            }
        }

        void refreshTimer_Tick(object sender, EventArgs e)
        {
            getGeoLocation();
        }

        private void weatherClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                RootObject result = JsonConvert.DeserializeObject<RootObject>(e.Result);
                City = result.city.name; 
                DayList = new ObservableCollection<Day>(result.list);

                CurrentDay = DayList[0];
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        async private void getGeoLocation()
        {

            try
            {
                //gets geoposition of phone
                Geoposition pos = await geo.GetGeopositionAsync();
                String lat = pos.Coordinate.Latitude.ToString();
                String lon = pos.Coordinate.Longitude.ToString();


                //uses the lon and lat with the weather api to get the weather in the area of the phone 
                WebClient weatherClient = new WebClient();
                weatherClient.DownloadStringCompleted += weatherClient_DownloadStringCompleted;

                String url = "http://api.openweathermap.org/data/2.5/forecast/daily?lat=" + lat + "&lon=" + lon + "&cnt=10&mode=json&units=metric";
                weatherClient.DownloadStringAsync(new Uri(url));              

            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Couldn't get the geolocation");
            }

        }
    }
}