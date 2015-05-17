using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviders
{
    public class AdProviderVserv : AdProvider
    {
        public AdProviderVserv()
        {
            this.AdProviderType = AdType.Vserv;

            this.AdProviderConfigValues = new Dictionary<AdProviderConfig.SupportedPlatforms, AdProviderConfig.AdProviderDetails>()
            {
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "com.vserv.windows.ads.wp8", 
                                                ElementName = "com.vserv.windows.ads.wp8.VservAdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"ZoneId"},
                                                        {AdProviderConfigOptions.AdType,"UX"},
                                                        {AdProviderConfigOptions.StartMethod,"LoadAd"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"DidLoadAd"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"FailedToLoadAd"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"DidInteractWithAd"},
                                                        {AdProviderConfigOptions.StopMethod,"StopRefresh"}
                                                 
                                                    }
                                            }
                            },
                              {SupportedPlatforms.Windows81, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "com.vserv.windows.ads.wp81", 
                                                ElementName = "com.vserv.windows.ads.wp81.VservAdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"ZoneId"},
                                                        {AdProviderConfigOptions.AdType,"UX"},
                                                        {AdProviderConfigOptions.StartMethod,"LoadAd"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"DidLoadAd"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"FailedToLoadAd"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"DidInteractWithAd"},
                                                        {AdProviderConfigOptions.StopMethod,"StopRefresh"}
                                                 
                                                    }
                                            }
                            }
            };
        }
    }
}