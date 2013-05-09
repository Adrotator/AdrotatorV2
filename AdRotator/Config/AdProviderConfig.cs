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
            WindowsPhone = 0,
            Windows8 = 1
        }

        #region Platform Config

        #region Windows Phone
        private static Dictionary<AdType, AdProviderDetails> AdProviderConfigValuesWP = new Dictionary<AdType, AdProviderDetails>()
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
                                                AssemblyName = "", 
                                                ElementName = "", 
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
                                                AssemblyName = "", 
                                                ElementName = "", 
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

        public static Dictionary<AdType, AdProviderDetails>[] AdProviderConfigValues = new Dictionary<AdType, AdProviderDetails>[2]
        {
            AdProviderConfig.AdProviderConfigValuesWP,
            AdProviderConfig.AdProviderConfigValuesWin8
        };

    }
}
