using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviders
{
    public class AdProviderPubCenter : AdProvider
    {
        public AdProviderPubCenter()
        {
            this.AdProviderType = AdType.PubCenter;
            this.AdProviderConfigValues = new Dictionary<AdProviderConfig.SupportedPlatforms, AdProviderConfig.AdProviderDetails>()
            {
                            {SupportedPlatforms.WindowsPhone7, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Microsoft.Advertising.Mobile.UI", 
                                                ElementName = "Microsoft.Advertising.Mobile.UI.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"ApplicationId"},
                                                        {AdProviderConfigOptions.SecondaryId,"AdUnitId"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdRefreshed"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"ErrorOccurred"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Microsoft.Advertising.Mobile.UI", 
                                                ElementName = "Microsoft.Advertising.Mobile.UI.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"ApplicationId"},
                                                        {AdProviderConfigOptions.SecondaryId,"AdUnitId"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdRefreshed"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"ErrorOccurred"}
                                                   }
                                            }
                            },
                            {SupportedPlatforms.Windows8, new AdProviderDetails() 
                                            { 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"ApplicationId"},
                                                        {AdProviderConfigOptions.SecondaryId,"AdUnitId"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        //WinRT components cannot dynamically bind events, the rotters :(
                                                        //{AdProviderConfigOptions.AdSuccessEvent,"AdRefreshed"},
                                                        //{AdProviderConfigOptions.AdFailedEvent,"ErrorOccurred"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone81Appx, new AdProviderDetails() 
                                            { 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"ApplicationId"},
                                                        {AdProviderConfigOptions.SecondaryId,"AdUnitId"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"},
                                                        //WinRT components cannot dynamically bind events, the rotters :(
                                                        //{AdProviderConfigOptions.AdSuccessEvent,"AdRefreshed"},
                                                        //{AdProviderConfigOptions.AdFailedEvent,"ErrorOccurred"}
                                                    }
                                            }
                            },
            };
        }
    }
}