using AdRotator.Model;

namespace AdRotator.Interface
{
    interface IAdRotator
    {
        /// <summary>
        /// Return Current AsSettings object for saving locally in client
        /// </summary>
        /// <returns></returns>
        AdSettings GetConfig();

        /// <summary>
        /// Get AdProvider from config
        /// </summary>
        /// <param name="Culture">Provide current culture</param>
        /// <returns></returns>
        AdProvider GetAd();

        /// <summary>
        /// Report an Ad Type as failed, Not returning Ads
        /// </summary>
        /// <param name="AdType"></param>
        void AdFailed(AdType AdType);

        /// <summary>
        /// Restart Ad Polling, clear old failed Ad Providers
        /// </summary>
        void ClearFailedAds();

        
    }
}
