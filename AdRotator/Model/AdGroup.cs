
using AdRotator.AdProviders;
namespace AdRotator.Model
{
    public partial class AdGroup : AdProvider
    {
        private AdProvider[] itemsField;

         /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AdDuplex", typeof(AdProviderAdDuplex), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Vserv", typeof(AdProviderVserv), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("AdMob", typeof(AdProviderAdMob), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("DefaultHouseAd", typeof(AdProviderDefaultHouseAd), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("InnerActive", typeof(AdProviderInnerActive), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("MobFox", typeof(AdProviderMobFox), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("None", typeof(AdProviderNone), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Pubcenter", typeof(AdProviderPubCenter), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Smaato", typeof(AdProviderSmaato), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlElementAttribute("Inmobi", typeof(AdProviderInmobi), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
     }
}
