using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdRotator.Model;

namespace AdRotator.Interface
{
    public interface IAdRotatorProvider
    {
        bool IsInDesignMode { get; }

        string RemoteSettingsLocation { get; set; }

        string LocalSettingsLocation { get; set; }

        bool IsNetworkEnabled { get; set; }

        bool IsLoaded { get; }

        bool IsInitialised { get; }

        bool IsAdRotatorEnabled { get; set; }

        object DefaultHouseAdBody { get; set; }

        AdSettings LoadSettingsFileLocal();
        AdSettings LoadSettingsFileRemote();
        AdSettings LoadSettingsFileProject();

        string Invalidate();
    }
}
