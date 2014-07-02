using System.Windows;
using Microsoft.Phone.Controls;

namespace AdRotator.Examples.WinPhone7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        bool _adRotatorHidden = true;
        AdRotator.AdRotatorControl _myAdControl;
        //GoogleAnalyticsTracker.Tracker tracker = null;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            AdRotatorControl.Log += (s) => AdRotatorControl_Log(s);
            Loaded += MainPage_Loaded;
            InitialiseAdRotatorProgramatically();
            //tracker.TrackPageView("My API - Create", "api/create");

        }

        void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AdRotatorControl_Log("Page Loaded");
            HideButton_Tap(null, null);
            if (_myAdControl != null) _myAdControl.Invalidate(null);
        }

        void AdRotatorControl_Log(string message)
        {
            Dispatcher.BeginInvoke(() => MessagesListBox.Items.Insert(0, message));
        }

        private void HideButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _adRotatorHidden = !_adRotatorHidden;
            AdRotatorControl.Visibility = _adRotatorHidden ? Visibility.Collapsed : Visibility.Visible;
            HideButton.Content = _adRotatorHidden ? "UnHide AdRotator" : "Hide AdRotator";

        }

        void InitialiseAdRotatorProgramatically()
        {
            _myAdControl = new AdRotatorControl(1);
            //_myAdControl.LocalSettingsLocation = "defaultAdSettings.xml";
            _myAdControl.RemoteSettingsLocation = "http://adrotator.apphb.com/V2defaultAdSettings.xml";
            _myAdControl.AdWidth = 728;
            _myAdControl.AdHeight = 90;
            //_myAdControl.AutoStartAds = true;
            ProgramaticAdRotator.Children.Add(_myAdControl);
           // AdRotatorControl.Log += (s) => { tracker.TrackEvent("AdRotator", "AdLogEvent", "an Ad", 0); tracker.TrackPageView("My API - Create", "api/view"); };
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
    }
}