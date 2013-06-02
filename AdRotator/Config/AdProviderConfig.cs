using AdRotator.Model;
using System.Collections.Generic;

namespace AdRotator.AdProviderConfig
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

}
