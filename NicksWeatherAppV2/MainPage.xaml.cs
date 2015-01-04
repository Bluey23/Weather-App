using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NicksWeatherAppV2.Resources;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.IO.IsolatedStorage;
using System.Windows.Media;

namespace NicksWeatherAppV2
{
    public partial class MainPage : PhoneApplicationPage
    {

        ShellTileSchedule SampleTileSchedule = new ShellTileSchedule();
        

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}

        // When the page is loaded, make sure that you have obtained the users consent to use their location
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
                // User has already opted in or out of Location
                return;
            }
            else
            {
                MessageBoxResult result =
                    MessageBox.Show("This app accesses your phone's location. Is that ok?",
                    "Location",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        private void SlideView_OnSelectionChanged(object sender, EventArgs e)
        {
        }

        private void WeatherForecast_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Slide.SelectedIndex = 1;

            ShellTile newTile = ShellTile.ActiveTiles.First();

            IconicTileData UpdateTileData = new IconicTileData
            {
                Title = "[title]",
                Count = 9,
                WideContent1 = "[1st row of content]",
                WideContent2 = "[2nd row of content]",
                WideContent3 = "[3rd row of content]",
                SmallIconImage = new Uri("/Icons/01.pg", UriKind.Relative),
                IconImage = new Uri("/Icons/01.pg", UriKind.Relative),
                BackgroundColor = new Color { A = 34, B = 34, G = 23, R = 34 }
            };

            newTile.Update(UpdateTileData);
        }

    }
}