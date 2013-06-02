using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviders
{
    public class AdProviderAdMob : AdProvider
    {
        public AdProviderAdMob()
        {
            this.AdProviderType = AdType.AdMob;
            this.AdProviderConfigValues = new Dictionary<AdProviderConfig.SupportedPlatforms, AdProviderConfig.AdProviderDetails>()
            {
                            {SupportedPlatforms.WindowsPhone7, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Google.AdMob.Ads.WindowsPhone7", 
                                                ElementName = "Google.AdMob.Ads.WindowsPhone7.WPF.BannerAd", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AdUnitID"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdReceived"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdFailed"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Google.AdMob.Ads.WindowsPhone7", 
                                                ElementName = "Google.AdMob.Ads.WindowsPhone7.WPF.BannerAd", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AdUnitID"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdReceived"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdFailed"}
                                                    }
                                            }
                            },
            };
        }
    }
}