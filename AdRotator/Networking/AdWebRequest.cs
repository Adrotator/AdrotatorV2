using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace AdRotator.Networking
{
    internal class AdWebRequest
    {
        public static void ReadInnerActiveAdResponse(Uri rssUri, Action<List<AdWebResponse>, Exception> callback)
        {
            callback.CheckNotNullThrowException();

            Network.GetStringFromURL(rssUri, (results, ex) =>
            {
                if (ex != null)
                {
                    callback(null, ex);
                    return;
                }
                try
                {
                    results = results.Replace("tns:", "");

                    var xdoc = XDocument.Parse(results);
                    var AdError = xdoc.Root.Attribute("Error").Value;
                    var AdClientID = xdoc.Root.Element("Client").FirstAttribute.Value;
                    List<AdWebResponse> AdWebResponse = (from item in xdoc.Descendants("Ad")
                                                         select new AdWebResponse()
                                                       {
                                                           Error = AdError,
                                                           ClientID = AdClientID,
                                                           Text = item.IfNullEmptyString("Text"),
                                                           URL = item.IfNullEmptyString("URL"),
                                                           ImageURL = item.IfNullEmptyString("Image")
                                                       }).ToList();

                    callback(AdWebResponse, null);
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            });
        }

        public static void ReadMobFoxAdResponse(Uri rssUri, Action<List<AdWebResponse>, Exception> callback)
        {
            callback.CheckNotNullThrowException();

            Network.GetStringFromURL(rssUri, (results, ex) =>
            {
                if (ex != null || results.Contains("<error>") || results.Contains("<request type=\"noAd\">"))
                {
                    if (ex == null)
                    {
                        if (results.Contains("<error>"))
                        {
                            ex = new Exception("Publisher ID incorrect");
                        }
                        else
                        {
                            ex = new Exception("No Ad Served");
                        }
                    }
                    callback(null, ex);
                    return;
                }
                try
                {
                    var xdoc = XDocument.Parse(results);

                    List<AdWebResponse> AdWebResponse = (from item in xdoc.Descendants("request")
                                                         select new AdWebResponse()
                                                         {
                                                             URL = item.IfNullEmptyString("clickurl"),
                                                             ImageURL = string.IsNullOrEmpty(item.IfNullEmptyString("imageurl")) ? item.ifNullHTMLImageSource("htmlString") : item.IfNullEmptyString("imageurl")
                                                         }).ToList();

                    callback(AdWebResponse, null);
                }
                catch (Exception e)
                {

                    callback(null, e);
                }
            });
        }
    }
}
