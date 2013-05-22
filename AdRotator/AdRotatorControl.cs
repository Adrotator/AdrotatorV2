using AdRotator.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace AdRotator
{
    public sealed class AdRotatorControl : Control, IAdRotatorProvider
    {
        private AdRotatorComponent adRotatorControl = new AdRotatorComponent(CultureInfo.CurrentUICulture.ToString(), new FileHelpers());
        AdRotator.AdProviderConfig.SupportedPlatforms CurrentPlatform = AdRotator.AdProviderConfig.SupportedPlatforms.Windows8;

        #region LoggingEventCode
        public delegate void LogHandler(string message);
        public event LogHandler Log;
        protected void OnLog(string message)
        {
            if (Log != null)
            {
                Log(message);
            }
        }
        #endregion


        public AdRotatorControl()
        {
            this.DefaultStyleKey = typeof(AdRotatorControl);

            Loaded += AdRotatorControl_Loaded;

            // List of AdProviders supportd on this platform
            AdRotatorComponent.PlatformSupportedAdProviders = new List<AdType>()
                { 
                    AdType.AdDuplex, 
                    AdType.PubCenter, 
                    AdType.Inmobi
                };
            adRotatorControl.Log += (s) => OnLog(s);
        }

        private Grid LayoutRoot
        {
            get
            {
                return GetTemplateChild("LayoutRoot") as Grid;
            }
        }

        void adRotatorControl_AdAvailable(AdProvider adProvider)
        {
            Invalidate(adProvider);
        }

        void AdRotatorControl_Loaded(object sender, RoutedEventArgs e)
        {
            // This call needs to happen when the control is loaded 
            // b/c dependency properties are propagated to their values at this point

            if (IsInDesignMode)
            {
                LayoutRoot.Children.Add(new TextBlock() { Text = "AdRotator in design mode, No ads will be displayed", VerticalAlignment = VerticalAlignment.Center });
            }
            else
            {
                adRotatorControl.AdAvailable += adRotatorControl_AdAvailable;
                adRotatorControl.GetConfig();
            }

            adRotatorControl.isLoaded = true;
        }

        public string Invalidate(AdProvider adProvider)
        {
            if (adProvider == null)
            {
                adRotatorControl.GetAd();
                return "No Provider set";
            }
            if (adProvider.AdProviderType == AdType.None)
            {
                return adRotatorControl.AdsFailed();
            }

            //(SJ) should we make this call the GetAd function? or keep it seperate
            //Isn't the aim of the GetAd function to return an ad to display or would this break other implementations?
            object providerElement = null;
            try
            {
                providerElement = adRotatorControl.GetProviderFrameworkElement(CurrentPlatform, adProvider);
            }
            catch (Exception)
            {
                adRotatorControl.AdFailed(adProvider.AdProviderType);
                return "Ad Failed to initialise";
            }
            if (providerElement == null)
            {
                adRotatorControl.AdFailed(adProvider.AdProviderType);
                return "No Ad Returned";
            }

            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add((FrameworkElement)providerElement);
            return adProvider.AdProviderType.ToString();
        }


        #region IAdRotatorProvider Members


        #region AdWidth

        /// <summary>
        /// Sets the Ad Controls Ad Width property - where availale
        /// /// </summary>
        public int AdWidth
        {
            get { return (int)adRotatorControl.AdWidth; }
            set { SetValue(AdWidthProperty, value); }
        }

        public static readonly DependencyProperty AdWidthProperty = DependencyProperty.Register("AdWidth", typeof(int), typeof(AdRotatorControl), new PropertyMetadata(480, AdWidthChanged));

        private static void AdWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.AdWidthChanged(e);
            }
        }

        private void AdWidthChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.AdWidth = (int)e.NewValue;
        }

        #endregion

        #region AdHeight

        /// <summary>
        /// Sets the Ad Controls Ad Height property - where availale
        /// </summary>
        public int AdHeight
        {
            get { return (int)adRotatorControl.AdHeight; }
            set { SetValue(AdHeightProperty, value); }
        }

        public static readonly DependencyProperty AdHeightProperty = DependencyProperty.Register("AdHeight", typeof(int), typeof(AdRotatorControl), new PropertyMetadata(80, AdHeightChanged));

        private static void AdHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.AdHeightChanged(e);
            }
        }

        private void AdHeightChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.AdHeight = (int)e.NewValue;
        }

        #endregion

        #region IsTest

        /// <summary>
        /// When set to true the control runs Ad Providers in "Test" mode if available
        /// </summary>
        public bool IsTest
        {
            get { return (bool)GetValue(IsTestProperty); }
            set { SetValue(IsTestProperty, value); }
        }

        public static readonly DependencyProperty IsTestProperty = DependencyProperty.Register("IsTest", typeof(bool), typeof(AdRotatorControl), new PropertyMetadata(false, IsTestChanged));

        private static void IsTestChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.AdWidthChanged(e);
            }
        }

        private void IsTestChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.isTest = (bool)e.NewValue;
        }

        #endregion

        #region IsInDesignMode
        public bool IsInDesignMode
        {
            get
            {
                return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
            }
        }
        #endregion

        #region RemoteSettingsLocation
        public string RemoteSettingsLocation
        {
            get { return (string)adRotatorControl.RemoteSettingsLocation; }
            set { SetValue(RemoteSettingsLocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoteSettingsLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoteSettingsLocationProperty =
            DependencyProperty.Register("RemoteSettingsLocation", typeof(string), typeof(AdRotatorControl), new PropertyMetadata(string.Empty, RemoteSettingsLocationPropertyChanged));

        private static void RemoteSettingsLocationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.OnRemoteSettingsLocationPropertyChanged(e);
            }
        }
        private void OnRemoteSettingsLocationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.RemoteSettingsLocation = (string)e.NewValue;
        }
        #endregion

        #region LocalSettingsLocation

        public string LocalSettingsLocation
        {
            get { return (string)adRotatorControl.LocalSettingsLocation; }
            set { SetValue(LocalSettingsLocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocalSettingsLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocalSettingsLocationProperty =
            DependencyProperty.Register("LocalSettingsLocation", typeof(string), typeof(AdRotatorControl), new PropertyMetadata(string.Empty, LocalSettingsLocationPropertyChanged));

        private static void LocalSettingsLocationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.OnLocalSettingsLocationPropertyChanged(e);
            }
        }

        private void OnLocalSettingsLocationPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.LocalSettingsLocation = (string)e.NewValue;
        }
        #endregion

        #region IsAdRotatorEnabled
        public bool IsAdRotatorEnabled
        {
            get { return (bool)adRotatorControl.IsAdRotatorEnabled; }
            set { SetValue(IsAdRotatorEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAdRotatorEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAdRotatorEnabledProperty =
            DependencyProperty.Register("IsAdRotatorEnabled", typeof(bool), typeof(AdRotatorControl), new PropertyMetadata(true, IsAdRotatorEnabledPropertyChanged));

        private static void IsAdRotatorEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.OnIsAdRotatorEnabledPropertyChanged(e);
            }
        }

        private void OnIsAdRotatorEnabledPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.IsAdRotatorEnabled = (bool)e.NewValue;
        }
        #endregion

        #region DefaultHouseAdBody
        public object DefaultHouseAdBody
        {
            get { return (object)adRotatorControl.DefaultHouseAdBody; }
            set { SetValue(DefaultHouseAdBodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultHouseAdBody.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultHouseAdBodyProperty =
            DependencyProperty.Register("DefaultHouseAdBody", typeof(object), typeof(AdRotatorControl), new PropertyMetadata(null, AdRotatorEnabledPropertyChanged));

        private static void AdRotatorEnabledPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.OnDefaultHouseAdBodyPropertyChanged(e);
            }
        }

        private void OnDefaultHouseAdBodyPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.DefaultHouseAdBody = e.NewValue;
        }

        #endregion

        #region IsLoaded
        public bool IsLoaded
        {
            get
            {
                return adRotatorControl.isLoaded;
            }
        }
        #endregion

        #region IsInitialised
        public bool IsInitialised
        {
            get
            {
                return adRotatorControl.isInitialised;
            }
        }
        #endregion

        #region PlatformAdProviderComponents
        public Dictionary<AdType,Type> PlatformAdProviderComponents
        {
            get
            {
                return AdRotatorComponent.PlatformAdProviderComponents;
            }
        }
        #endregion



        #endregion
    }
}
