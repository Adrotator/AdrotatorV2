using AdRotator.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
#if WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media.Animation;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
#endif

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace AdRotator
{
    public sealed class AdRotatorControl : Control, IAdRotatorProvider, IDisposable
    {
        private AdRotatorComponent adRotatorControl = new AdRotatorComponent(CultureInfo.CurrentUICulture.ToString(), new FileHelpers());
#if WINDOWS_PHONE
        AdRotator.AdProviderConfig.SupportedPlatforms CurrentPlatform = AdRotator.AdProviderConfig.SupportedPlatforms.WindowsPhone8;
#else
        AdRotator.AdProviderConfig.SupportedPlatforms CurrentPlatform = AdRotator.AdProviderConfig.SupportedPlatforms.Windows8;
#endif
        #region LoggingEventCode
        public delegate void LogHandler(string message);
        public event LogHandler Log;
        internal void OnLog(string message)
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
                    AdType.Inmobi,
#if WINDOWS_PHONE
                    AdType.Smaato,
                    AdType.MobFox,
                    AdType.AdMob,
                    AdType.InnerActive,
#endif
                };
            adRotatorControl.Log += (s) => OnLog(s);
        }

        private Grid AdRotatorRoot
        {
            get
            {
                return GetTemplateChild("AdRotatorRoot") as Grid;
            }
        }

        void adRotatorControl_AdAvailable(AdProvider adProvider)
        {
            Invalidate(adProvider);
        }

        private bool templateApplied;

#if WINDOWS_PHONE
        public override void OnApplyTemplate()

#else
        protected override void OnApplyTemplate()
