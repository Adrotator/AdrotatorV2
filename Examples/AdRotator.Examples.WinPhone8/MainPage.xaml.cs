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

            AdRotatorControl.Log += (s) => AdRotatorControl_Log(s);
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
            myAdControl = new AdRotatorControl(1);
            //myAdControl.LocalSettingsLocation = "defaultAdSettings.xml";
            myAdControl.RemoteSettingsLocation = "http://adrotator.apphb.com/V2defaultAdSettings.xml";
            myAdControl.AdWidth = 728;
            myAdControl.AdHeight = 90;
            myAdControl.AutoStartAds = true;
            ProgramaticAdRotator.Children.Add(myAdControl);
        }
    }
}