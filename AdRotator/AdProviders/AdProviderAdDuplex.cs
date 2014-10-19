using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviders
{
    public class AdProviderAdDuplex : AdProvider
    {
        public AdProviderAdDuplex()
        {
            this.AdProviderType = AdType.AdDuplex;

            this.AdProviderConfigValues = new Dictionary<AdProviderConfig.SupportedPlatforms, AdProviderConfig.AdProviderDetails>()
            {
                            {SupportedPlatforms.WindowsPhone7, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "AdDuplex.WindowsPhone", 
                                                ElementName = "AdDuplex.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.IsTest,"IsTest"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdLoaded"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdLoadingError"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClick"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "AdDuplex.WindowsPhone", 
                                                ElementName = "AdDuplex.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.IsTest,"IsTest"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"AdLoaded"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdLoadingError"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClick"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.Windows8, new AdProviderDetails() 
                                            { 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.IsTest,"IsTest"},
                                                        {AdProviderConfigOptions.Size,"Size"},
                                                        //{AdProviderConfigOptions.AdSuccessEvent,"AdLoaded"},
                                                        //{AdProviderConfigOptions.AdFailedEvent,"AdLoadingError"},
                                                        //{AdProviderConfigOptions.AdClickedEvent,"AdClick"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone81Appx, new AdProviderDetails() 
                                            { 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.IsTest,"IsTest"},
                                                        {AdProviderConfigOptions.Size,"Size"},
                                                        //{AdProviderConfigOptions.AdSuccessEvent,"AdLoaded"},
                                                        //{AdProviderConfigOptions.AdFailedEvent,"AdLoadingError"},
                                                        //{AdProviderConfigOptions.AdClickedEvent,"AdClick"}
                                                    }
                                            }
                            },
            };
        }
    }
}