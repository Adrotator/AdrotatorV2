
namespace AdRotator.Model
{
    public class AdSettingsBase
    {
        private double widthField;

        private bool widthFieldSpecified;

        private double heightField;

        private bool heightFieldSpecified;

        private AdSlideDirection adSlideDirectionField;

        private bool adSlideDirectionFieldSpecified;

        private double adRefreshSecondsField;

        private bool adRefreshSecondsFieldSpecified;

        private double adSlideDisplaySecondsField;

        private bool adSlideDisplaySecondsFieldSpecified;

        private double adSlideHiddenSecondsField;

        private bool adSlideHiddenSecondsFieldSpecified;

        /// <remarks/>
//        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "Width", Type = typeof(AdCultureDescriptor))]
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double Width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WidthSpecified
        {
            get
            {
                return this.widthFieldSpecified;
            }
            set
            {
                this.widthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double Height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HeightSpecified
        {
            get
            {
                return this.heightFieldSpecified;
            }
            set
            {
                this.heightFieldSpecified = value;
            }
        }

         /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public AdSlideDirection AdSlideDirection
        {
            get
            {
                return this.adSlideDirectionField;
            }
            set
            {
                this.adSlideDirectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdSlideDirectionSpecified
        {
            get
            {
                return this.adSlideDirectionFieldSpecified;
            }
            set
            {
                this.adSlideDirectionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AdRefreshSeconds
        {
            get
            {
                return this.adRefreshSecondsField;
            }
            set
            {
                this.adRefreshSecondsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdRefreshSecondsSpecified
        {
            get
            {
                return this.adRefreshSecondsFieldSpecified;
            }
            set
            {
                this.adRefreshSecondsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AdSlideDisplaySeconds
        {
            get
            {
                return this.adSlideDisplaySecondsField;
            }
            set
            {
                this.adSlideDisplaySecondsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdSlideDisplaySecondsSpecified
        {
            get
            {
                return this.adSlideDisplaySecondsFieldSpecified;
            }
            set
            {
                this.adSlideDisplaySecondsFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public double AdSlideHiddenSeconds
        {
            get
            {
                return this.adSlideHiddenSecondsField;
            }
            set
            {
                this.adSlideHiddenSecondsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdSlideHiddenSecondsSpecified
        {
            get
            {
                return this.adSlideHiddenSecondsFieldSpecified;
            }
            set
            {
                this.adSlideHiddenSecondsFieldSpecified = value;
            }
        }
    }
}
