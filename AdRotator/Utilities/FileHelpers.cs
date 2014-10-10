using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

#if NETFX_CORE || UNIVERSAL
using Windows.Storage;
using Windows.Storage.Search;

#endif
#if WINDOWS_PHONE
using System.IO.IsolatedStorage;
using System.Windows;
#endif

namespace AdRotator
{
	internal class FileHelpers : Utilities.FileHelpers
	{
		#region internal properties
#if WINDOWS_PHONE
		//If no storage file supplied, use the default isolated storage folder
		static IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
#elif NETFX_CORE
        static StorageFolder defaultFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
#elif ANDROID
		// Keep this static so we only call Game.Activity.Assets.List() once
		// No need to call it for each file if the list will never change.
		// We do need one file list per folder though.
		static Dictionary<string, string[]> filesInFolders = new Dictionary<string,string[]>();
#endif
        #endregion

        #region public properties

#if NETFX_CORE || UNIVERSAL
        public override char notSeparator { get { return '/'; } }
		public override char separator { get { return '\\'; } }
#else
        public override char notSeparator { get { return '\\'; } }
		public override char separator { get { return Path.DirectorySeparatorChar; } }
#endif
		#endregion

		#region File Handlers

		public override Stream FileOpen(string filePath, string fileMode, string fileAccess, string fileShare)
		{
#if !NETFX_CORE && !UNIVERSAL
			return FileOpen(filePath, (FileMode)Enum.Parse(typeof(FileMode), fileMode, true), (FileAccess)Enum.Parse(typeof(FileAccess), fileAccess, false), (FileShare)Enum.Parse(typeof(FileShare), fileShare, false));
#else
			throw new NotImplementedException();
#endif
		}

#if !NETFX_CORE && !UNIVERSAL
		public Stream FileOpen(string filePath, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
		{
#if WINDOWS_STORE_APP
			var folder = ApplicationData.Current.LocalFolder;
			if (fileMode == FileMode.Create || fileMode == FileMode.CreateNew)
			{
				return folder.OpenStreamForWriteAsync(filePath, CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();
			}
			else if (fileMode == FileMode.OpenOrCreate)
			{
				if (fileAccess == FileAccess.Read)
					return folder.OpenStreamForReadAsync(filePath).GetAwaiter().GetResult();
				else
				{
					// Not using OpenStreamForReadAsync because the stream position is placed at the end of the file, instead of the beginning
					var f = folder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists).AsTask().GetAwaiter().GetResult();
					return f.OpenAsync(FileAccessMode.ReadWrite).AsTask().GetAwaiter().GetResult().AsStream();
				}
			}
			else if (fileMode == FileMode.Truncate)
			{
				return folder.OpenStreamForWriteAsync(filePath, CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();
			}
			else
			{
				//if (fileMode == FileMode.Append)
				// Not using OpenStreamForReadAsync because the stream position is placed at the end of the file, instead of the beginning
				folder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists).AsTask().GetAwaiter().GetResult().OpenAsync(FileAccessMode.ReadWrite).AsTask().GetAwaiter().GetResult().AsStream();
				var f = folder.CreateFileAsync(filePath, CreationCollisionOption.OpenIfExists).AsTask().GetAwaiter().GetResult();
				return f.OpenAsync(FileAccessMode.ReadWrite).AsTask().GetAwaiter().GetResult().AsStream();
			}
#else
			return File.Open(filePath, fileMode, fileAccess, fileShare);
#endif
		}
#endif
		public override Stream FileOpenRead(string Location, string safeName)
		{
#if NETFX_CORE || UNIVERSAL
			var stream = Task.Run( () => OpenStreamAsync(safeName).Result ).Result;
			if (stream == null)
				throw new FileNotFoundException(safeName);

			return stream;
#elif ANDROID
			return Game.Activity.Assets.Open(safeName);
#elif IOS
			var absolutePath = Path.Combine(Location, safeName);
			if (SupportRetina)
			{
				// Insert the @2x immediately prior to the extension. If this file exists
				// and we are on a Retina device, return this file instead.
				var absolutePath2x = Path.Combine(Path.GetDirectoryName(absolutePath),
												  Path.GetFileNameWithoutExtension(absolutePath)
												  + "@2x" + Path.GetExtension(absolutePath));
				if (File.Exists(absolutePath2x))
					return File.OpenRead(absolutePath2x);
			}
			return File.OpenRead(absolutePath);
#elif WINDOWS_PHONE
			return storage.OpenFile(safeName, FileMode.Open, FileAccess.Read);
#else
			var absolutePath = Path.Combine(Location, safeName);
			return File.OpenRead(absolutePath);
#endif
		}

		public override Stream FileOpenRead(Uri Location, string safeName)
		{
			//Needs Error Handling
			Stream stream = null;
#if !NETFX_CORE && !UNIVERSAL
			try
			{
				var resourceStream = Application.GetResourceStream(Location);
				stream = resourceStream.Stream;
			}
			catch { }

#endif
            return stream;
		}

		public override bool FileExists(string fileName)
		{
#if NETFX_CORE || UNIVERSAL
			var result = Task.Run(async () =>
			{
				try
				{
					var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(fileName);
					return file == null ? false : true;
				}
				catch (FileNotFoundException)
				{
					return false;
				}
			}).Result;

			if (result)
			{
				return true;
			}

#elif ANDROID
			int index = fileName.LastIndexOf(Path.DirectorySeparatorChar);
			string path = string.Empty;
			string file = fileName;
			if (index >= 0)
			{
				file = fileName.Substring(index + 1, fileName.Length - index - 1);
				path = fileName.Substring(0, index);
			}

			// Only read the assets file list once
			string[] files = DirectoryGetFiles(path);

			if (files.Any(s => s.ToLower() == file.ToLower()))
				return true;
#elif WINDOWS_PHONE
			if(storage.FileExists(fileName))
				return true;
#else
			if (File.Exists(fileName))
				return true;
#endif
			return false;
		}

		public override Stream FileCreate(string filePath)
		{
#if WINDOWS_STOREAPP
			var folder = ApplicationData.Current.LocalFolder;
			var awaiter = folder.OpenStreamForWriteAsync(filePath, CreationCollisionOption.ReplaceExisting).GetAwaiter();
			return awaiter.GetResult();
#elif WINDOWS_PHONE
			return storage.CreateFile(filePath);
#elif NETFX_CORE || UNIVERSAL
            throw new NotImplementedException();
#else
			// return A new file with read/write access.
			return File.Create(filePath);
#endif
		}

		public override void FileDelete(string filePath)
		{
#if WINDOWS_STOREAPP
			var folder = ApplicationData.Current.LocalFolder;
			var deleteFile = folder.GetFileAsync(filePath).AsTask().GetAwaiter().GetResult();
			deleteFile.DeleteAsync().AsTask().Wait();
#elif WINDOWS_PHONE
			storage.DeleteFile(filePath);
#elif NETFX_CORE || UNIVERSAL
            throw new NotImplementedException();
#else
			// Now let's try to delete it
			File.Delete(filePath);
#endif
		}

		public override string NormalizeFilename(string fileName, string[] extensions)
		{

			if (FileExists(fileName))
				return fileName;

			// Check the file extension
			if (!string.IsNullOrEmpty(Path.GetExtension(fileName)))
				return null;

			foreach (string ext in extensions)
			{
				// Concat the file name with valid extensions
				string fileNamePlusExt = fileName + ext;

				if (FileExists(fileNamePlusExt))
					return fileNamePlusExt;
			}

			return null;
		}

		// Renamed from - public override string GetFilename(string name)
		public override string NormalizeFilePathSeperators(string name)
        {
#if NETFX_CORE || UNIVERSAL
            // Replace non-windows seperators.
			name = name.Replace('/', '\\');
#else
			// Replace Windows path separators with local path separators
			name = name.Replace('\\', Path.DirectorySeparatorChar);
#endif
			return name;
		}



		#region File Handler reciprocal overloads

		public override Stream FileOpenRead(object storageFile, string Location, string safeName)
		{
			if (storageFile == null)
			{
				throw new NullReferenceException("Must supply a storageFile reference");
			}

#if WINDOWS_PHONE
			storage = (IsolatedStorageFile)storageFile;
#endif

			return FileOpenRead(Location, safeName);

		}

		public override bool FileExists(object storageFile, string fileName)
		{
			if (storageFile == null)
			{
				throw new NullReferenceException("Must supply a storageFile reference");
			}

#if WINDOWS_PHONE
			storage = (IsolatedStorageFile)storageFile;
#endif

			return FileExists(fileName);
		}

		public override Stream FileCreate(object storageFile, string filePath)
		{
			if (storageFile == null)
			{
				throw new NullReferenceException("Must supply a storageFile reference");
			}

#if WINDOWS_PHONE
			storage = (IsolatedStorageFile)storageFile;
#endif

			return FileCreate(filePath);
		}

		public override void FileDelete(object storageFile, string filePath)
		{
			if (storageFile == null)
			{
				throw new NullReferenceException("Must supply a storageFile reference");
			}

#if WINDOWS_PHONE
			storage = (IsolatedStorageFile)storageFile;
#endif

			FileDelete(filePath);
		}

		#endregion

		#endregion

		#region Directory Handlers

		public override bool DirectoryExists(string dirPath)
        {
#if WINDOWS_STOREAPP || NETFX_CORE || UNIVERSAL
            var folder = ApplicationData.Current.LocalFolder;

			try
			{
				var result = folder.GetFolderAsync(dirPath).GetResults();
				return result != null;
			}
			catch
			{
				return false;
			}
#elif WINDOWS_PHONE
			return storage.DirectoryExists(dirPath);
#else
			return Directory.Exists(dirPath);
#endif
		}

		public override string[] DirectoryGetFiles(string storagePath)
        {
#if WINDOWS_STOREAPP || NETFX_CORE || UNIVERSAL
            var folder = ApplicationData.Current.LocalFolder;
			var results = folder.GetFilesAsync().AsTask().GetAwaiter().GetResult();
			return results.Select<StorageFile, string>(e => e.Name).ToArray();
#elif WINDOWS_PHONE
			return storage.GetFileNames(storagePath);
#elif ANDROID
			string[] files = null;
			if (!filesInFolders.TryGetValue(storagePath, out files))
			{
				files = Game.Activity.Assets.List(storagePath);
				filesInFolders[storagePath] = files;
			}
			return filesInFolders[storagePath];
#else
			return Directory.GetFiles(storagePath);
#endif
		}

		public override string[] DirectoryGetFiles(string storagePath, string searchPattern)
		{
			if (string.IsNullOrEmpty(searchPattern))
				throw new ArgumentNullException("Parameter searchPattern must contain a value.");

#if WINDOWS_STOREAPP || NETFX_CORE || UNIVERSAL
            var folder = ApplicationData.Current.LocalFolder;
			var options = new QueryOptions( CommonFileQuery.DefaultQuery, new [] { searchPattern } );
			var query = folder.CreateFileQueryWithOptions(options);
			var files = query.GetFilesAsync().AsTask().GetAwaiter().GetResult();
			return files.Select<StorageFile, string>(e => e.Name).ToArray();
#else
			return Directory.GetFiles(storagePath, searchPattern);
#endif
		}

		public override string[] DirectoryGetDirectories(string storagePath)
        {
#if WINDOWS_STOREAPP || NETFX_CORE || UNIVERSAL
            var folder = ApplicationData.Current.LocalFolder;
			var results = folder.GetFoldersAsync().AsTask().GetAwaiter().GetResult();
			return results.Select<StorageFolder, string>(e => e.Name).ToArray();
#else
			return Directory.GetDirectories(storagePath);
#endif
		}

		public override string[] DirectoryGetDirectories(string storagePath, string searchPattern)
		{
			throw new NotImplementedException();
		}

		public override void DirectoryCreate(string directory)
		{
			if (string.IsNullOrEmpty(directory))
				throw new ArgumentNullException("Parameter directory must contain a value.");

            // Now let's try to create it
#if WINDOWS_STOREAPP || NETFX_CORE || UNIVERSAL
            var folder = ApplicationData.Current.LocalFolder;
			var task = folder.CreateFolderAsync(directory, CreationCollisionOption.OpenIfExists);
			task.AsTask().Wait();
#else
			Directory.CreateDirectory(directory);
#endif
		}

		/// <summary>
		/// Creates a new directory in the storage-container.
		/// </summary>
		/// <param name="directory">Relative path of the directory to be created.</param>
		public override void DirectoryCreate(string directory, string storagePath)
		{
			if (string.IsNullOrEmpty(directory))
				throw new ArgumentNullException("Parameter directory must contain a value.");

			// relative so combine with our path
			var dirPath = Path.Combine(storagePath, directory);

			DirectoryCreate(dirPath);
		}

		public override void DirectoryDelete(string dirPath)
        {
#if WINDOWS_STOREAPP || NETFX_CORE || UNIVERSAL
            var folder = ApplicationData.Current.LocalFolder;
			var deleteFolder = folder.GetFolderAsync(dirPath).AsTask().GetAwaiter().GetResult();
			deleteFolder.DeleteAsync().AsTask().Wait();
#else
			Directory.Delete(dirPath);
#endif
		}
		
		public override string GetInstallPath()
		{
			string Location = string.Empty;
		//Get Install Path 
#if WINDOWS || LINUX
			Location = AppDomain.CurrentDomain.BaseDirectory;
#elif NETFX_CORE
			Location = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
#elif IOS || MONOMAC
			Location = NSBundle.MainBundle.ResourcePath;
#elif PSM
			Location = "/Application";
#else
			Location = string.Empty;
#endif
			return Location;
		}

		#region Directory Handler reciprocal overloads

		public override string[] DirectoryGetFiles(object storageFile, string storagePath)
		{
			if (storageFile == null)
			{
				throw new NullReferenceException("Must supply a storageFile reference");
			}

#if WINDOWS_PHONE
			storage = (IsolatedStorageFile)storageFile;
#endif

			return DirectoryGetFiles(storagePath);
		}

		public override bool DirectoryExists(string dirPath, out object storageFile)
		{

#if WINDOWS_PHONE
			storageFile = storage;
			if (storage == null)
			{
				return false;
			}
#else
			storageFile = null;
#endif

			return DirectoryExists(dirPath);
		}

		#endregion

		#endregion

		#region Stream Handlers

		public override Stream OpenStream(string rootDirectory, string assetName, string extension)
		{
			Stream stream = null;
			try
			{
				string assetPath = Path.Combine(rootDirectory, assetName) + extension;

#if WINDOWS_PHONE
				stream = new IsolatedStorageFileStream(assetPath, FileMode.Create, storage);
#elif !NETFX_CORE && !UNIVERSAL
				stream = TitleContainer.OpenStream(assetPath);
#if ANDROID
				SeekStreamtoStart(stream, 0);
#else
				SeekStreamtoStart(stream);
#endif
#endif
			}
			catch (FileNotFoundException fileNotFound)
			{
				throw new FileNotFoundException("The content file was not found.", fileNotFound);
			}
#if !NETFX_CORE && !UNIVERSAL
			catch (DirectoryNotFoundException directoryNotFound)
			{
				throw new DirectoryNotFoundException("The directory was not found.", directoryNotFound);
			}
#endif
			catch (Exception exception)
			{
				throw new Exception("Opening stream error.", exception);
			}
			return stream;
		}

		public override Stream SeekStreamtoStart(Stream stream, long StartPos, out long pos)
		{
#if ANDROID
				// Android native stream does not support the Position property. LzxDecoder.Decompress also uses
				// Seek.  So we read the entirity of the stream into a memory stream and replace stream with the
				// memory stream.
				MemoryStream memStream = new MemoryStream();
				stream.CopyTo(memStream);
				memStream.Seek(0, SeekOrigin.Begin);
				stream.Dispose();
				stream = memStream;
				// Position is at the start of the MemoryStream as Stream.CopyTo copies from current position
				pos = 0;
#else
				pos = StartPos;
#endif
				return stream;
		}

		public override void StreamClose(Stream stream)
		{
#if !NETFX_CORE && !UNIVERSAL
			stream.Close();
#endif
		}


		public override async Task<Stream> OpenStreamAsync(string name)
		{
#if NETFX_CORE
			var package = Windows.ApplicationModel.Package.Current;

			try
			{
				var storageFile = await package.InstalledLocation.GetFileAsync(name);
				var randomAccessStream = await storageFile.OpenReadAsync();
				return randomAccessStream.AsStreamForRead();
			}
			catch (IOException)
			{
				// The file must not exist... return a null stream.
				return null;
			}
#else
			Stream returnStream = null;
			await Task.Factory.StartNew(() =>
				{
					if (FileExists(name))
					{
						try
						{
							returnStream = FileOpenRead("", name);
						}
						catch { }
					}
				});
			return returnStream;
#endif

		}

        public override async Task<Stream> OpenStreamAsyncFromProject(string name)
        {
#if NETFX_CORE
			var package = Windows.ApplicationModel.Package.Current;

			try
			{
				var storageFile = await package.InstalledLocation.GetFileAsync(name);
				var randomAccessStream = await storageFile.OpenReadAsync();
				return randomAccessStream.AsStreamForRead();
			}
			catch (IOException)
			{
				// The file must not exist... return a null stream.
				return null;
			}
#else
            Stream returnStream = null;
            await Task.Factory.StartNew(() =>
            {
                    try
                    {
                        returnStream = FileOpenRead(new Uri(name, UriKind.Relative), name);
                    }
                    catch { }
            });
            return returnStream;
#endif

        }


		#region Stream Handler reciprocal overloads

		public override Stream SeekStreamtoStart(Stream stream)
		{
			long StartPos = stream.Position;
			return SeekStreamtoStart(stream, StartPos, out StartPos);
		}

		public override Stream SeekStreamtoStart(Stream stream, long StartPos)
		{
			return SeekStreamtoStart(stream, StartPos, out StartPos);
		}


		#endregion

		#endregion

        #region Save/Load Functions

        public override async Task<bool> SaveData(string path, string data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(await OpenStreamAsync(path)))
                {
                    await writer.WriteLineAsync(data);
                }
                return true;
            }
            catch { }

            return false;

        }

        public override async Task<object> LoadData(string path, System.Type type)
        {
            try
            {
                using (Stream stream = await OpenStreamAsync(path))
                {
                    // Deserialize the Session State
                    XmlSerializer x = new XmlSerializer(type);
                    return x.Deserialize(stream);
                }
            }
            catch { return null; }
        }

        public override async Task<string> LoadData(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(await OpenStreamAsync(path)))
                {
                    return sr.ReadToEnd();
                }
            }
            catch { return null; }
        }

#if NETFX_CORE
        public static void SafeDeleteFile(string filename)
        {
            SafeDeleteFile(defaultFolder, filename);
        }

        public static void SafeDeleteFile(StorageFolder folder, string filename)
        {
            try
            {
                DeleteFile(folder, filename);
            }
            catch { }
        }

        public static void DeleteFile(string filename)
        {
            DeleteFile(defaultFolder, filename);
        }

        public async static void DeleteFile(StorageFolder folder, string filename)
        {
            var file = await folder.GetFileAsync(filename);
            await file.DeleteAsync();
        }
#endif

        #endregion

    }
}
