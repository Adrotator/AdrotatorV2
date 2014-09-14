using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AdRotator.Examples.Windows8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool AdRotatorHidden = true;
        AdRotator.AdRotatorControl myAdControl;
        //GoogleAnalyticsTracker.RT.Tracker tracker = null;

        public MainPage()
        {
            this.InitializeComponent();
            MyAdRotatorControl.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.PubCenter, typeof(Microsoft.Advertising.WinRT.UI.AdControl));
            MyAdRotatorControl.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.AdDuplex, typeof(AdDuplex.Controls.AdControl));
            //AdRotatorControl.Log += (s) => System.Diagnostics.Debug.WriteLine(s);
            Loaded += (s, e) => HideButton_Tapped(null,null);
            //InitialiseAdRotatorProgramatically();
            //tracker = new GoogleAnalyticsTracker.RT.Tracker("UA-51978219-2", "AdRotator");
            //tracker.TrackPageViewAsync("My API - Create", "api/create");


        }

        void msadcontrol_ErrorOccurred(object sender, Microsoft.Advertising.WinRT.UI.AdErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void msadcontrol_AdRefreshed(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Started");
        }

        private void HideButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdRotatorHidden = !AdRotatorHidden;
            MyAdRotatorControl.Visibility = AdRotatorHidden ? Visibility.Collapsed : Visibility.Visible;
            if(myAdControl != null) myAdControl.Visibility = AdRotatorHidden ? Visibility.Collapsed : Visibility.Visible;
            HideButton.Content = AdRotatorHidden ? "UnHide AdRotator" : "Hide AdRotator";

        }

        void InitialiseAdRotatorProgramatically()
        {
            myAdControl = new AdRotatorControl();
            myAdControl.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.PubCenter, typeof(Microsoft.Advertising.WinRT.UI.AdControl));
            myAdControl.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.AdDuplex, typeof(AdDuplex.Controls.AdControl));
            myAdControl.LocalSettingsLocation = "ProgramaticdefaultAdSettings.xml";
            //myAdControl.RemoteSettingsLocation = "http://adrotator.apphb.com/V2defaultAdSettings.xml";
            myAdControl.AdWidth = 150;
            myAdControl.AdHeight = 150;
            myAdControl.AutoStartAds = true;
            myAdControl.BorderBrush = new SolidColorBrush(Windows.UI.Colors.AntiqueWhite);
            myAdControl.BorderThickness = new Thickness(20);
            ProgramaticAdRotator.Children.Add(myAdControl);
            //AdRotatorControl.Log += (s) => { tracker.TrackEventAsync("AdRotator", "AdLogEvent", "an Ad", 0); tracker.TrackPageViewAsync("My API - Create", "api/view"); };
            //<!--GoogleAnalyticsId="UA-51978219-1"-->
        }
    }
}
