using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdRotator.Utilities
{
    public interface IFileHelpers
    {
        Stream OpenStream(string rootDirectory, string assetName, string extension);

        Stream FileOpen(string filePath, string fileMode, string fileAccess, string fileShare);

        Stream FileOpenRead(string Location, string safeName);

        Stream FileOpenRead(Uri Location, string safeName);

        Stream FileOpenRead(object storageFile, string Location, string safeName);

        bool FileExists(string fileName);
    }
}
