
namespace AdRotator
{
    internal interface INetworkWire
    {
        //Override implementation with async/await methods locally if possible
        string GetStringFromURL(string URL);
        string GetDeviceIP();
        
    }
}
