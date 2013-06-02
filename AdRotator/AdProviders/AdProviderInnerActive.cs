using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviders
{
    public class AdProviderInnerActive : AdProvider
    {
        public AdProviderInnerActive()
        {
            this.AdProviderType = AdType.InnerActive;
            this.AdProviderConfigValues = new Dictionary<AdProviderConfig.SupportedPlatforms, AdProviderConfig.AdProviderDetails>()
            {
                            {SupportedPlatforms.WindowsPhone7, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Inneractive.Ad", 
                                                ElementName = "Inneractive.Ad.InneractiveAd",
                                                RequiresParameters = true,
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppID"},
                                                        {AdProviderConfigOptions.AdType,"AdType"},
                                                        {AdProviderConfigOptions.ReloadTime,"ReloadTime"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdReceived"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdFailed"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClicked"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Inneractive.Ad", 
                                                ElementName = "Inneractive.Ad.InneractiveAd", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppID"},
                                                        {AdProviderConfigOptions.AdType,"AdType"},
                                                        {AdProviderConfigOptions.ReloadTime,"ReloadTime"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdReceived"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdFailed"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClicked"}
                                                    }
                                            }
                            },
            };
        }
    }
}