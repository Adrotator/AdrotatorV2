using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviders
{
    public class AdProviderSmaato : AdProvider
    {
        public AdProviderSmaato()
        {
            this.AdProviderType = AdType.Smaato;
            this.AdProviderConfigValues = new Dictionary<AdProviderConfig.SupportedPlatforms, AdProviderConfig.AdProviderDetails>()
            {
                            {SupportedPlatforms.WindowsPhone7, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "SOMAWP7", 
                                                ElementName = "SOMAWP7.SomaAdViewer", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"Adspace"},
                                                        {AdProviderConfigOptions.SecondaryId,"Pub"},
                                                        {AdProviderConfigOptions.StartMethod,"StartAds"},
                                                        {AdProviderConfigOptions.AdWidth,"AdSpaceWidth"},
                                                        {AdProviderConfigOptions.AdHeight,"AdSpaceHeight"},
                                                        {AdProviderConfigOptions.ShowErrors,"ShowErrors"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"NewAdAvailable"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdError"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClick"},
                                                        {AdProviderConfigOptions.StopMethod,"StopAds"}
                                                   }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "SOMAWP8", 
                                                ElementName = "SOMAWP8.SomaAdViewer", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"Adspace"},
                                                        {AdProviderConfigOptions.SecondaryId,"Pub"},
                                                        {AdProviderConfigOptions.StartMethod,"StartAds"},
                                                        {AdProviderConfigOptions.AdWidth,"AdSpaceWidth"},
                                                        {AdProviderConfigOptions.AdHeight,"AdSpaceHeight"},
                                                        {AdProviderConfigOptions.ShowErrors,"ShowErrors"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"NewAdAvailable"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdError"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClick"},
                                                        {AdProviderConfigOptions.StopMethod,"StopAds"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.Windows8, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "SOMAW81", 
                                                ElementName = "SOMAW81.SomaAdViewer", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"Adspace"},
                                                        {AdProviderConfigOptions.SecondaryId,"Pub"},
                                                        {AdProviderConfigOptions.StartMethod,"StartAds"},
                                                        {AdProviderConfigOptions.AdWidth,"AdSpaceWidth"},
                                                        {AdProviderConfigOptions.AdHeight,"AdSpaceHeight"},
                                                        {AdProviderConfigOptions.ShowErrors,"ShowErrors"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"NewAdAvailable"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdError"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClick"},
                                                        {AdProviderConfigOptions.StopMethod,"StopAds"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.Windows81, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "SOMAW81", 
                                                ElementName = "SOMAW81.SomaAdViewer", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"Adspace"},
                                                        {AdProviderConfigOptions.SecondaryId,"Pub"},
                                                        {AdProviderConfigOptions.StartMethod,"StartAds"},
                                                        {AdProviderConfigOptions.AdWidth,"AdSpaceWidth"},
                                                        {AdProviderConfigOptions.AdHeight,"AdSpaceHeight"},
                                                        {AdProviderConfigOptions.ShowErrors,"ShowErrors"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"NewAdAvailable"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdError"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClick"},
                                                        {AdProviderConfigOptions.StopMethod,"StopAds"}
                                                    }
                                            }
                            },
                            {SupportedPlatforms.WindowsPhone81Appx, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "SOMAWP81", 
                                                ElementName = "SOMAWP81.SomaAdViewer", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"Adspace"},
                                                        {AdProviderConfigOptions.SecondaryId,"Pub"},
                                                        {AdProviderConfigOptions.StartMethod,"StartAds"},
                                                        {AdProviderConfigOptions.AdWidth,"AdSpaceWidth"},
                                                        {AdProviderConfigOptions.AdHeight,"AdSpaceHeight"},
                                                        {AdProviderConfigOptions.ShowErrors,"ShowErrors"},
                                                        {AdProviderConfigOptions.AdSuccessEvent,"NewAdAvailable"},
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdError"},
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClick"},
                                                        {AdProviderConfigOptions.StopMethod,"StopAds"}
                                                   }
                                            }
                            },
            };
        }
    }
}