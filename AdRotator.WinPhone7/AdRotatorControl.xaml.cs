using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AdRotator.Interface;
using AdRotator.Model;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Shell;

namespace AdRotator.WinPhone7
{
    public partial class AdRotatorControl : UserControl, IAdRotatorProvider
    {
        private AdRotator adRotatorControl;
        private AdProvider currentAdProvider;

        private bool isLoaded;
        private bool isInitialised;
        private bool isNetworkEnabled;


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
        }

        void AdRotatorControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsInDesignMode)
            {
                LayoutRoot.Children.Add(new TextBlock() { Text = "AdRotator in design mode, No ads will be displayed", VerticalAlignment = System.Windows.VerticalAlignment.Center });
            }

            if (!NetworkInterface.GetIsNetworkAvailable() || NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
            {
                isNetworkEnabled = true;
            }

            isLoaded = true;
        }

        public bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(this);
            }
        }
        
        public string RemoteSettingsLocation
        {
            get { return (string)GetValue(RemoteSettingsLocationProperty); }
            set { SetValue(RemoteSettingsLocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RemoteSettingsLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RemoteSettingsLocationProperty =
            DependencyProperty.Register("RemoteSettingsLocation", typeof(string), typeof(AdRotatorControl), new PropertyMetadata(string.Empty));



        public string LocalSettingsLocation
        {
            get { return (string)GetValue(LocalSettingsLocationProperty); }
            set { SetValue(LocalSettingsLocationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocalSettingsLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocalSettingsLocationProperty =
            DependencyProperty.Register("LocalSettingsLocation", typeof(string), typeof(AdRotatorControl), new PropertyMetadata(string.Empty));





        public bool IsAdRotatorEnabled
        {
            get { return (bool)GetValue(IsAdRotatorEnabledProperty); }
            set { SetValue(IsAdRotatorEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsAdRotatorEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsAdRotatorEnabledProperty =
            DependencyProperty.Register("IsAdRotatorEnabled", typeof(bool), typeof(AdRotatorControl), new PropertyMetadata(true));

        


        public object DefaultHouseAdBody
        {
            get { return (object)GetValue(DefaultHouseAdBodyProperty); }
            set { SetValue(DefaultHouseAdBodyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DefaultHouseAdBody.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultHouseAdBodyProperty =
            DependencyProperty.Register("DefaultHouseAdBody", typeof(object), typeof(AdRotatorControl), new PropertyMetadata(null));

        
        

        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }
        }

        public bool IsInitialised
        {
            get
            {
                return isInitialised;
            }
        }

        public bool IsNetworkEnabled
        {
            get
            {
                return isNetworkEnabled;
            }
            set
            {
                isNetworkEnabled = value;
            }
        }

        public Model.AdSettings LoadSettingsFileLocal()
        {
            return null;
        }

        public Model.AdSettings LoadSettingsFileRemote()
        {
            return null;
        }

        public Model.AdSettings LoadSettingsFileProject()
        {
            return null;
        }


        public string Invalidate()
        {
            throw new NotImplementedException();
        }
    }
}
