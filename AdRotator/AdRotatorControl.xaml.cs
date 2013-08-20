using AdRotator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace AdRotator
{
    public partial class AdRotatorControl : UserControl, IAdRotatorProvider
    {
        #region Logging Event Code
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

        private AdRotatorComponent adRotatorControl = new AdRotatorComponent(Thread.CurrentThread.CurrentUICulture.ToString(), new FileHelpers());
#if WINPHONE7
        AdRotator.AdProviderConfig.SupportedPlatforms CurrentPlatform = AdRotator.AdProviderConfig.SupportedPlatforms.WindowsPhone7;
#else
        AdRotator.AdProviderConfig.SupportedPlatforms CurrentPlatform = AdRotator.AdProviderConfig.SupportedPlatforms.WindowsPhone8;
#endif

        public AdRotatorControl()
        {
            InitializeComponent();
            Loaded += AdRotatorControl_Loaded;

            // List of AdProviders supportd on this platform
            AdRotatorComponent.PlatformSupportedAdProviders = new List<AdType>()
                { 
                    AdType.AdDuplex, 
                    AdType.PubCenter, 
                    AdType.Smaato,
                    AdType.Inmobi,
                    AdType.MobFox,
                    AdType.AdMob,
                    AdType.InnerActive
                };
            adRotatorControl.Log += (s) => { OnLog(s); };
            
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
                LayoutRoot.Children.Add(new TextBlock() { Text = "AdRotator in design mode, No ads will be displayed", VerticalAlignment = System.Windows.VerticalAlignment.Center });
            }
            else
            {
                adRotatorControl.AdAvailable += adRotatorControl_AdAvailable;
                if (AutoStartAds)
                {
                    adRotatorControl.GetConfig();
                    if (!adRotatorControl.adRotatorRefreshIntervalSet)
                    {
                        adRotatorControl.StartAdTimer();
                    }
                }
            }

            adRotatorControl.isLoaded = true;

        }

        public string Invalidate(AdProvider adProvider)
        {
            if (adProvider == null)
            {
                adRotatorControl.GetAd(null);
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
            catch
            {
                adRotatorControl.AdFailed(adProvider.AdProviderType);
                return "Ad Failed to initialise";
            }
            if (providerElement == null)
            {
                adRotatorControl.AdFailed(adProvider.AdProviderType);
                return "No Ad Returned";
            }
            Dispatcher.BeginInvoke(() =>
                {
                    LayoutRoot.Children.Clear();
                    LayoutRoot.Children.Add((FrameworkElement)providerElement);
                    OnLog(string.Format("Displaying ads for {0}", adProvider.AdProviderType));
                });
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
                return DesignerProperties.GetIsInDesignMode(this);
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
            DependencyProperty.Register("RemoteSettingsLocation", typeof(string), typeof(AdRotatorControl), new PropertyMetadata(string.Empty,RemoteSettingsLocationPropertyChanged));

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
            DependencyProperty.Register("LocalSettingsLocation", typeof(string), typeof(AdRotatorControl), new PropertyMetadata(string.Empty,LocalSettingsLocationPropertyChanged));

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
            DependencyProperty.Register("IsAdRotatorEnabled", typeof(bool), typeof(AdRotatorControl), new PropertyMetadata(false,IsAdRotatorEnabledPropertyChanged));

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
            DependencyProperty.Register("DefaultHouseAdBody", typeof(object), typeof(AdRotatorControl), new PropertyMetadata(null,AdRotatorEnabledPropertyChanged));

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
        public Dictionary<AdType, Type> PlatformAdProviderComponents
        {
            get
            {
                return AdRotatorComponent.PlatformAdProviderComponents;
            }
        }
        #endregion

        #region AutoStartAds
        public bool AutoStartAds
        {
            get { return (bool)adRotatorControl.autoStartAds; }
            set { SetValue(AutoStartAdsProperty, value); }
        }

        public static readonly DependencyProperty AutoStartAdsProperty =
            DependencyProperty.Register("AutoStartAds", typeof(bool), typeof(AdRotatorControl), new PropertyMetadata(false, AutoStartAdsPropertyChanged));

        private static void AutoStartAdsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.OnAutoStartAdsPropertyChanged(e);
            }
        }

        private void OnAutoStartAdsPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.autoStartAds = (bool)e.NewValue;
        }
        #endregion

        #region AdRefreshInterval
        /// <summary>
        /// Another notes about the Ad Refresh Rate
        /// </summary>
        public int AdRefreshInterval
        {
            get { return (int)adRotatorControl.adRotatorRefreshInterval; }
            set { SetValue(AutoStartAdsProperty, value); }
        }

        /// <summary>
        /// Sets the Ad Refresh rate in seconds
        /// *Note minimum is 60 seconds
        /// </summary>
        public static readonly DependencyProperty AdRefreshIntervalProperty =
            DependencyProperty.Register("AdRefreshInterval", typeof(int), typeof(AdRotatorControl), new PropertyMetadata(60, AdRefreshIntervalChanged));

        private static void AdRefreshIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.OnAdRefreshIntervalPropertyChanged(e);
            }
        }

        private void OnAdRefreshIntervalPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            adRotatorControl.adRotatorRefreshInterval = (int)e.NewValue;
            adRotatorControl.adRotatorRefreshIntervalSet = true;
            adRotatorControl.StartAdTimer();
        }
        #endregion

        #endregion

    }
}
