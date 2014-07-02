using AdRotator.Utilities;
using System;
using System.Reflection;
using System.Threading.Tasks;
#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Markup;
#elif NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
#endif


namespace AdRotator
{
    class DefaultHouseAd : FrameworkElement
    {
        public object HouseAdBody;

        public string DefaultHouseAdURL;

        private int adRotatorControlID;

        public delegate void OnAdFailed(object sender, EventArgs e);
        public delegate void OnAdLoaded(object sender, EventArgs e);
        public delegate void OnAdClicked(object sender, EventArgs e);

        public event OnAdFailed AdLoadingFailed;
        public event OnAdLoaded AdLoaded;
        public event OnAdClicked AdClicked;

        private IFileHelpers fileHelper;

        private FrameworkElement Content { get; set; }

        public DefaultHouseAd(int AdRotatorControlID, IFileHelpers FileHelper)
        {
            this.fileHelper = FileHelper;
            this.adRotatorControlID = AdRotatorControlID;
        }

        public async Task<FrameworkElement> Initialise(string LocalHouseAdBodyName, string URL = "")
        {
            await GetRemoteHouseAdURL(LocalHouseAdBodyName, URL);
#if WINDOWS_PHONE
            this.Tap += DefaultHouseAd_Tapped;
#elif NETFX_CORE
            this.Tapped += DefaultHouseAd_Tapped;
#endif

            return Content;
        }



        private async Task GetRemoteHouseAdURL(string LocalHouseAdBodyName, string URL)
        {
            Object o = null;
            try
            {
                var asm = GetAssemblyFromClass(LocalHouseAdBodyName);
                Type t = asm.GetType(LocalHouseAdBodyName);
                o = Activator.CreateInstance(t);
            }
            catch 
            {
                AdRotatorControl.OnLog(adRotatorControlID, "Failed to resolve DefaultAd Class definition - not found");
            }


            //check to see if the class is instantiated or not
            if (o != null)
            {
                HouseAdBody = o;
            }

            if (!string.IsNullOrEmpty(URL))
            {
                DefaultHouseAdURL = URL;
                await GetRemoteHouseAdControl();
            }
            else
            {
                LoadProjectDefaultAd();
            }
        }

        private static Assembly GetAssemblyFromClass(string LocalHouseAdBodyName)
        {
            Assembly resolvedAssembly = null;
            var classDefinition = LocalHouseAdBodyName.Split('.');
            var assemblyLength = classDefinition.Length - 1;
            for (int i = assemblyLength; i > 1; i--)
            {
                try
                {
                    var assemblyNameValue = LocalHouseAdBodyName.Substring(0, LocalHouseAdBodyName.IndexOf(classDefinition[i]) - 1);
                    AssemblyName name = new AssemblyName(assemblyNameValue);
                    resolvedAssembly = Assembly.Load(name);
                    if (resolvedAssembly != null)
                    {
                        break;
                    }
                }
                catch { }
            }

            return resolvedAssembly;
        }

#if WINDOWS_PHONE
        void DefaultHouseAd_Tapped(object sender, System.Windows.Input.GestureEventArgs e)
#elif NETFX_CORE
        void DefaultHouseAd_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
#endif
        {

            if (AdClicked != null)
            {
                AdClicked("", new EventArgs());
            }
        }

        private async Task GetRemoteHouseAdControl()
        {
            try
            {
                //Remote Ad Process
                if (DefaultHouseAdURL != null)
                {
                    var s = await Networking.Network.GetStringFromURLAsync(DefaultHouseAdURL);

                    if (string.IsNullOrEmpty(s))
                    {
                        await LoadCachedAd();
                    }
                    else
                    {
                        try
                        {
                            this.Content = (FrameworkElement)XamlReader.Load(s);
                            await fileHelper.SaveData("RemoteDefaultHouseAd", s);
                            if (AdLoaded != null)
                            {
                                AdLoaded(null, new EventArgs());
                            }
                        }
                        catch 
                        {
                            LoadCachedAd();
                        }
                    }
                }
                else
                {
                    await LoadCachedAd();
                }
            }
            catch
            {
                if (HouseAdBody == null)
                {
                    if (AdLoadingFailed != null)
                    {
                        AdLoadingFailed("", new EventArgs());
                    }
                }
                else
                {
                    LoadCachedAd();
                }
            }
        }

        private async Task LoadCachedAd()
        {
            try
            {
                var remoteDefaultHouseAd = await fileHelper.LoadData("RemoteDefaultHouseAd");

                if (!string.IsNullOrEmpty(remoteDefaultHouseAd))
                {
                    try
                    {
                        this.Content = (FrameworkElement)XamlReader.Load(remoteDefaultHouseAd);
                        if (AdLoaded != null)
                        {
                            AdLoaded(null, new EventArgs());
                        }
                    }
                    catch 
                    {
                        LoadProjectDefaultAd();
                    }
                }
                else
                {
                    LoadProjectDefaultAd();
                }
            }
            catch (Exception error)
            {
                var value = error;
                if (AdLoadingFailed != null)
                {
                    AdLoadingFailed("", new EventArgs());
                }
            }
        }

        private void LoadProjectDefaultAd()
        {
            if (HouseAdBody == null)
            {
                if (AdLoadingFailed != null)
                {
                    AdLoadingFailed("Get Remote Ad Failed", new EventArgs());
                }
            }
            else
            {
                this.Content = (FrameworkElement)HouseAdBody;
                if (AdLoaded != null)
                {
                    AdLoaded(null, new EventArgs());
                }
            }
        }

    }
}
