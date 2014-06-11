using AdRotator.Model;
using AdRotator.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AdRotator
{
    /// <summary>
    /// *Notes (food for thought)
    ///     - Ad Validity checking?
    /// </summary>
    internal partial class AdRotatorComponent
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

        #region AdAvailableEventCode

        public delegate void AdAvailableHandler(AdProvider adProvider);

        public event AdAvailableHandler AdAvailable;

        protected void OnAdAvailable(AdProvider adProvider)
        {
            if (AdAvailable != null)
            {
                OnLog(string.Format("Trying provider {0}", adProvider.AdProviderType));
                AdAvailable(adProvider);
            }
        }

        #endregion

        #region Properties

        const int AdRotatorMinRefreshRate = 60;

        /// <summary>
        /// The ad settings based on which the ad descriptor for the current UI culture can be selected
        /// </summary>
        private static AdSettings _settings;

        private string culture;

        /// <summary>
        /// Random generator
        /// </summary>
        internal static Random _rnd = new Random();

        /// <summary>
        /// State of the current Ad Display order (of provided)
        /// </summary>
        private int OrderIndex = 0;

        private IFileHelpers fileHelper;
        private ReflectionHelpers reflectionHelper = new ReflectionHelpers();

        private static Timer adRotatorTimer;

        private static TimerCallback timerDelegate;

        private int _adRotatorRefreshInterval;


        internal int AdWidth { get; set; }

        internal int AdHeight { get; set; }

        internal bool IsAdRotatorEnabled { get; set; }

        internal string RemoteSettingsLocation { get; set; }

        internal string LocalSettingsLocation { get; set; }

        internal object DefaultHouseAdBody { get; set; }

        internal bool isLoaded { get; set; }

        internal bool isInitialised { get; set; }

        internal bool isTest { get; set; }

        internal bool autoStartAds { get; set; }

        internal static List<AdType> PlatformSupportedAdProviders { get; set; }

        internal static Dictionary<AdType,Type> PlatformAdProviderComponents { get; set; }


        /// <summary>
        /// RefreshInterval in seconds
        /// Control the minimum value so that AdProviders are not abused, or set to 0 to disable auto refresh (just the live provider will be used)
        /// </summary>
        internal int adRotatorRefreshInterval { get { return _adRotatorRefreshInterval; } set { _adRotatorRefreshInterval = value == 0 ? 0 : Math.Max(AdRotatorMinRefreshRate, value); } }
        internal bool adRotatorRefreshIntervalSet = false;
        #endregion


        //Discuss - can we use calling asembly to run functions in child projects?, that way we can force calling remote functions in the core project. This would controll project flow.
        /// <summary>
        /// AdRotator Initialiser
        /// </summary>
        /// <param name="adSettings">XML string of the AdSettings content</param>
        /// <param name="Culture">Specified culture you want AdRotator initialised for</param>
        public AdRotatorComponent(string Culture, IFileHelpers FileHelper)
        {
            this.IsAdRotatorEnabled = true;
            this.culture = Culture;
            this.fileHelper = FileHelper;
            this.AdHeight = 80;
            this.AdWidth = 480;
            PlatformSupportedAdProviders = new List<AdType>();
            PlatformAdProviderComponents = new Dictionary<AdType, Type>();

            timerDelegate = new TimerCallback(GetAd);
        }

        internal async void GetConfig()
        {
            try
            {
                await LoadAdSettings();
            }
            catch { }

            if (_settings != null && _settings.CultureDescriptors.Count() > 0)
            {
                //Set Current culture based on Culture Value
               _settings.GetAdDescriptorBasedOnUICulture(culture);
            }
            if (adRotatorRefreshInterval == 0)
            {
                OnAdAvailable(_settings.GetAd());
            } 
        }

        internal void GetAd(Object stateInfo)
        {
            if (_settings == null)
            {
                GetConfig();
            }
            OnAdAvailable(_settings.GetAd());
        }

        internal object GetProviderFrameworkElement(AdRotator.AdProviderConfig.SupportedPlatforms platform, AdProvider adProvider)
        {
            var provider = adProvider.AdProviderConfigValues[platform];
            Type providerType = null;
            object instance;
            try
            {
                if (PlatformAdProviderComponents.ContainsKey(adProvider.AdProviderType))
                {
                    providerType = PlatformAdProviderComponents[adProvider.AdProviderType];
                }
                else
                {
                    providerType = reflectionHelper.TryGetType(provider.AssemblyName, provider.ElementName);
                }
            }
            catch (PlatformNotSupportedException)
            {
                AdFailed(adProvider.AdProviderType);
            }
            if (providerType == null)
            {
                return null;
            }
            try
            {
                instance = Activator.CreateInstance(providerType);

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.AppId))
                {
                    reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.AppId], adProvider.AppId.ToString());
                }

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.SecondaryId))
                {
                    reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.SecondaryId], adProvider.SecondaryId.ToString());
                }

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.AdType))
                {
                    reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.AdType], "IaAdType_Banner");
                }

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.IsTest))
                {
                    reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.IsTest], adProvider.IsTest.ToString());
                }

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.AdWidth))
                {
                    reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.AdWidth], AdWidth.ToString());
                }

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.AdHeight))
                {
                    reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.AdHeight], AdHeight.ToString());
                }

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.Size))
                {
                    reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.Size], AdWidth.ToString() + "x" + AdHeight.ToString());
                }

