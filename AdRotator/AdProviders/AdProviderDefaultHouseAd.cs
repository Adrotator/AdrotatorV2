using AdRotator.AdProviderConfig;
using AdRotator.Model;
using System.Collections.Generic;


namespace AdRotator.AdProviders
{
    public class AdProviderDefaultHouseAd : AdProvider
    {
        public AdProviderDefaultHouseAd()
        {
            this.AdProviderType = AdType.DefaultHouseAd;
        }
    }
}