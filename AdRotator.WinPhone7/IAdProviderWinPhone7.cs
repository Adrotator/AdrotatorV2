using AdRotator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AdRotator.WinPhone7
{
    public interface IAdProviderWinPhone7: IAdProvider
    {
        /// <summary>
        /// Returns the visual element containing the ad itself
        /// </summary>
        /// <returns></returns>
        FrameworkElement GetVisualElement();
    }
}
