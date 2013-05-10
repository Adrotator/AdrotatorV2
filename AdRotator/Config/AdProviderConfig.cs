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
            public Dictionary<AdProviderConfigOptions, string> ConfigurationOptions;
        }

        public enum AdProviderConfigOptions
        {
            AppId,
            SecondaryId,
            IsTest,
        }

        public enum SupportedPlatforms
        {
            WindowsPhone7 = 0,
            WindowsPhone8 = 1,
            Windows8 = 2
        }

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
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.IsTest,""}
                                                    }
                                            }
            },
            {AdType.PubCenter, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Microsoft.Advertising.Mobile.UI", 
                                                ElementName = "Microsoft.Advertising.Mobile.UI.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.SecondaryId,""},
                                                        {AdProviderConfigOptions.IsTest,""}
                                                    }
                                            }
            },
            {AdType.Smaato, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "SOMAWP7", 
                                                ElementName = "SOMAWP7.SomaAd", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.SecondaryId,""}
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
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.IsTest,""}
                                                    }
                                            }
            },
            {AdType.PubCenter, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Microsoft.Advertising.Mobile.UI", 
                                                ElementName = "Microsoft.Advertising.Mobile.UI.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.SecondaryId,""},
                                                        {AdProviderConfigOptions.IsTest,""}
                                                    }
                                            }
            },
            {AdType.Smaato, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "SOMAWP8", 
                                                ElementName = "SOMAWP8.SomaAd", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.SecondaryId,""}
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
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.IsTest,""}
                                                    }
                                            }
            },
            {AdType.PubCenter, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "Microsoft.Advertising.WinRT.UI", 
                                                ElementName = "Microsoft.Advertising.WinRT.UI.AdControl", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.SecondaryId,""},
                                                        {AdProviderConfigOptions.IsTest,""}
                                                    }
                                            }
            },
            {AdType.Smaato, new AdProviderDetails() 
                                            { 
                                                AssemblyName = "", 
                                                ElementName = "", 
                                                ConfigurationOptions = new Dictionary<AdProviderConfigOptions,string>() 
                                                    { 
                                                        {AdProviderConfigOptions.AppId,""},
                                                        {AdProviderConfigOptions.SecondaryId,""}
                                                    }
                                            }
            },
        };
        #endregion

        #endregion

        public static Dictionary<AdType, AdProviderDetails>[] AdProviderConfigValues = new Dictionary<AdType, AdProviderDetails>[3]
        {
            AdProviderConfig.AdProviderConfigValuesWP7,
            AdProviderConfig.AdProviderConfigValuesWP8,
            AdProviderConfig.AdProviderConfigValuesWin8
        };

    }
}
