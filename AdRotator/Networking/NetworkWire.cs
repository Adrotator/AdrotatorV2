using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AdRotator.Extensions;

namespace AdRotator.Networking
{
    internal class Network : INetworkWire
    {
        private static string CurrentIP;
        const string IPValidatorHost = "http://compiledexperience.com/windows-phone-7/my-ip";

   
        public static void GetStringFromURL(Uri uri, Action<string, Exception> callback)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(uri);
            request.BeginGetResponse(r =>
            {
                try
                {
                    var httpRequest = (HttpWebRequest)r.AsyncState;
                    var httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(r);
                    using (StreamReader streamReader1 =
                      new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string resultString = streamReader1.ReadToEnd();
                        callback(resultString, null);
                    }
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            }, request);
        }

        internal static void GetDeviceIP(Action<string, Exception> callback)
        {
            if (CurrentIP != null && !string.IsNullOrEmpty(CurrentIP))
            {
                callback(CurrentIP, null);
            }

            var request = (HttpWebRequest)HttpWebRequest.Create(new Uri(IPValidatorHost));
            request.BeginGetResponse(r =>
            {
                try
                {
                    var httpRequest = (HttpWebRequest)r.AsyncState;
                    var httpResponse = (HttpWebResponse)httpRequest.EndGetResponse(r);
                    using (StreamReader streamReader1 =
                      new StreamReader(httpResponse.GetResponseStream()))
                    {
                        string resultString = streamReader1.ReadToEnd();
                        var value = (resultString).Split(new Char[] { '"' });
                        if (value.Length > 2)
                        {
                            var iPValue = (value[3]).Split(new Char[] { '.' });
                            if (iPValue.Length == 4)
                            {
                                CurrentIP = value[3];
                                callback(CurrentIP, null);
                                return;
                            }
                        }
                        callback(null, new Exception("Failed to get IP Successfully"));
                    }
                }
                catch (Exception e)
                {
                    callback(null, e);
                }
            }, request);

        }

        public async static Task<string> GetStringFromURLAsync(string uri)
        {
            try
            {
                var httpResponse = await HttpWebRequest.Create(uri).GetResponseAsync();
                using (StreamReader streamReader1 =
                         new StreamReader(httpResponse.GetResponseStream()))
                {
                    string resultString = streamReader1.ReadToEnd();
                    return resultString;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        internal async static Task<string> GetDeviceIPAsync()
        {
            if (CurrentIP != null && !string.IsNullOrEmpty(CurrentIP))
            {
                return CurrentIP;
            }
            try
            {
                var IP = await GetStringFromURLAsync(IPValidatorHost);
                var value = IP.Split(new Char[] { '"' });
                if (value.Length > 2)
                {
                    var iPValue = (value[3]).Split(new Char[] { '.' });
                    if (iPValue.Length == 4)
                    {
                        CurrentIP = value[3];
                        return CurrentIP;
                    }
                }
                //IP Unresolved
                return string.Empty;
            }
            catch
            {
                //error occured trying to resolve IP
                return string.Empty;
            }
        }


        public string GetStringFromURL(string URL)
        {
            throw new NotImplementedException();
        }

        public string GetDeviceIP()
        {
            throw new NotImplementedException();
        }
    }
}
