using AdRotator.AdProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AdRotator.Model
{
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderNone))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdProviderDefaultHouseAd))]
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
