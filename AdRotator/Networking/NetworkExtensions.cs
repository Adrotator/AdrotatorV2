using System;
using System.Xml.Linq;

namespace AdRotator.Networking
{
    internal static class NetworkExtensions
    {
        public static void CheckNotNullThrowException(this object obj)
        {
            if (obj == null)
                throw new Exception(obj.GetType().ToString() + " cannot be null");
        }
        public static void IfNullDoThis(this object obj, Action act)
        {
            if (obj == null)
                act.Invoke();
        }
        public static void IfNotNullDoThis(this object obj, Action act)
        {
            if (obj != null)
                act.Invoke();
        }

        public static void IfNotNullInvoke(this Action act)
        {
            if (act != null)
                act.Invoke();
        }

        public static string IfNullEmptyString(this XElement item, string key)
        {
            return (item.Element(key) == null) ? string.Empty : item.Element(key).Value;
        }

        public static string IfNullEmptyValue(this XElement item)
        {
            return (item.Value == null) ? string.Empty : item.Value;
        }

        public static string IfNullEmptyAttribute(this XElement item, string attribute)
        {
            return (item.Attribute(attribute) == null) ? string.Empty : item.Attribute(attribute).Value;
        }

        public static string ifNullHTMLImageSource(this XElement item, string key)
        {
            if ((item.Element(key) == null))
            {
                return string.Empty; 
            }
            try
            {
                var HTMLText = item.Element(key).Value;
                var ImgPos = HTMLText.IndexOf("<img id=");
                if (ImgPos < 1)
                {
                    return string.Empty;
                }
                var SrcPos = HTMLText.IndexOf("src=", ImgPos) + 5;
                var endPos = HTMLText.IndexOf('"', SrcPos);
                return HTMLText.Substring(SrcPos, endPos - SrcPos);
            }
            catch (Exception)
            {
                return string.Empty; 
            }

        }
    }
}
