using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviders
{
    public class AdProviderMobFox : AdProvider
    {
        public AdProviderMobFox()
        {
            this.AdProviderType = AdType.MobFox;
            this.AdProviderConfigValues = new Dictionary<AdProviderConfig.SupportedPlatforms, AdProviderConfig.AdProviderDetails>()
            {
                            {SupportedPlatforms.WindowsPhone7, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "MobFox.Ads", 
                                                ElementName = "MobFox.Ads.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"PublisherID"},
                                                        {AdProviderConfigOptions.IsTest,"TestMode"},
                                                        {AdProviderConfigOptions.StartMethod,"RequestNextAd"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"NewAd"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"NoAd"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "MobFox.Ads", 
                                                ElementName = "MobFox.Ads.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"PublisherID"},
                                                        {AdProviderConfigOptions.IsTest,"TestMode"},
                                                        {AdProviderConfigOptions.StartMethod,"RequestNextAd"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"NewAd"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"NoAd"}
                                                    }
                                            }
                            },
            };
        }
    }
}