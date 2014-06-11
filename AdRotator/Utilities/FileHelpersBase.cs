using System;
using System.IO;
using System.Threading.Tasks;

namespace AdRotator.Utilities
{
    internal abstract class FileHelpers : IFileHelpers
    {
        public abstract char notSeparator { get; }
        public abstract char separator { get; }

        public abstract Stream OpenStream(string rootDirectory, string assetName, string extension);

        public abstract Stream FileOpen(string filePath, string fileMode, string fileAccess, string fileShare);

        //public abstract Stream FileOpen(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare);

        public abstract Stream FileOpenRead(string Location, string safeName);

        public abstract Stream FileOpenRead(object storageFile, string Location, string safeName);

        public abstract Stream FileOpenRead(Uri Location, string safeName);

        public abstract Task<Stream> OpenStreamAsync(string name);
    
        public abstract string NormalizeFilePathSeperators(string name);

        public abstract bool FileExists(string fileName);

        public abstract bool FileExists(object storageFile, string fileName);

        public abstract Stream FileCreate(string filePath);

        public abstract Stream FileCreate(object storageFile, string filePath);
    
        public abstract void FileDelete(string filePath);

        public abstract void FileDelete(object storageFile, string filePath);

        public abstract string NormalizeFilename(string fileName, string[] extensions);

        public abstract string[] DirectoryGetFiles(string storagePath);

        public abstract string[] DirectoryGetFiles(object storageFile, string storagePath);
    
        public abstract string[] DirectoryGetFiles(string storagePath, string searchPattern);

        public abstract bool DirectoryExists(string dirPath);

        public abstract bool DirectoryExists(string dirPath, out object storageFile);
        
        public abstract string[] DirectoryGetDirectories(string storagePath);

        public abstract string[] DirectoryGetDirectories(string storagePath, string searchPattern);

        public abstract void DirectoryDelete(string dirPath);

        public abstract string GetInstallPath();

        public abstract void DirectoryCreate(string directory);

        public abstract void DirectoryCreate(string directory, string storagePath);

        public abstract Stream SeekStreamtoStart(Stream stream);

        public abstract Stream SeekStreamtoStart(Stream stream, long StartPos);

        public abstract Stream SeekStreamtoStart(Stream stream, long StartPos, out long pos);

        public abstract void StreamClose(Stream stream);

        public abstract Task<string> LoadData(string path);

        public abstract Task<object> LoadData(string path, Type type);

        public abstract Task<bool> SaveData(string path, string data);

    }
}
