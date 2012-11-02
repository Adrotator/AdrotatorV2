using System.Xml.Serialization;

namespace AdRotator.Model
{
    public partial class AdSettings
    {

        /// <summary>
        /// String to identify the default culture
        /// </summary>
        public const string DEFAULT_CULTURE = "default"; 
        
        private AdCultureDescriptor[] cultureDescriptorsField;

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
}
