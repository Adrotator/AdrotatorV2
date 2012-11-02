
namespace AdRotator.Model
{
    public partial class AdGroup : AdSettingsBase
    {
        private AdProvider[] itemsField;

        private int probabilityField;

        private bool probabilityFieldSpecified;

        private bool enabledInTrialOnlyField;

        private bool enabledInTrialOnlyFieldSpecified;

        private int adOrderField;

        private bool adOrderFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AdDuplex", typeof(AdProviderAdDuplex), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("AdMob", typeof(AdProviderAdMob), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("DefaultHouseAd", typeof(AdProviderDefaultHouseAd), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("InnerActive", typeof(AdProviderInnerActive), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("MobFox", typeof(AdProviderMobFox), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("None", typeof(AdProviderNone), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Pubcenter", typeof(AdProviderPubCenter), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Smaato", typeof(AdProviderSmaato), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AdProvider[] Items
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
        public int Probability
        {
            get
            {
                return this.probabilityField;
            }
            set
            {
                this.probabilityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProbabilitySpecified
        {
            get
            {
                return this.probabilityFieldSpecified;
            }
            set
            {
                this.probabilityFieldSpecified = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int AdOrder
        {
            get
            {
                return this.adOrderField;
            }
            set
            {
                this.adOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdOrderSpecified
        {
            get
            {
                return this.adOrderFieldSpecified;
            }
            set
            {
                this.adOrderFieldSpecified = value;
            }
        }
    }

}
