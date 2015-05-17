using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviders
{
    public class AdProviderInmobi : AdProvider
    {
        public AdProviderInmobi()
        {
            this.AdProviderType = AdType.Inmobi;
            this.AdProviderConfigValues = new Dictionary<AdProviderConfig.SupportedPlatforms, AdProviderConfig.AdProviderDetails>()
            {
                            {SupportedPlatforms.WindowsPhone7, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "InMobiWP7SDK", 
                                                ElementName = "InMobi.WpSdk.IMAdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.ReloadTime,"RefreshInterval"},
                                                        {AdProviderConfigOptions.StartMethod,"LoadNewAd"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdRequestLoaded"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdRequestFailed"},
                                                        {AdProviderConfigOptions.AdType,"AdSize"},
                                                   }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "InMobiWPAdSDK", 
                                                ElementName = "InMobi.WP.AdSDK.IMAdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.ReloadTime,"RefreshInterval"},
                                                        {AdProviderConfigOptions.StartMethod,"LoadNewAd"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"OnAdRequestLoaded"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"OnAdRequestFailed"},
                                                        {AdProviderConfigOptions.AdType,"AdSize"},
                                                    }
                                            }
                            },
                            {SupportedPlatforms.Windows8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "InMobiW8AdSDK", 
                                                ElementName = "InMobi.W8.AdSDK.IMAdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.ReloadTime,"RefreshInterval"},
                                                        {AdProviderConfigOptions.StartMethod,"LoadNewAd"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"OnAdRequestLoaded"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"OnAdRequestFailed"},
                                                        {AdProviderConfigOptions.AdType,"AdSize"},
                                                   }
                                            }
                            },
                            {SupportedPlatforms.Windows81, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "InMobiW8AdSDK", 
                                                ElementName = "InMobi.W8.AdSDK.IMAdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.ReloadTime,"RefreshInterval"},
                                                        {AdProviderConfigOptions.StartMethod,"LoadNewAd"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"OnAdRequestLoaded"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"OnAdRequestFailed"},
                                                        {AdProviderConfigOptions.AdType,"AdSize"},
                                                   }
                                            }
                            },
            };
        }
    }
}
