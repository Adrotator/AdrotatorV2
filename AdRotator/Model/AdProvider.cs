using AdRotator.AdProviders;
using System.Collections.Generic;

namespace AdRotator.Model
{
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderNone))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderDefaultHouseAd))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderVserv))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderSmaato))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderMobFox))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderInnerActive))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderAdDuplex))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderAdMob))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderPubCenter))]
    public abstract partial class AdProvider : AdSettingsBase, IAdProvider
    {
        private AdType adProviderTypeField;

        private string appIdField;

        private string secondaryIdField;

        private int probabilityField;

        private bool probabilityFieldSpecified;

        private bool isTestField;

        private bool isTestFieldSpecified;

        private int adOrderField;

        private bool adOrderFieldSpecified;

        private bool enabledInTrialOnlyField;

        private bool enabledInTrialOnlyFieldSpecified;

        [System.Xml.Serialization.XmlIgnore]
        internal Dictionary<AdRotator.AdProviderConfig.SupportedPlatforms, AdRotator.AdProviderConfig.AdProviderDetails> AdProviderConfigValues;

        [System.Xml.Serialization.XmlIgnore]
        public AdType AdProviderType
        {
            get
            {
                return this.adProviderTypeField;
            }
            set
            {
                this.adProviderTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AppId
        {
            get
            {
                return this.appIdField;
            }
            set
            {
                this.appIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SecondaryId
        {
            get
            {
                return this.secondaryIdField;
            }
            set
            {
                this.secondaryIdField = value;
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
        public bool IsTest
        {
            get
            {
                return this.isTestField;
            }
            set
            {
                this.isTestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsTestSpecified
        {
            get
            {
                return this.isTestFieldSpecified;
            }
            set
            {
                this.isTestFieldSpecified = value;
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
        
        protected void Populate(IAdProvider adProvider)
        {
            this.AdProviderType = adProvider.AdProviderType;
            this.AppId = adProvider.AppId;
            this.SecondaryId = adProvider.SecondaryId;
            this.Probability = adProvider.Probability;
            this.IsTest = adProvider.IsTest;
            this.AdOrder = adProvider.AdOrder;
        }

    }
}
