
using System.Threading.Tasks;
namespace AdRotator
{
    internal interface IAdRotatorProvider
    {
        int AdWidth { get; set; }

        int AdHeight { get; set; }

        bool IsTest { get; set; }

        bool IsInDesignMode { get; }

        string RemoteSettingsLocation { get; set; }

        string LocalSettingsLocation { get; set; }

        bool IsAdRotatorEnabled { get; set; }

        string DefaultHouseAdBody { get; set; }

        bool IsLoaded { get; }

        bool IsInitialised { get; }

        System.Collections.Generic.Dictionary<AdRotator.Model.AdType, System.Type> PlatformAdProviderComponents { get; }

        bool AutoStartAds { get; set; }

        int AdRefreshInterval { get; set; }

        //string GoogleAnalyticsId { get; set; }

        //object GoogleAnalyticsControl { get; set; }

        //string FlurryAnalyticsId { get; set; }

        //object FlurryAnalyticsControl { get; set; }

        //DISCUSS: should we return strings here? Maybe raising events when stuff is loaded would be more sensible (GO)
        Task<string> Invalidate(AdRotator.Model.AdProvider adProvider);
    }
}
