using System.Collections.Generic;

namespace AdRotator
{

    public static class AdProviderConfig
    {
        public enum AdProviderConfigOptions
        {
            AppId,
            SecondaryId,
            IsTest,
        }

        public enum SupportedAdProviders
        {
            AdDuplex,
            PubCenter,
            Smaato,
        }

        public static Dictionary<SupportedAdProviders, Dictionary<AdProviderConfigOptions, string>> AdProviderConfigValues = new Dictionary<SupportedAdProviders, Dictionary<AdProviderConfigOptions, string>>()
        {
            {SupportedAdProviders.AdDuplex, new Dictionary<AdProviderConfigOptions,string>() 
                                                { 
                                                    {AdProviderConfigOptions.AppId,""},
                                                    {AdProviderConfigOptions.IsTest,""}
                                                }
            },
            {SupportedAdProviders.PubCenter, new Dictionary<AdProviderConfigOptions,string>() 
                                                { 
                                                    {AdProviderConfigOptions.AppId,""},
                                                    {AdProviderConfigOptions.SecondaryId,""},
                                                    {AdProviderConfigOptions.IsTest,""}
                                                }
            },
            {SupportedAdProviders.Smaato, new Dictionary<AdProviderConfigOptions,string>() 
                                                { 
                                                    {AdProviderConfigOptions.AppId,""},
                                                    {AdProviderConfigOptions.SecondaryId,""}
                                                }
            },
        };
    }
}
