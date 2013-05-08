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


        public static Dictionary<AdType, AdProviderDetails> AdProviderConfigValues = new Dictionary<AdType, AdProviderDetails>()
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
    }
}
