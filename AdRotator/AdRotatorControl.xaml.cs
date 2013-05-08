using AdRotator.Model;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace AdRotator
{
    public partial class AdRotatorControl : UserControl, IAdRotatorProvider
    {
        private AdRotatorComponent adRotatorControl = new AdRotatorComponent(Thread.CurrentThread.CurrentUICulture.ToString(), new FileHelpers());

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
            InitializeComponent();
            Loaded += AdRotatorControl_Loaded;

            // List of AdProviders supportd on this platform
            adRotatorControl.PlatformSupportedAdProviders = new AdType[3] 
                { 
                    AdType.AdDuplex, 
                    AdType.PubCenter, 
                    AdType.Smaato 
                };


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
                adRotatorControl.GetConfig();
            }

            adRotatorControl.isLoaded = true;

        }

        public string Invalidate(AdProvider adProvider)
        {
            if (adProvider == null)
            {
                adProvider = adRotatorControl.GetAd();
            }
            var provider = AdProviderConfig.AdProviderConfigValues[adProvider.AdProviderType];
            Type providerType = TryGetType(provider.AssemblyName, provider.ElementName);
            var instance = Activator.CreateInstance(providerType);
            if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.AppId))
            {
                TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.AppId], adProvider.AppId.ToString());
            }

            if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.SecondaryId))
            {
                TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.SecondaryId], adProvider.SecondaryId.ToString());
            } 
            
            if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.IsTest))
            {
                TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.IsTest], adProvider.IsTest.ToString());
            }

            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add((FrameworkElement)instance);
            return adProvider.AdProviderType.ToString();
        }

        public static Type TryGetType(string assemblyName, string typeName)
        {
            try
            {
                var assem = Assembly.Load(assemblyName);
                    Type t = assem.GetType(typeName, false);
                    if (t != null) { return t; }
            }
            catch { }

            return null;
        }
        public static void TrySetProperty(object instance, string PropertyName, string PropertyValue)
        {
            try
            {
                PropertyInfo propertyInfo = instance.GetType().GetProperty(PropertyName);
                propertyInfo.SetValue(instance, Convert.ChangeType(PropertyValue, propertyInfo.PropertyType, Thread.CurrentThread.CurrentUICulture), null);
            }
            catch { }
        }

        #region RemoteSettingsLocation
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
            DependencyProperty.Register("IsAdRotatorEnabled", typeof(bool), typeof(AdRotatorControl), new PropertyMetadata(true,IsAdRotatorEnabledPropertyChanged));

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


    }
}
