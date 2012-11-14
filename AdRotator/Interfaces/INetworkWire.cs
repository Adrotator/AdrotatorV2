using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdRotator
{
    public interface INetworkWire
    {
        //Override implementation with async/await methods locally if possible
        string GetStringFromURL(string URL);
        string GetDeviceIP();
        
    }
}
