using AdRotator.AdProviders;

namespace AdRotator.Model
{
    public partial class AdCultureDescriptor : AdSettingsBase
    {
        private object[] itemsField;

        private string cultureNameField;

        private AdType defaultAdTypeField;

        private bool defaultAdTypeFieldSpecified;

        private bool enabledInTrialOnlyField;

        private bool enabledInTrialOnlyFieldSpecified;

        /// <remarks/>
        /// To add a new Add provider it needs to be defined in x Places
        /// * Update XSD with new Ad Provider Type and Choice
        /// * Add a new AdType using the Providers Market name (take care with case sensitivity)
        /// * Create new AdProvider class in "AdProviders" folder which inherits the AdProvider Type and sets the correct AdType
        /// * Add an entry below for the deserialisation for the provider !!Case Sensitive
        /// * Copy the new line and add to same place in the AdGroup.cs class if you want that provider to also be supported by AdGroups
        /// * Finally, in the AdProviderConfig.cs add a new provider entry for each of the platforms that provider supports. (see notes there)
        [System.Xml.Serialization.XmlElementAttribute("AdDuplex", typeof(AdProviderAdDuplex), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("AdGroup", typeof(AdGroup), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("AdMob", typeof(AdProviderAdMob), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("DefaultHouseAd", typeof(AdProviderDefaultHouseAd), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("InnerActive", typeof(AdProviderInnerActive), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("MobFox", typeof(AdProviderMobFox), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("None", typeof(AdProviderNone), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("PubCenter", typeof(AdProviderPubCenter), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Smaato", typeof(AdProviderSmaato), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Inmobi", typeof(AdProviderInmobi), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Vserv", typeof(AdProviderVserv), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CultureName
        {
            get
            {
                return this.cultureNameField;
            }
            set
            {
                this.cultureNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public AdType DefaultAdType
        {
            get
            {
                return this.defaultAdTypeField;
            }
            set
            {
                this.defaultAdTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DefaultAdTypeSpecified
        {
            get
            {
                return this.defaultAdTypeFieldSpecified;
            }
            set
            {
                this.defaultAdTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool EnabledInTrialOnly
        {
            get
            {
                return this.enabledInTrialOnlyField;
            }
            set
            {
                this.enabledInTrialOnlyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EnabledInTrialOnlySpecified
        {
            get
            {
                return this.enabledInTrialOnlyFieldSpecified;
            }
            set
            {
                this.enabledInTrialOnlyFieldSpecified = value;
            }
        }

    }
}
