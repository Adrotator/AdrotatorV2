using System.Collections.Generic;

namespace AdRotator.AdProviderConfig
{
        internal struct AdProviderDetails
        {
            public string AssemblyName;
            public string ElementName;
            public bool RequiresParameters;
            public Dictionary<AdProviderConfigOptions, string> ConfigurationOptions;
        }

        internal enum AdProviderConfigOptions
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
            Size,
            ReloadTime,
            ShowErrors,
            AdSuccessEvent,
            AdFailedEvent,
            AdClickedEvent,
            StopMethod
        }

        internal enum SupportedPlatforms
        {
            WindowsPhone7 = 0,
            WindowsPhone8 = 1,
            Windows8 = 2,
            Windows81 = 3,
            WindowsPhone81Appx = 4
        }

}