#endif
        {
            base.OnApplyTemplate();
            if (IsInDesignMode)
            {
                AdRotatorRoot.Children.Add(new TextBlock() { Text = "AdRotator in design mode, No ads will be displayed", VerticalAlignment = VerticalAlignment.Center });
            }
            else
            {
                adRotatorControl.AdAvailable += adRotatorControl_AdAvailable;
                adRotatorControl.AdAvailable += adRotatorControl_AdAvailable;
                if (AutoStartAds)
                {
                    adRotatorControl.GetConfig();
                    if (!adRotatorControl.adRotatorRefreshIntervalSet)
                    {
                        adRotatorControl.StartAdTimer();
                    }
                }
                InitialiseSlidingAnimations();
            }
            templateApplied = true;
            OnAdRotatorReady();
            if (adRotatorControl.isLoaded)
            {
                Invalidate(null);
            }

        }
        void AdRotatorControl_Loaded(object sender, RoutedEventArgs e)
        {
            // This call needs to happen when the control is loaded 
            // b/c dependency properties are propagated to their values at this point



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

            if (SlidingAdDirection != AdSlideDirection.None && !_slidingAdTimerStarted)
            {
                _slidingAdTimerStarted = true;
                ResetSlidingAdTimer(SlidingAdDisplaySeconds);
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

            AdRotatorRoot.Children.Clear();
            AdRotatorRoot.Children.Add((FrameworkElement)providerElement);
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
#if WINDOWS_PHONE
                return DesignerProperties.GetIsInDesignMode(this);
#else
                return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
#endif
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

        #region DefaultHouseAd

        #region DefaultHouseAdBody
        public string DefaultHouseAdBody
        {
            get { return (string)GetValue(DefaultHouseAdBodyProperty);}
            set { SetValue(DefaultHouseAdBodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultHouseAdBody.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultHouseAdBodyProperty =
            DependencyProperty.Register("DefaultHouseAdBody", typeof(string), typeof(AdRotatorControl), new PropertyMetadata(string.Empty));

        #endregion

        #region DefaultHouseAdURI

        public string DefaultHouseAdURI
        {
            get { return (string)GetValue(DefaultHouseAdURIProperty);}
            set { SetValue(DefaultHouseAdURIProperty, value); }
        }

        public static readonly DependencyProperty DefaultHouseAdURIProperty = DependencyProperty.Register("DefaultHouseAdURI", typeof(string), typeof(AdRotatorControl), new PropertyMetadata(String.Empty));

        #endregion

        public delegate void DefaultHouseAdClickEventHandler();

        public event DefaultHouseAdClickEventHandler DefaultHouseAdClick;
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

        #region AutoStartAds
        public bool AutoStartAds
        {
            get { return (bool)adRotatorControl.autoStartAds; }
            set { SetValue(AutoStartAdsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAdRotatorEnabled.  This enables animation, styling, binding, etc...
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

        #region SlidingAd Properties

        Storyboard SlidingAdTimer;
        Storyboard SlideOutLRAdStoryboard;
        Storyboard SlideInLRAdStoryboard;
        Storyboard SlideOutUDAdStoryboard;
        Storyboard SlideInUDAdStoryboard;

        private bool _slidingAdHidden = false;

        private bool _slidingAdTimerStarted = false;

        #region SlidingAdDirection

        /// <summary>
        /// Direction the popup will hide / appear from
        /// If not set the AdControl will remain on screen
        /// </summary>
        public AdSlideDirection SlidingAdDirection
        {
            get { return (AdSlideDirection)GetValue(SlidingAdDirectionProperty); }
            set { SetValue(SlidingAdDirectionProperty, value); }
        }

        public static readonly DependencyProperty SlidingAdDirectionProperty = DependencyProperty.Register("SlidingAdDirection", typeof(AdSlideDirection), typeof(AdRotatorControl), new PropertyMetadata(AdSlideDirection.None, SlidingAdDirectionChanged));

        private static void SlidingAdDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AdRotatorControl;
            if (sender != null)
            {
                sender.OnSlidingAdDirectionChanged(e);
            }
        }

        private void OnSlidingAdDirectionChanged(DependencyPropertyChangedEventArgs e)
        {
            SetupAnimationBounds((AdSlideDirection)e.NewValue);
        }

        private void SetupAnimationBounds(AdSlideDirection adSlideDirection)
        {
            if (AdRotatorRoot != null)
            {
#if WINDOWS_PHONE
                var bounds = AdRotatorControl.DisplayResolution;
#else
                var bounds = Window.Current.Bounds;
#endif
                switch (adSlideDirection)
                {
                    case AdSlideDirection.Left:
                        ((DoubleAnimation)SlideOutLRAdStoryboard.Children[0]).To = -(bounds.Width * 2);
                        ((DoubleAnimation)SlideInLRAdStoryboard.Children[0]).From = -(bounds.Width * 2);
                        break;
                    case AdSlideDirection.Right:
                        ((DoubleAnimation)SlideOutLRAdStoryboard.Children[0]).To = bounds.Width * 2;
                        ((DoubleAnimation)SlideInLRAdStoryboard.Children[0]).From = bounds.Width * 2;
                        break;
                    case AdSlideDirection.Bottom:
                        ((DoubleAnimation)SlideOutUDAdStoryboard.Children[0]).To = bounds.Height * 2;
                        ((DoubleAnimation)SlideInUDAdStoryboard.Children[0]).From = bounds.Height * 2;
                        break;
                    case AdSlideDirection.Top:
                        ((DoubleAnimation)SlideOutUDAdStoryboard.Children[0]).To = -(bounds.Height * 2);
                        ((DoubleAnimation)SlideInUDAdStoryboard.Children[0]).From = -(bounds.Height * 2);
                        break;
                    default:
                        ((DoubleAnimation)SlideOutLRAdStoryboard.Children[0]).To = 0;
                        ((DoubleAnimation)SlideInLRAdStoryboard.Children[0]).From = 0;
                        ((DoubleAnimation)SlideOutUDAdStoryboard.Children[0]).To = 0;
                        ((DoubleAnimation)SlideInUDAdStoryboard.Children[0]).From = 0;
                        break;
                }
            }
        }


        #endregion

        #region SlidingAdDisplaySeconds

        /// <summary>
        /// Amount of time in seconds the ad is displayed on Screen if <see cref="SlidingAdDirection"/> is set to something else than None
        /// </summary>
        public int SlidingAdDisplaySeconds
        {
            get { return (int)GetValue(SlidingAdDisplaySecondsProperty); }
            set { SetValue(SlidingAdDisplaySecondsProperty, value); }
        }

        public static readonly DependencyProperty SlidingAdDisplaySecondsProperty = DependencyProperty.Register("SlidingAdDisplaySeconds", typeof(int), typeof(AdRotatorControl), new PropertyMetadata(10));

        #endregion

        #region SlidingAdHiddenSeconds

        /// <summary>
        ///  Amount of time in seconds to wait before displaying the ad again 
        ///  (if <see cref="SlidingAdDirection"/> is set to something else than None).
        ///  Basically the lower this number the more the user is "nagged" by the ad coming back now and again
        /// </summary>
        public int SlidingAdHiddenSeconds
        {
            get { return (int)GetValue(SlidingAdHiddenSecondsProperty); }
            set { SetValue(SlidingAdHiddenSecondsProperty, value); }
        }

        public static readonly DependencyProperty SlidingAdHiddenSecondsProperty = DependencyProperty.Register("SlidingAdHiddenSeconds", typeof(int), typeof(AdRotatorControl), new PropertyMetadata(20));

        #endregion

        #region Animation Events
        private void SlideOutAdStoryboard_Completed(object sender, object e)
        {
            _slidingAdHidden = true;
            Invalidate(null);
            ResetSlidingAdTimer(SlidingAdHiddenSeconds);
        }

        private void SlideInAdStoryboard_Completed(object sender, object e)
        {
            _slidingAdHidden = false;
            ResetSlidingAdTimer(SlidingAdDisplaySeconds);
        }

        private void ResetSlidingAdTimer(int durationInSeconds)
        {
            if (IsAdRotatorEnabled)
            {
                SlidingAdTimer.Duration = new Duration(new TimeSpan(0, 0, durationInSeconds));
                SlidingAdTimer.Begin();
            }
        }

        private void SlidingAdTimer_Completed(object sender, object e)
        {
            switch (SlidingAdDirection)
            {
                case AdSlideDirection.Top:
                case AdSlideDirection.Bottom:
                    if (_slidingAdHidden)
                    {
                        SlideInUDAdStoryboard.Begin();
                    }
                    else
                    {
                        SlideOutUDAdStoryboard.Begin();
                    }
                    break;
                case AdSlideDirection.Left:
                case AdSlideDirection.Right:
                    if (_slidingAdHidden)
                    {
                        SlideInLRAdStoryboard.Begin();
                    }
                    else
                    {
                        SlideOutLRAdStoryboard.Begin();
                    }
                    break;
                default:
                    break;
            }
        }

        void InitialiseSlidingAnimations()
        {
            SlidingAdTimer = new Storyboard();
            SlidingAdTimer.Completed += SlidingAdTimer_Completed;

            DoubleAnimation SlideOutLRAdStoryboardAnimation = new DoubleAnimation();
            Storyboard.SetTarget(SlideOutLRAdStoryboardAnimation, AdRotatorRoot);
#if WINDOWS_PHONE
            Storyboard.SetTargetProperty(SlideOutLRAdStoryboardAnimation, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)"));
#else
            Storyboard.SetTargetProperty(SlideOutLRAdStoryboardAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
#endif
            SlideOutLRAdStoryboardAnimation.Completed += SlideOutAdStoryboard_Completed;

            SlideOutLRAdStoryboard = new Storyboard();
            SlideOutLRAdStoryboard.Children.Add(SlideOutLRAdStoryboardAnimation);


            DoubleAnimation SlideInLRAdStoryboardAnimation = new DoubleAnimation();
            Storyboard.SetTarget(SlideInLRAdStoryboardAnimation, AdRotatorRoot);
#if WINDOWS_PHONE
            Storyboard.SetTargetProperty(SlideInLRAdStoryboardAnimation, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)"));
 #else
            Storyboard.SetTargetProperty(SlideInLRAdStoryboardAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateX)");
#endif
            SlideInLRAdStoryboardAnimation.Completed += SlideInAdStoryboard_Completed;

            SlideInLRAdStoryboard = new Storyboard();
            SlideInLRAdStoryboard.Children.Add(SlideInLRAdStoryboardAnimation);

            DoubleAnimation SlideOutUDAdStoryboardAnimation = new DoubleAnimation();
            Storyboard.SetTarget(SlideOutUDAdStoryboardAnimation, AdRotatorRoot);
#if WINDOWS_PHONE
            Storyboard.SetTargetProperty(SlideOutUDAdStoryboardAnimation, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));
 #else
            Storyboard.SetTargetProperty(SlideOutUDAdStoryboardAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
#endif
            SlideOutUDAdStoryboardAnimation.Completed += SlideOutAdStoryboard_Completed;

            SlideOutUDAdStoryboard = new Storyboard();
            SlideOutUDAdStoryboard.Children.Add(SlideOutUDAdStoryboardAnimation);

            DoubleAnimation SlideInUDAdStoryboardAnimation = new DoubleAnimation();
            Storyboard.SetTarget(SlideInUDAdStoryboardAnimation, AdRotatorRoot);
#if WINDOWS_PHONE
            Storyboard.SetTargetProperty(SlideInUDAdStoryboardAnimation, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));
 #else
            Storyboard.SetTargetProperty(SlideInUDAdStoryboardAnimation, "(UIElement.RenderTransform).(CompositeTransform.TranslateY)");
#endif
            SlideInUDAdStoryboardAnimation.Completed += SlideInAdStoryboard_Completed;

            SlideInUDAdStoryboard = new Storyboard();
            SlideInUDAdStoryboard.Children.Add(SlideInUDAdStoryboardAnimation);

            SetupAnimationBounds(SlidingAdDirection);
        }
        #endregion
        #endregion

        #region WindowsPhone Screen size detection
#if WINDOWS_PHONE
        public static Size DisplayResolution
        {
            get
            {
                if (Environment.OSVersion.Version.Major < 8)
                    return new Size(480, 800);
                int scaleFactor = (int)GetProperty(Application.Current.Host.Content, "ScaleFactor");
                switch (scaleFactor)
                {
                    case 100:
                        return new Size(480, 800);
                    case 150:
                        return new Size(720, 1280);
                    case 160:
                        return new Size(768, 1280);
                }
                return new Size(480, 800);
            }
        }
        private static object GetProperty(object instance, string name)
        {
            var getMethod = instance.GetType().GetProperty(name).GetGetMethod();
            return getMethod.Invoke(instance, null);
        } 
#endif
        #endregion

        #region AdRotatorReadyEvent

        public delegate void AdRotatorReadyStatus();

        public event AdRotatorReadyStatus AdRotatorReady;

        private void OnAdRotatorReady()
        {
            if (AdRotatorReady != null)
            {
                AdRotatorReady();
            }
        }
        #endregion

        public void Dispose()
        {
            AdRotatorRoot.Children.Clear();
            //providerElement = null;
            DefaultHouseAdBody = null;
        }
    }
}
