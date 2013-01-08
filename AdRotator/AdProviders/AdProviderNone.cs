using AdRotator.Model;

namespace AdRotator.AdProviders
{
    public class AdProviderNone : AdProvider
    {
        public AdProviderNone()
        {
            this.AdProviderType = AdType.None;
        }
    }
}