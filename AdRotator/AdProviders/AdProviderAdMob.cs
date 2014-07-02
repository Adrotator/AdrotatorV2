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
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "GoogleAds", 
                                                ElementName = "GoogleAds.AdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AdUnitID"},
                                                        {AdProviderConfigOptions.AdType,"Format"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"ReceivedAd"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"FailedToReceiveAd"}
                                                    }
                                            }
                            },
            };
        }
    }
}