using AdRotator.Model;
using AdRotator.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private FileHelpers fileHelper;
        private ReflectionHelpers reflectionHelper = new ReflectionHelpers();

        internal int AdWidth { get; set; }

        internal int AdHeight { get; set; }

        internal bool IsAdRotatorEnabled { get; set; }

        internal string RemoteSettingsLocation { get; set; }

        internal string LocalSettingsLocation { get; set; }

        internal object DefaultHouseAdBody { get; set; }

        internal bool isLoaded { get; set; }

        internal bool isInitialised { get; set; }

        internal bool isTest { get; set; }

        internal static List<AdType> PlatformSupportedAdProviders { get; set; }

        internal static Dictionary<AdType,Type> PlatformAdProviderComponents { get; set; }

        #endregion


        //Discuss - can we use calling asembly to run functions in child projects?, that way we can force calling remote functions in the core project. This would controll project flow.
        /// <summary>
        /// AdRotator Initialiser
        /// </summary>
        /// <param name="adSettings">XML string of the AdSettings content</param>
        /// <param name="Culture">Specified culture you want AdRotator initialised for</param>
        public AdRotatorComponent(string Culture, FileHelpers FileHelper)
        {
            this.IsAdRotatorEnabled = true;
            this.culture = Culture;
            this.fileHelper = FileHelper;
            this.AdHeight = 80;
            this.AdWidth = 480;
            PlatformSupportedAdProviders = new List<AdType>();
            PlatformAdProviderComponents = new Dictionary<AdType, Type>();
        }
        public AdRotatorComponent(string Culture, FileHelpers FileHelper, ReflectionHelpers ReflectionHelper)
            : this(Culture, FileHelper)
        {
        }
            

        public async void GetConfig()
        {
            await LoadAdSettings();

            if (_settings != null && _settings.CultureDescriptors.Count() > 0)
            {
                //Set Current culture based on Culture Value
               _settings.GetAdDescriptorBasedOnUICulture(culture);
            }
            OnAdAvailable(_settings.GetAd());
        }

        public void GetAd()
        {
            if (_settings == null)
            {
                GetConfig();
            }
            OnAdAvailable(_settings.GetAd());
        }

        public object GetProviderFrameworkElement(AdRotator.AdProviderConfig.SupportedPlatforms platform, AdProvider adProvider)
        {
            var provider = AdProviderConfig.AdProviderConfigValues[(int)platform][adProvider.AdProviderType];
            Type providerType;
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
            catch (PlatformNotSupportedException e)
            {
                AdFailed(adProvider.AdProviderType);
                throw e;
            }
            if (providerType == null)
            {
                return null;
            }
            try
            {
                //Removed constructor initialising for now, code kept for reference for now,JIC
            //if (provider.RequiresParameters)
            //{
            //    throw new NotImplementedException("Not got constructor initialisation working yet");

            //    var parameterCount = provider.ConfigurationOptions.Count();
            //    object[] parameters = new object[parameterCount];
            //    var ObjConstr = providerType.GetConstructors().FirstOrDefault(constructor => constructor.GetParameters().Count() == parameterCount);
            //    var pInfos = ObjConstr.GetParameters();
            //    for (int i = 0; i < parameterCount; i++)
            //    {
            //        try
            //        {
            //            switch ((AdProviderConfig.AdProviderConfigOptions)Enum.Parse(typeof(AdProviderConfig.AdProviderConfigOptions), pInfos[i].Name.ToString(), true))
            //            {
            //                case AdProviderConfig.AdProviderConfigOptions.AppId:
            //                    parameters[i] = Convert.ChangeType(adProvider.AppId.ToString(), pInfos[i].ParameterType, CultureInfo.InvariantCulture);
            //                    break;
            //                case AdProviderConfig.AdProviderConfigOptions.AdType:
            //                    parameters[i] = StringToEnum(pInfos[i].ParameterType, "IaAdType_Banner");
            //                    break;
            //                case AdProviderConfig.AdProviderConfigOptions.ReloadTime:
            //                    parameters[i] = Convert.ChangeType(20, pInfos[i].ParameterType, CultureInfo.InvariantCulture);
            //                    break;
            //                default:
            //                    break;
            //            }
            //        }
            //        catch
            //        {
            //            parameters[i] = 0;
            //        }
            //    }
            //    //instance = ObjConstr.Invoke(parameters);
            //    instance = Activator.CreateInstance(providerType,parameters);

            //}
            //else
            //{
                instance = Activator.CreateInstance(providerType);
            //}

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

#if DEBUG
            if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.ShowErrors))
            {
                reflectionHelper.TrySetProperty(instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.ShowErrors], "true");
            }
