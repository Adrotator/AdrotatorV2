using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace AdRotator.Model
{
    public partial class AdSettings
    {

        /// <summary>
        /// Current working Culture
        /// </summary>
        internal AdCultureDescriptor CurrentCulture = null;

        internal AdType CurrentAdType = AdType.None;
        
        private AdCultureDescriptor[] cultureDescriptorsField;

        /// <summary>
        /// List of the ad types that have failed to load
        /// </summary>
        internal List<AdType> _failedAdTypes = new List<AdType>();

        internal int CurrentAdOrderIndex = 0;
        internal AdProvider CurrentAdProvider;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CultureDescriptors", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AdCultureDescriptor[] CultureDescriptors
        {
            get
            {
                return this.cultureDescriptorsField;
            }
            set
            {
                this.cultureDescriptorsField = value;
            }
        }




    }

    internal static class AdSettingsExtensions
    {
        internal static void Serialise(this AdSettings adsettings, Stream output)
        {
            XmlSerializer xs = new XmlSerializer(typeof(AdSettings));
            try
            {
                xs.Serialize(output, adsettings);
            }
            catch
            {
                throw new XmlException("Config file was not in the expected format or not found");
            }
        }

        //internal static void Serialise(this AdSettings adsettings)
        //{
        //    string output;
        //    XmlSerializer xs = new XmlSerializer(typeof(AdSettings));
        //    try
        //    {
        //        using (TextWriter stringWriter = new StringWriter())
        //        {
        //            xs.Serialize(stringWriter,adsettings);
        //            output = stringWriter.
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw new XmlException("Unable to save AdSettings", Ex.InnerException);
        //    }
        //    return adsettings;
        //}

        internal static AdSettings Deserialise(this AdSettings adsettings, Stream input)
        {
            XmlSerializer xs = new XmlSerializer(typeof(AdSettings));
            try
            {
                adsettings = (AdSettings)xs.Deserialize(input);
            }
            catch
            {
                throw new XmlException("Config file was not in the expected format or not found");
            }
            return adsettings;
        }

        internal static AdSettings Deserialise(this AdSettings adsettings, string input)
        {
            XmlSerializer xs = new XmlSerializer(typeof(AdSettings));
            try
            {
                using (TextReader stringReader = new StringReader(input))
                {
                    adsettings = (AdSettings)xs.Deserialize(stringReader);
                }
            }
            catch (Exception Ex)
            {
                throw new XmlException("Unable to unpack AdSettings", Ex.InnerException);
            }
            return adsettings;
        }

        internal static void GetAdDescriptorBasedOnUICulture(this AdSettings adsettings, string culture)
        {
            if (String.IsNullOrEmpty(culture))
            {
                culture = GlobalConfig.DEFAULT_CULTURE;
            }
            var cultureShortName = culture.Substring(0, 2);
            var descriptor = adsettings.CultureDescriptors.Where(x => x.CultureName == culture).FirstOrDefault();
            if (descriptor != null)
            {
                adsettings.CurrentCulture = descriptor;
                return;
            }
            var sameLanguageDescriptor = adsettings.CultureDescriptors.Where(x => x.CultureName.StartsWith(cultureShortName)).FirstOrDefault();
            if (sameLanguageDescriptor != null)
            {
                adsettings.CurrentCulture = sameLanguageDescriptor;
                return;
            }
            var defaultDescriptor = adsettings.CultureDescriptors.Where(x => x.CultureName.ToLower() == GlobalConfig.DEFAULT_CULTURE || string.IsNullOrEmpty(x.CultureName)).FirstOrDefault();
            if (defaultDescriptor != null)
            {
                adsettings.CurrentCulture = defaultDescriptor;
                return;
            }
        }

        internal static AdProvider GetAd(this AdSettings adsettings, AdMode mode)
        {
            //Need to handle Groups and Order

            if (adsettings == null || adsettings.CurrentCulture == null)
            {
                return new AdRotator.AdProviders.AdProviderNone();
            }

            var validDescriptors = adsettings.CurrentCulture.Items
            .Where(x => !adsettings._failedAdTypes.Contains(((AdProvider)x).AdProviderType)
                        && AdRotatorComponent.PlatformSupportedAdProviders.Contains(((AdProvider)x).AdProviderType)
                        && (((AdProvider)x).Probability > 0) || ((AdProvider)x).AdOrder > 0).Cast<AdProvider>().ToArray();

            var defaultHouseAd = (AdProvider)adsettings.CurrentCulture.Items.FirstOrDefault(x => ((AdProvider)x).AdProviderType == AdType.DefaultHouseAd && !adsettings._failedAdTypes.Contains(AdType.DefaultHouseAd));

            if (validDescriptors != null && validDescriptors.Length > 0)
            {
                switch (mode)
                {
                    case AdMode.Random:
                        validDescriptors = RandomPermutation<AdProvider>(validDescriptors);

                        var totalValueBetweenValidAds = validDescriptors.Sum(x => ((AdProvider)x).Probability);
                        var randomValue = AdRotator.AdRotatorComponent._rnd.NextDouble() * totalValueBetweenValidAds;
                        double totalCounter = 0;
                        foreach (AdProvider probabilityDescriptor in validDescriptors)
                        {
                            totalCounter += probabilityDescriptor.Probability;
                            if (randomValue < totalCounter)
                            {
                                adsettings.CurrentAdType = probabilityDescriptor.AdProviderType;
                                return probabilityDescriptor;
                            }
                        }
                        break;
                    case AdMode.Stepped:
                    case AdMode.Ordered:
                        validDescriptors = validDescriptors.OrderBy(x => x.AdOrder).Cast<AdProvider>().ToArray();
                        if (mode == AdMode.Ordered) return validDescriptors[0];
                        adsettings.CurrentAdProvider = validDescriptors[adsettings.CurrentAdOrderIndex];
                        adsettings.CurrentAdOrderIndex++;
                        if (adsettings.CurrentAdOrderIndex > validDescriptors.Length - 1)
                        {
                            adsettings.CurrentAdOrderIndex = 0;
                        }
                        return adsettings.CurrentAdProvider;
                }
                
            }

            if (defaultHouseAd != null)
            {
                adsettings.CurrentAdType = AdType.DefaultHouseAd;
                return defaultHouseAd;
            }

            return new AdProviders.AdProviderNone();
        }


        internal static void AdFailed(this AdSettings adsettings, Model.AdType AdType)
        {
            if (!adsettings._failedAdTypes.Contains(AdType))
            {
                adsettings._failedAdTypes.Add(AdType);
            }
        }

        internal static void ClearFailedAds(this AdSettings adsettings)
        {
            adsettings._failedAdTypes.Clear();
        }

        internal static void RemoveAdFromFailedAds(this AdSettings adsettings, AdType adType)
        {
            if (adsettings._failedAdTypes.Contains(adType))
            {
                adsettings._failedAdTypes.Remove(adType);
            }
        }

        internal static IEnumerable<T> RandomPermutation<T>(IEnumerable<T> array)
        {

            T[] retArray = array.ToArray();

            for (int i = 0; i < array.Count(); i += 1)
            {
                int swapIndex = AdRotator.AdRotatorComponent._rnd.Next(i, array.Count());
                if (swapIndex != i)
                {
                    T temp = retArray[i];
                    retArray[i] = retArray[swapIndex];
                    retArray[swapIndex] = temp;
                }
            }

            return retArray;
        }

        public static T[] RandomPermutation<T>(T[] array)
        {

            T[] retArray = new T[array.Length];
            array.CopyTo(retArray, 0);

            for (int i = 0; i < array.Length; i += 1)
            {
                int swapIndex = AdRotator.AdRotatorComponent._rnd.Next(i, array.Length);
                if (swapIndex != i)
                {
                    T temp = retArray[i];
                    retArray[i] = retArray[swapIndex];
                    retArray[swapIndex] = temp;
                }
            }

            return retArray;
        }
        
    }
}
