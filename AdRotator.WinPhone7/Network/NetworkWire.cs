using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace AdRotator.WinPhone7.Networking
{
    public class Network
    {
        private static string CurrentIP;
        const string IPValidatorHost = "http://compiledexperience.com/windows-phone-7/my-ip";


        public async static Task<string> GetStringFromURL(string uri)
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

        internal async static Task <string> GetDeviceIP()
        {
            if (CurrentIP != null && !string.IsNullOrEmpty(CurrentIP))
            {
                return CurrentIP;
            }
            try
            {
                var IP = await GetStringFromURL(IPValidatorHost);
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
    }
}
