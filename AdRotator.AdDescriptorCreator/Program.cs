using AdRotator.AdProviders;
using AdRotator.Model;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AdRotator.AdDescriptorCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateSettings();
            //var settings = LoadSettings();
            //Console.ReadKey();
        }

        private static AdSettings LoadSettings()
        {
            AdSettings result = null;
            using (var stream = File.Open("defaultAdSettings.xml", FileMode.OpenOrCreate))
            {
                result = (AdSettings)new XmlSerializer(typeof(AdSettings)).Deserialize(stream);
                stream.Close();
            }
            return result;
        }

        private static void CreateSettings()
        {
            var settings = new XmlWriterSettings();
            settings.IndentChars = "\t";
            settings.Indent = true;
            var defaultsettings = createdemodate();
            //var defaultsettings = CreateDefaultAdSettings();
            using (var stream = File.Create("defaultAdSettings.xml"))
            {
                using (var xw = XmlTextWriter.Create(stream, settings))
                {
                    new XmlSerializer(typeof(AdSettings), new System.Type[] { typeof(AdSettingsBase), typeof(bool), typeof(double), typeof(int) }).Serialize(xw, defaultsettings);
                }
                stream.Close();
            }
        }

        public static AdSettings createdemodate()
        {
            var response = new AdSettings()
            {
                CultureDescriptors = new List<AdCultureDescriptor>()
                {
                    new AdCultureDescriptor()
                    {
                        Items = new List<object>()
                        {
                             new AdProviderPubCenter() { Probability=35, AppId = "test_client", SecondaryId="Image480_80" },
                             new AdProviderPubCenter() { Probability=35, AppId = "test_client", SecondaryId="Image480_80" },
                             new AdProviderPubCenter() { Probability=35, AppId = "test_client", SecondaryId="Image480_80" },
                             new AdGroup() 
                             { 
                                 AdOrder = 2,
                                 AdSlideDirection = AdSlideDirection.Top,
                                 Items = new List<AdProvider>()
                                 {
                                     new AdProviderDefaultHouseAd() { AppId = "1", AdOrder= 1 },
                                     new AdProviderDefaultHouseAd() { AppId = "2", AdOrder= 2 },
                                     new AdProviderDefaultHouseAd() { AppId = "3", AdOrder= 3 },
                                 }.ToArray()
                             }
                        }.ToArray(),
                        CultureName = "en-US",
                        AdSlideDirection = AdSlideDirection.Bottom,
                        AdSlideDisplaySeconds = 10,
                        AdSlideHiddenSeconds = 20,
                        AdRefreshSeconds = 10
                    },
                    new AdCultureDescriptor()
                    {
                        Items = new List<object>()
                        {
                             new AdGroup() 
                             { 
                                 AdOrder = 2,
                                 AdSlideDirection = AdSlideDirection.Top,
                                 Items = new List<AdProvider>()
                                 {
                                     new AdProviderDefaultHouseAd() { AppId = "ShowME", AdOrder= 1 },
                                     new AdProviderDefaultHouseAd() { AppId = "ThenShowME", AdOrder= 2 },
                                     new AdProviderDefaultHouseAd() { AppId = "DisplayMeLast", AdOrder= 3 },
                                 }.ToArray()
                             },
                             new AdGroup() 
                             { 
                                 AdOrder = 2,
                                 AdSlideDirection = AdSlideDirection.None,
                                 EnabledInTrialOnly = true,
                                 Items = new List<AdProvider>()
                                 {
                                     new AdProviderPubCenter() { Probability=35, AppId = "test_client", SecondaryId="Image480_80" },
                                     new AdProviderAdDuplex() { Probability=35, AppId = "0" },
                                     new AdProviderAdMob() { Probability=35, AppId = "0" },
                                     new AdProviderSmaato() { Probability=35, AppId = "0" },
                                     new AdProviderPubCenter() { Probability=35, AppId = "test_client", SecondaryId="Image80_80" },
                                     new AdProviderVserv() { Probability=35, AppId = "20846" }, //vserv test banner id is 20846
                                 }.ToArray()
                             }
                   
                        }.ToArray(),
                        CultureName = "Default"
                    },
                }.ToArray()
            };
            return response;
        }


    }
}
