using System.IO;
using System.Threading.Tasks;

namespace AdRotator.Utilities
{
    public interface IFileHelpers
    {
        Task<Stream> OpenStreamAsync(string name);

        Task<string> LoadData(string path);

        Task<object> LoadData(string path, System.Type type);

        Task<bool> SaveData(string path, string data);
    }
}
