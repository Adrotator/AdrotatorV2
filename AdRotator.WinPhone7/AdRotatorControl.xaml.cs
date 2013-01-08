using AdRotator.Model;
using Microsoft.Phone.Net.NetworkInformation;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;

namespace AdRotator.WinPhone7
{
    public partial class AdRotatorControl : UserControl, IAdRotatorProvider
    {
        private AdRotator adRotatorControl;
        private string adSettingsString = "";
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
            // This call needs to happen when the control is loaded 
            // b/c dependency properties are propagated to their values at this point
            LoadAdSettings();

            if (IsInDesignMode)
            {
                LayoutRoot.Children.Add(new TextBlock() { Text = "AdRotator in design mode, No ads will be displayed", VerticalAlignment = System.Windows.VerticalAlignment.Center });
            }

            if (!NetworkInterface.GetIsNetworkAvailable() || NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
            {
                isNetworkEnabled = true;
            }

            isLoaded = true;
            if (!string.IsNullOrEmpty(adSettingsString))
            {
                adRotatorControl = new AdRotator(adSettingsString, Thread.CurrentThread.CurrentUICulture.ToString());
                Invalidate();
            }
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

        #region AdSettings File retrieval

        /// <summary>
        /// Loads the ad settings object either from isolated storage or from the resource path defined in DefaultSettingsFileUri.
        /// </summary>
        /// <returns></returns>
        private async void LoadAdSettings()
        {

            //If not checked remote && network available - get remote
            if (!String.IsNullOrEmpty(RemoteSettingsLocation) && isNetworkEnabled)
            {
                adSettingsString = await LoadSettingsFileRemote(RemoteSettingsLocation);
            }            

            if (string.IsNullOrEmpty(adSettingsString))
            {
                adSettingsString = LoadSettingsFileLocal();
                if (String.IsNullOrEmpty(adSettingsString))
                {
                    adSettingsString = LoadSettingsFileProject();
                }
            }            

            if (IsLoaded)
            {
                adRotatorControl = new AdRotator(adSettingsString, Thread.CurrentThread.CurrentUICulture.ToString());
                Invalidate();
            }
        }

        //not finished (SJ)
        public string LoadSettingsFileLocal()
        {
            // if successful set and invalidate
            // If not loadSettings again
            string AdSettingsString = "";

            var SETTINGS_FILE_NAME = string.IsNullOrEmpty(LocalSettingsLocation) ? AdRotator.SETTINGS_FILE_NAME : LocalSettingsLocation;
            try
            {
                var isfData = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream isfStream = null;
                if (isfData.FileExists(SETTINGS_FILE_NAME))
                {
                    using (isfStream = new IsolatedStorageFileStream(SETTINGS_FILE_NAME, FileMode.Open, isfData))
                    {
                        try
                        {
                            //AdSettingsString = new TextReader().ReadAsync(isfStream);
                        }
                        catch { }
                    }
                }
            }
            catch (IsolatedStorageException)
            {
            }

            return AdSettingsString;

        }

        public async Task<string> LoadSettingsFileRemote(string RemoteSettingsLocation)
        {
            return await Networking.Network.GetStringFromURL(RemoteSettingsLocation);
        }

        //Not Finished (SJ)
        //Needs testing (GO)
        public string LoadSettingsFileProject()
        {
            string projectAdSettingsString = "";
            if (LocalSettingsLocation != null)
            {
                try
                {
                    var localSettingsFileInfo = Application.GetResourceStream(new Uri(LocalSettingsLocation,UriKind.Relative));
                    if (localSettingsFileInfo == null)
                    {
                        // TODO: log this properly
                        Debug.WriteLine("The ad configuration file " + LocalSettingsLocation + " could not be found. Either the path is incorrect or the build type is not set to resource");
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(localSettingsFileInfo.Stream))
                        {
                            projectAdSettingsString = reader.ReadToEnd();
                        }
                    }
                }
                catch(Exception ex) {
                }
            }
            return projectAdSettingsString;
        }

        #endregion

        public string Invalidate()
        {
            var adProvider = adRotatorControl.GetAd();
            var winPhone7AdProvider = AdProviderWinPhone7.CreateWinPhone7AdProvider(adProvider);
            var element = winPhone7AdProvider.GetVisualElement();
            LayoutRoot.Children.Clear();
            LayoutRoot.Children.Add(element);
            return "";
        }

    }
}