#endif

            if (provider.ConfigurationOptions.ContainsKey(AdProviderConfig.AdProviderConfigOptions.StartMethod))
            {
                reflectionHelper.TryInvokeMethod(providerType, instance, provider.ConfigurationOptions[AdProviderConfig.AdProviderConfigOptions.StartMethod]);
            }
                
                }
            catch (PlatformNotSupportedException)
            {
                OnLog(string.Format("Configured provider {0} not found in this installation", adProvider.AdProviderType.ToString()));
                AdFailed(adProvider.AdProviderType);
                GetAd();
                return null;
            }
            catch (NotImplementedException)
            {
                OnLog(string.Format("Configured provider {0} is not fully implemented yet", adProvider.AdProviderType.ToString()));
                AdFailed(adProvider.AdProviderType);
                GetAd();
                return null;
            }
            catch (Exception e)
            {
                OnLog(string.Format("General exception [{0}] occured, continuing", e.InnerException.ToString()));
                AdFailed(adProvider.AdProviderType);
                GetAd();
                return null;
            }

            OnLog(string.Format("Ad created for provider {0}", adProvider.AdProviderType.ToString()));

            return instance;
        }



        /// <summary>
        /// Called when all attempts to get ads have failed and to disable the control
        /// </summary>
        /// <returns></returns>
        public string AdsFailed()
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
                 await LoadSettingsFileRemote(RemoteSettingsLocation);
            }

            if (_settings == null)
            {
                await LoadSettingsFileLocal();
                if (_settings == null)
                {
                    await LoadSettingsFileProject();
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
                await Task.Factory.StartNew(() =>
                    {
                        if (fileHelper.FileExists(SETTINGS_FILE_NAME))
                        {
                            using (var stream = fileHelper.FileOpenRead("", SETTINGS_FILE_NAME))
                            {
                                _settings = _settings.Deserialise(stream);
                            }
                        }
                    });
            }
            catch
            {
                throw new FileNotFoundException("Could not locate the local Settings file");
            }
        }

        public async Task LoadSettingsFileRemote(string RemoteSettingsLocation)
        {
            var settings = await Networking.Network.GetStringFromURLAsync(RemoteSettingsLocation);
            _settings = _settings.Deserialise(settings);
        }

        //Not Finished (SJ)
        //Needs testing (GO)
        public async Task LoadSettingsFileProject()
        {
            if (LocalSettingsLocation != null)
            {
                try
                {
                    await Task.Factory.StartNew(() =>
                        {
                            using (var stream = fileHelper.FileOpenRead(new Uri(LocalSettingsLocation, UriKind.Relative), ""))
                            {
                                _settings = _settings.Deserialise(stream);
                            }
                        });
                }
                catch
                {
                    throw new FileNotFoundException(string.Format("The ad configuration file {0} could not be found. Either the path is incorrect or the build type is not set to resource", LocalSettingsLocation));
                }
            }
        }

        /// <summary>
        /// Saves the passed settings file to isolated storage
        /// </summary>
        /// <param name="settings"></param>
        private void SaveAdSettings(AdSettings settings)
        {
            var SETTINGS_FILE_NAME = string.IsNullOrEmpty(LocalSettingsLocation) ? GlobalConfig.DEFAULT_SETTINGS_FILE_NAME : LocalSettingsLocation;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(AdSettings));
                using (var stream = fileHelper.OpenStream("", SETTINGS_FILE_NAME, ""))
                {
                    xs.Serialize(stream, settings);
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
            OnLog(string.Format("Ads failed request for: {0}", AdType.ToString()));
        }

        public void ClearFailedAds()
        {
            _settings.ClearFailedAds();
            OnLog(string.Format("Failed Ads cleard"));
        }

        private void RemoveAdFromFailedAds(AdType AdType)
        {
            _settings.RemoveAdFromFailedAds(AdType);
            OnLog(string.Format("Ads failed request for: {0}", AdType.ToString()));
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
