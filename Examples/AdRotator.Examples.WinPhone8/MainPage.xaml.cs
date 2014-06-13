using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AdRotator.Examples.WinPhone8.Resources;

namespace AdRotator.Examples.WinPhone8
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool AdRotatorHidden = true;
        AdRotator.AdRotatorControl myAdControl;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
            this.AdRotatorControl.Log += (s) => AdRotatorControl_Log(s);
            Loaded += MainPage_Loaded;
            InitialiseAdRotatorProgramatically();
        }

        void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AdRotatorControl_Log("Page Loaded");
            HideButton_Tap(null, null);
        }

        void AdRotatorControl_Log(string message)
        {
            Dispatcher.BeginInvoke(() => MessagesListBox.Items.Insert(0, message));
        }

        private void HideButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            AdRotatorHidden = !AdRotatorHidden;
            AdRotatorControl.Visibility = AdRotatorHidden ? Visibility.Collapsed : Visibility.Visible;
            HideButton.Content = AdRotatorHidden ? "UnHide AdRotator" : "Hide AdRotator";

        }

        void InitialiseAdRotatorProgramatically()
        {
            //myAdControl = new AdRotatorControl(1);
            ////myAdControl.LocalSettingsLocation = "defaultAdSettings.xml";
            //myAdControl.RemoteSettingsLocation = "http://adrotator.apphb.com/V2defaultAdSettings.xml";
            //myAdControl.AdWidth = 728;
            //myAdControl.AdHeight = 90;
            //myAdControl.AutoStartAds = true;
            //ProgramaticAdRotator.Children.Add(myAdControl);

            var mymsad = new Microsoft.Advertising.Mobile.UI.AdControl();
            mymsad.AdUnitId = "";
            mymsad.ApplicationId = "";
            mymsad.ErrorOccurred += mymsad_ErrorOccurred;
            mymsad.AdRefreshed += mymsad_AdRefreshed;
        }

        void mymsad_AdRefreshed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void mymsad_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void googlead_ReceivedAd(object sender, GoogleAds.AdEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void googlead_FailedToReceiveAd(object sender, GoogleAds.AdErrorEventArgs e)
        {
            throw new NotImplementedException();
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