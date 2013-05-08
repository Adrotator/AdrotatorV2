
namespace AdRotator
{
    public interface IAdRotatorProvider
    {
        bool IsInDesignMode { get; }

        //string RemoteSettingsLocation { get; set; }

        //string LocalSettingsLocation { get; set; }

        bool IsLoaded { get; }

        bool IsInitialised { get; }

        bool IsAdRotatorEnabled { get; set; }

        object DefaultHouseAdBody { get; set; }

        //string LoadSettingsFileLocal();
        //string LoadSettingsFileRemote(string RemoteSettingsLocation);
        //string LoadSettingsFileProject();

        //DISCUSS: should we return strings here? Maybe raising events when stuff is loaded would be more sensible (GO)
        string Invalidate(AdRotator.Model.AdProvider adProvider);
    }
}
