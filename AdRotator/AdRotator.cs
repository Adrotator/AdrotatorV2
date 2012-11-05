using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdRotator.Interface;
using AdRotator.Model;

namespace AdRotator
{
    /// <summary>
    /// *Notes (food for thought)
    ///     - Ad Validity checking?
    /// </summary>
    public class AdRotator : IAdRotator
    {
        /// <summary>
        /// Random generator
        /// </summary>
        private static Random _rnd = new Random();

        /// <summary>
        /// List of the ad types that have failed to load
        /// </summary>
        private static List<AdType> _failedAdTypes = new List<AdType>();

        /// <summary>
        /// Current working Culture
        /// </summary>
        private AdCultureDescriptor CurrentCulture = null;

        /// <summary>
        /// State of the current Ad Display order (of provided)
        /// </summary>
        private int OrderIndex = 0;


        #region Logging Event Code
        public delegate void LogHandler(string message);
        public event LogHandler Log;

        protected void OnLog(string message)
        {
            if (Log != null)
            {
                Log(message);
            }
        }
        #endregion        #endregion

        /// <summary>
        /// The ad settings based on which the ad descriptor for the current UI culture can be selected
        /// </summary>
        private static AdSettings _settings;



        public AdRotator(AdSettings adSettings, string Culture)
        {
            _settings = adSettings;
            if (_settings != null && _settings.CultureDescriptors.Count() > 0)
            {
                //Set Current culture based on Culture Value
                CurrentCulture = GetAdDescriptorBasedOnUICulture(Culture);               
            }

        }

        public AdSettings GetConfig()
        {
            throw new NotImplementedException();
        }

        public AdProvider GetAd()
        {
            //Need to handle Groups and Order

            var validDescriptors = CurrentCulture.Items
            .Where(x => !_failedAdTypes.Contains(((AdProvider)x).AdProviderType)
                        && ((AdProvider)x).Probability > 0)
            .ToList();

            var totalValueBetweenValidAds = validDescriptors.Sum(x => ((AdProvider)x).Probability);
            var randomValue = _rnd.NextDouble() * totalValueBetweenValidAds;
            double totalCounter = 0;
            foreach (AdProvider probabilityDescriptor in validDescriptors)
            {
                totalCounter += probabilityDescriptor.Probability;
                if (randomValue < totalCounter)
                {
                    return probabilityDescriptor;
                }
            }
            var defaultHouseAd = (AdProvider)validDescriptors.FirstOrDefault(x => ((AdProvider)x).AdProviderType == AdType.DefaultHouseAd && !_failedAdTypes.Contains(AdType.DefaultHouseAd));
            if (defaultHouseAd != null)
            {
                return defaultHouseAd;
            }
            return null;
        }

        public void AdFailed(Model.AdType AdType)
        {
            throw new NotImplementedException();
        }

        public void ClearFailedAds()
        {
            throw new NotImplementedException();
        }


        #region internal Functions

        private AdCultureDescriptor GetAdDescriptorBasedOnUICulture(string culture)
        {
            if (String.IsNullOrEmpty(culture))
            {
                culture = AdSettings.DEFAULT_CULTURE;
            }
            var cultureShortName = culture.Substring(0, 2);
            var descriptor = _settings.CultureDescriptors.Where(x => x.CultureName == culture).FirstOrDefault();
            if (descriptor != null)
            {
                return descriptor;
            }
            var sameLanguageDescriptor = _settings.CultureDescriptors.Where(x => x.CultureName.StartsWith(cultureShortName)).FirstOrDefault();
            if (sameLanguageDescriptor != null)
            {
                return sameLanguageDescriptor;
            }
            var defaultDescriptor = _settings.CultureDescriptors.Where(x => x.CultureName == AdSettings.DEFAULT_CULTURE).FirstOrDefault();
            if (defaultDescriptor != null)
            {
                return defaultDescriptor;
            }
            return null;
        }

        private void RemoveAdFromFailedAds(AdType adType)
        {
            if (_failedAdTypes.Contains(adType))
            {
                _failedAdTypes.Remove(adType);
            }
        }

        /// <summary>
        /// Called when the settings have been loaded. Clears all failed ad types and invalidates the control
        /// </summary>
        private void Init()
        {
            _failedAdTypes.Clear();
        }
        #endregion
    }
}
