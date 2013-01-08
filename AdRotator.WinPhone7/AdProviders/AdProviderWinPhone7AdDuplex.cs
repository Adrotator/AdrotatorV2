using AdRotator.Model;
using AdRotator.WinPhone7;
using System;
using System.Net;
using System.Windows;
using System.Xml.Serialization;

namespace AdRotator.WindowsPhone7.AdProviders
{
    public class AdProviderWinPhone7AdDuplex : AdProviderWinPhone7
    {
        public AdProviderWinPhone7AdDuplex(IAdProvider adProvider)
            : base(adProvider)
        {
        }

        public override FrameworkElement GetVisualElement()
        {
            var control = new AdDuplex.AdControl();
            control.AppId = this.AppId;
            return control;
        }
    }
}