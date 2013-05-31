using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator
{

    public static class AdProviderConfig
    {
        public struct AdProviderDetails
        {
            public string AssemblyName;
            public string ElementName;
            public bool RequiresParameters;
            public Dictionary<AdProviderConfigOptions, string> ConfigurationOptions;
        }

        public enum AdProviderConfigOptions
        {
            AppId,
            SetAppId,
            SecondaryId,
            SetSecondaryId,
            IsTest,
            AdType,
            StartMethod,
            AdWidth,
            AdHeight,
            ReloadTime,
            ShowErrors,
            AdSuccessEvent,
            AdFailedEvent,
            AdClickedEvent
        }

        public enum SupportedPlatforms
        {
            WindowsPhone7 = 0,
            WindowsPhone8 = 1,
            Windows8 = 2
        }

        /// <remarks/>
        /// When adding a new provider ensure you set and test all values nessasary below
        /// * Assembly names are case sensitive - right look at the defnition for the AdProviders DISPLAY control type for the assembly name, without the .DLL extension
        /// * ElementName is the full class name (including namespace) of the display element for the AdProvider, !Note some providers use a hosting class and a display function
        /// * Add any property values required by the provider
        /// *WIP - (SJ)not yet added capability for provider only options ot events yet
        #region Platform Config

        #region Windows Phone 7
        private static Dictionary<AdType, AdProviderDetails> AdProviderConfigValuesWP7 = new Dictionary<AdType, AdProviderDetails>()
        {
            {AdType.AdDuplex, new AdProviderDetails() 
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
            {AdType.PubCenter, new AdProviderDetails() 
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
            {AdType.Smaato, new AdProviderDetails() 
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
                                                        {AdProviderConfigOptions.AdClickedEvent,"AdClick"}
                                                    }
                                            }
            },
            {AdType.InnerActive, new AdProviderDetails() 
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
            {AdType.MobFox, new AdProviderDetails() 
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
            {AdType.AdMob, new AdProviderDetails() 
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
            {AdType.Inmobi, new AdProviderDetails() 
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
                                                        {AdProviderConfigOptions.AdFailedEvent,"AdRequestFailed"}
                                                    }
                                            }
            },
        };
        #endregion
        #region Windows Phone 8
        private static Dictionary<AdType, AdProviderDetails> AdProviderConfigValuesWP8 = new Dictionary<AdType, AdProviderDetails>()
        {
            {AdType.AdDuplex, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "AdDuplex.WindowsPhone", 
                                                ElementName = "AdDuplex.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.IsTest,"IsTest"}
                                                    }
                                            }
            },
            {AdType.PubCenter, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Microsoft.Advertising.Mobile.UI", 
                                                ElementName = "Microsoft.Advertising.Mobile.UI.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"ApplicationId"},
                                                        {AdProviderConfigOptions.SecondaryId,"AdUnitId"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"}
                                                    }
                                            }
            },
            {AdType.Smaato, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "SOMAWP8", 
                                                ElementName = "SOMAWP8.SomaAdViewer", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"Adspace"},
                                                        {AdProviderConfigOptions.SecondaryId,"Pub"},
                                                        {AdProviderConfigOptions.StartMethod,"StartAds"}
                                                    }
                                            }
            },
            {AdType.InnerActive, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Inneractive.Ad", 
                                                ElementName = "Inneractive.Ad.InneractiveAd", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppID"},
                                                        {AdProviderConfigOptions.AdType,"AdType"},
                                                        {AdProviderConfigOptions.ReloadTime,"ReloadTime"}
                                                    }
                                            }
            },
            {AdType.MobFox, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "MobFox.Ads", 
                                                ElementName = "MobFox.Ads.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"PublisherID"},
                                                        {AdProviderConfigOptions.IsTest,"TestMode"},
                                                        {AdProviderConfigOptions.StartMethod,"RequestNextAd"}
                                                    }
                                            }
            },
            {AdType.AdMob, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Google.AdMob.Ads.WindowsPhone7", 
                                                ElementName = "Google.AdMob.Ads.WindowsPhone7.WPF.BannerAd", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AdUnitID"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"}
                                                    }
                                            }
            },
            {AdType.Inmobi, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "InMobiWPAdSDK", 
                                                ElementName = "InMobi.WP.AdSDK.IMAdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.ReloadTime,"RefreshInterval"},
                                                        {AdProviderConfigOptions.StartMethod,"LoadNewAd"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"}
                                                    }
                                            }
            },
        };
        #endregion
        #region Windows 8
        private static Dictionary<AdType, AdProviderDetails> AdProviderConfigValuesWin8 = new Dictionary<AdType, AdProviderDetails>()
        {
            {AdType.AdDuplex, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "AdDuplex.Windows", 
                                                ElementName = "AdDuplex.Controls.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.IsTest,"IsTest"}
                                                    }
                                            }
            },
            {AdType.PubCenter, new AdProviderDetails() 
                                            { 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"ApplicationId"},
                                                        {AdProviderConfigOptions.SecondaryId,"AdUnitId"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"}
                                                    }
                                            }
            },
            {AdType.MobFox, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "MobFox.Ads", 
                                                ElementName = "MobFox.Ads.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"PublisherID"},
                                                        {AdProviderConfigOptions.IsTest,"TestMode"},
                                                        {AdProviderConfigOptions.StartMethod,"RequestNextAd"}
                                                    }
                                            }
            },
            {AdType.AdMob, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Google.AdMob.Ads.WindowsPhone7", 
                                                ElementName = "Google.AdMob.Ads.WindowsPhone7.WPF.BannerAd", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AdUnitID"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"}
                                                    }
                                            }
            },
            {AdType.Inmobi, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "InMobiW8AdSDK", 
                                                ElementName = "InMobi.W8.AdSDK.IMAdView", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,"AppId"},
                                                        {AdProviderConfigOptions.ReloadTime,"RefreshInterval"},
                                                        {AdProviderConfigOptions.StartMethod,"LoadNewAd"},
                                                        {AdProviderConfigOptions.AdWidth,"Width"},
                                                        {AdProviderConfigOptions.AdHeight,"Height"}
                                                    }
                                            }
            },
        };
        #endregion

        #endregion

        /// <remarks/>
        /// TO add a new platform, define it's config above (be nice and region it) then increase the array size below and add it
        public static Dictionary<AdType, AdProviderDetails>[] AdProviderConfigValues = new Dictionary<AdType, AdProviderDetails>[3]
        {
            AdProviderConfig.AdProviderConfigValuesWP7,
            AdProviderConfig.AdProviderConfigValuesWP8,
            AdProviderConfig.AdProviderConfigValuesWin8
        };

    }
}
