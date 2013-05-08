using AdRotator.AdProviders;
using AdRotator.Model;
using AdRotator.WindowsPhone7.AdProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AdRotator.WinPhone7
{
    public abstract class AdProviderWinPhone7 : AdProvider, IAdProviderWinPhone7
    {
        protected IAdProvider adProvider;

        public AdProviderWinPhone7(IAdProvider adProvider){
            this.Populate(adProvider);
        }

        public abstract FrameworkElement GetVisualElement();

        public static IAdProviderWinPhone7 CreateWinPhone7AdProvider(IAdProvider adProvider)
        {
            if (adProvider is AdProviderAdDuplex)
            {
                return new AdProviderWinPhone7AdDuplex(adProvider);
            }
            return null;
        }
    }
}