#if DEBUG
                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.ShowErrors))
                {
                    reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.ShowErrors], "true");
                }
#endif
                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.AdSuccessEvent))
                {
                    WireUpDelegateEvent(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.AdSuccessEvent], string.Format("Ads served for: {0}", _settings.CurrentAdType.ToString()));
                }

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.AdFailedEvent))
                {
                    WireUpDelegateEvent(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.AdFailedEvent], string.Format("Ad failed request for: {0}", _settings.CurrentAdType.ToString()));
                }

                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.AdClickedEvent))
                {
                    WireUpDelegateEvent(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.AdClickedEvent], string.Format("Ad clicked for: {0}", _settings.CurrentAdType.ToString()));
                } 
                
                if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.StartMethod))
                {
                    reflectionHelper.TryInvokeMethod(providerType, instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.StartMethod]);
                }

            }
            catch (PlatformNotSupportedException)
            {
                OnLog(string.Format("Configured provider {0} not found in this installation", adProvider.AdProviderType.ToString()));
                AdFailed(adProvider.AdProviderType);
                return null;
            }
            catch (NotImplementedException)
            {
                OnLog(string.Format("Configured provider {0} is not fully implemented yet", adProvider.AdProviderType.ToString()));
                AdFailed(adProvider.AdProviderType);
                return null;
            }
            catch (Exception e)
            {
                OnLog(string.Format("General exception [{0}] occured, continuing", e.InnerException.ToString()));
                AdFailed(adProvider.AdProviderType);
                return null;
            }

            OnLog(string.Format("Ad created for provider {0}", adProvider.AdProviderType.ToString()));

            return instance;
        }

        public void StartAdTimer()
        {
            TimeSpan delayTime = new TimeSpan(0, 0, 0);
            TimeSpan intervalTime = new TimeSpan(0, 0, 0, 0, adRotatorRefreshInterval * 1000);
            if (adRotatorTimer != null)
            {
                StopAdTimer();
            }
            if (adRotatorRefreshInterval > 0)
            {
                adRotatorTimer = new Timer(timerDelegate, null, delayTime, intervalTime);
            }
        }

        public void StopAdTimer()
        {
            adRotatorTimer.Dispose();
            adRotatorTimer = null;
        }
        /// <summary>
        /// Called when all attempts to get ads have failed and to disable the control
        /// </summary>
        /// <returns></returns>
        internal string AdsFailed()
        {
            if (IsAdRotatorEnabled)
            {
                OnLog(string.Format("No Ads available"));
                this.IsAdRotatorEnabled = false;
            }
            return "All attempts failed to get ads, disabling";
        }

        #region AdSettings File retrieval

        /// <summary>
        /// Loads the ad settings object either from isolated storage or from the resource path defined in DefaultSettingsFileUri.
        /// </summary>
        /// <returns></returns>
        private async Task LoadAdSettings()
        {

            //If not checked remote && network available - get remote
            if (!String.IsNullOrEmpty(RemoteSettingsLocation))
            {
                try
                {
                    await LoadSettingsFileRemote(RemoteSettingsLocation);
                }
                catch { }
            }

            if (_settings == null)
            {
                await LoadSettingsFileLocal();
                if (_settings == null)
                {
                    try
                    {
                        await LoadSettingsFileProject();
                    }
                    catch { }
                }
            }
        }

        public async Task LoadSettingsFileLocal()
        {
            // if successful set and invalidate
            // If not loadSettings again
            var SETTINGS_FILE_NAME = string.IsNullOrEmpty(LocalSettingsLocation) ? GlobalConfig.DEFAULT_SETTINGS_FILE_NAME : LocalSettingsLocation;
            try
            {
                var stream = await fileHelper.OpenStreamAsync(SETTINGS_FILE_NAME);
                if (stream != null) _settings = _settings.Deserialise(stream);
            }
            catch
            {
                throw new FileNotFoundException("Could not locate the local Settings file");
            }
        }

        public async Task LoadSettingsFileRemote(string RemoteSettingsLocation)
        {
            var settings = await Networking.Network.GetStringFromURLAsync(RemoteSettingsLocation);
            if (settings != null) _settings = _settings.Deserialise(settings);
        }

        //Not Finished (SJ)
        //Needs testing (GO)
        public async Task LoadSettingsFileProject()
        {
            if (LocalSettingsLocation != null)
            {
                try
                {
                    using (Stream stream = await fileHelper.OpenStreamAsync(LocalSettingsLocation))
                    {
                        try
                        {
                            if (stream != null) _settings = _settings.Deserialise(stream);
                        }
                        catch { }
                    }
                }
                catch
                {
                    throw new FileNotFoundException(string.Format("The ad configuration file {0} could not be found. Either the path is incorrect or the build type is not set correctly", LocalSettingsLocation));
                }
            }
        }

        /// <summary>
        /// Saves the passed settings file to isolated storage
        /// </summary>
        /// <param name="settings"></param>
        private async void SaveAdSettings(AdSettings settings)
        {
            var SETTINGS_FILE_NAME = string.IsNullOrEmpty(LocalSettingsLocation) ? GlobalConfig.DEFAULT_SETTINGS_FILE_NAME : LocalSettingsLocation;
            try
            {
                using (Stream stream = await fileHelper.OpenStreamAsync(SETTINGS_FILE_NAME))
                {
                    settings.Serialise(stream);
                }
            }
            catch
            {
                throw new FileNotFoundException(string.Format("Could not locate the local Settings file"));
            }
        }

        #endregion

        #region internal Functions

        public void AdFailed(Model.AdType AdType)
        {
            _settings.AdFailed(AdType);
            //OnLog(string.Format("Ads failed request for: {0}", AdType.ToString()));
            GetAd(null);
        }

        public void ClearFailedAds()
        {
            _settings.ClearFailedAds();
            OnLog(string.Format("Failed Ads cleard"));
        }

        private void RemoveAdFromFailedAds(AdType AdType)
        {
            _settings.RemoveAdFromFailedAds(AdType);
            //OnLog(string.Format("Ads failed request for: {0}", AdType.ToString()));
        }


        private void WireUpDelegateEvent(object o, string eventName, string message)
        {
            Delegate handler;
            try
            {
                EventInfo ei = o.GetType().GetEvent(eventName);
                var parameters = ei.EventHandlerType.GetMethod("Invoke").GetParameters();
                switch (parameters.Count())
                {
                    case 2:
                        handler = new Action<object, object>((o1, o2) => DelegateEventHandler(message));
                        break;
                    case 3:
                        handler = new Action<object, object, object>((o1, o2, o3) => DelegateEventHandler(message));
                        break;
                    default:
                        handler = new Action<object>((o1) => DelegateEventHandler(message));
                        break;
                }
                Delegate del = Delegate.CreateDelegate(ei.EventHandlerType, handler.Target, handler.Method);

                ei.AddEventHandler(o, del);
            }
            catch (Exception)
            {
                throw new Exception("Failed to bind events, general failure");
            }
        }

        private void DelegateEventHandler(string message)
        {
            OnLog(message);
            if (message.Contains("failed"))
            {
                AdFailed(_settings.CurrentAdType);
            }
        }

        /// <summary>
        /// Called when the settings have been loaded. Clears all failed ad types and invalidates the control
        /// </summary>
        private void Init()
        {
            ClearFailedAds();
            OnLog(string.Format("Initialising AdRotator"));
        }
        #endregion
    }
}
