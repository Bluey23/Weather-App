using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
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

        public MainViewModel()
        {
            if (IsInDesignMode)
            {
            }
            else
            {

                geo = new Geolocator();
                getGeoLocation();

                refreshTimer.Interval = new TimeSpan(0, 1, 0);
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