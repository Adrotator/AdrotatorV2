// Version 1.0.0.0
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Xml;
using System.Reflection;

namespace AdRotatorUnitySDK.Assets.Editor
{
	/// <summary>
	/// Post build steps for Ad Rotator integration inside Windows Store App project.
	/// Store in "Assets\Editor".
	/// </summary>
	public static class AdRotatorPostBuild
	{
		/// <summary>
		/// Modify generated code and project to support Ad Rotator.
		/// </summary>
		/// <param name="target">Ongoing build target value.</param>
		/// <param name="path">Target directory of the build.</param>
		[PostProcessBuild(999)]
		public static void OnPostProcessBuild(BuildTarget target, string path)
		{
			if ((target == BuildTarget.MetroPlayer && UnityEditor.EditorUserBuildSettings.metroBuildType == MetroBuildType.VisualStudioCSharp) || (target == BuildTarget.WP8Player) )
			{				
				var projectParentDir = Directory.GetParent(Application.dataPath);
				var assetsDir = new DirectoryInfo(Path.Combine(projectParentDir.FullName, "Assets"));
				var generatedCodePath = DetermineGeneratedCodePath(path);

				// Include custom code and Ad Rotator configuration				
				var generatedCodeDir = new DirectoryInfo(generatedCodePath);
				const string rotatorTargetFile = "AdRotatorUnitySDK.targets";
				const string adSettingsTargetFile = "AdRotatorUnitySDK.AdSettings.targets";
				const string houseAdsTargetFile = "AdRotatorUnitySDK.HouseAds.targets";
				var adRotatorAssetPath = Path.Combine(assetsDir.FullName, "AdRotator");

				// Copy XAML, XML and Targets file to the proejct folder
				CopyFilesToProject(target, generatedCodeDir, adRotatorAssetPath);

				// Add Import statements and build targets in csproj
				UpdateCsProj(generatedCodeDir, target, rotatorTargetFile, adSettingsTargetFile, houseAdsTargetFile, adRotatorAssetPath);

                if (target == BuildTarget.MetroPlayer)
                {
                    // Add custom code next to already generated code in App.xaml.cs
                    UpdateSourceCodeWin8(generatedCodeDir);
                }
                if (target == BuildTarget.WP8Player)
                {
                    // Add custom code next to already generated code in App.xaml.cs
                    UpdateSourceCodeWP8(generatedCodeDir);
                    UpdateCapabilitiesWP8(generatedCodeDir);
                }
			}
		}

		/// <summary>
		/// Detect the path where code from this build is being generated.
		/// </summary>
		/// <param name="path">Target path received from Unity.</param>
		/// <returns>Detected code path.</returns>
		private static string DetermineGeneratedCodePath(string path)
		{			
			var generatedCodePath = Path.Combine(path, PlayerSettings.productName);

			if (!Directory.Exists(generatedCodePath))
			{
				var targetDir = Directory.GetParent(path);
				generatedCodePath = Path.Combine(path, targetDir.Name);
				if (!Directory.Exists(generatedCodePath))
				{
					var csProject = Directory.GetFiles(path, "*.csproj", SearchOption.AllDirectories).FirstOrDefault();
					if (csProject != null)
					{
						generatedCodePath = Path.GetDirectoryName(csProject);
					}
				}
			}
			return generatedCodePath;
		}

		/// <summary>
		/// Copy files from Assets/AdRotator to the generated project path.
		/// </summary>
		/// <param name="generatedCodeDir">Directory where code is generated.</param>
		/// <param name="adRotatorAssetPath">AdRotator Assets path from the Unity Project.</param>
		private static void CopyFilesToProject(BuildTarget target, DirectoryInfo generatedCodeDir, string adRotatorAssetPath)
		{
            if (target == BuildTarget.MetroPlayer)
            {
                File.Copy(Path.Combine(adRotatorAssetPath, "AdRotatorBridgeWin8.cs.ignore"), Path.Combine(generatedCodeDir.FullName, "AdRotatorBridge.cs"), true);
                File.Copy(Path.Combine(adRotatorAssetPath, "defaultAdSettingsWin8.xml"), Path.Combine(generatedCodeDir.FullName, "defaultAdSettings.xml"), true);
            }
            if (target == BuildTarget.WP8Player)
            {
                File.Copy(Path.Combine(adRotatorAssetPath, "AdRotatorBridgeWP8.cs.ignore"), Path.Combine(generatedCodeDir.FullName, "AdRotatorBridge.cs"), true);
                File.Copy(Path.Combine(adRotatorAssetPath, "defaultAdSettingsWP8.xml"), Path.Combine(generatedCodeDir.FullName, "defaultAdSettings.xml"), true);
            }
			//CopyFiles(adRotatorAssetPath, generatedCodeDir.FullName, "*.xml");
			CopyFiles(adRotatorAssetPath, generatedCodeDir.FullName, "*.xaml");
			CopyFiles(adRotatorAssetPath, generatedCodeDir.FullName, "*.targets");
		}

		/// <summary>
		/// Update CS project to include custom target files along with any reference missing.
		/// </summary>
		/// <param name="generatedCodeDir">Code where CS Project is found.</param>
		/// <param name="rotatorTargetFile">Name of the Bridge target file.</param>
		/// <param name="adSettingsTargetFile">Name of the Ad Settings target file.</param>
		/// <param name="houseAdsTargetFile">Name of the house Ads target file.</param>
		/// <param name="adRotatorAssetPath">Path of the AdRotator assets in the Unity project.</param>
        private static void UpdateCsProj(DirectoryInfo generatedCodeDir, BuildTarget target, string rotatorTargetFile, string adSettingsTargetFile, string houseAdsTargetFile, string adRotatorAssetPath)
		{			
			var unityProject = Path.Combine(generatedCodeDir.FullName, generatedCodeDir.Name + ".csproj");
			XElement xmlProject = XElement.Load(unityProject);
			var saveProjectFile = false;
			var rotatorTargets = from item in xmlProject.Elements(XName.Get("Import", xmlProject.GetDefaultNamespace().NamespaceName)) where item.Attributes("Project").Any(att => att.Value.Contains(rotatorTargetFile)) select item;

			// If Project not modified yet (no import found), then insert custom code and modify project.
			if (rotatorTargets.Count() == 0)
			{
				InsertImportTarget(xmlProject, rotatorTargetFile);
				InsertImportTarget(xmlProject, adSettingsTargetFile);
				InsertImportTarget(xmlProject, houseAdsTargetFile);

				saveProjectFile = true;
			}

			// Update targets files for Xaml and configXML settings	
            UpdateTargetFileContent(Path.Combine(generatedCodeDir.FullName, adSettingsTargetFile), "defaultAdSettings.xml");


			var xamlFiles = GetFiles(adRotatorAssetPath, "*.xaml");
			UpdateTargetFileXamlPage(Path.Combine(generatedCodeDir.FullName, houseAdsTargetFile), xamlFiles.Select(f => f.Name));

			if (saveProjectFile)
			{
				xmlProject.Save(unityProject);
			}
		}

		/// <summary>
		/// Update Unity generated source code to include the Unity to Windows 8 hook used for AdRotator.
		/// </summary>
		/// <param name="generatedCodeDir">Path of the generated source code.</param>
		private static void UpdateSourceCodeWin8(DirectoryInfo generatedCodeDir)
		{
			var appCsFile = Path.Combine(generatedCodeDir.FullName, "App.xaml.cs");
			var appCsFileAllLines = File.ReadAllLines(appCsFile);
			var saveFile = false;
			for (int i = 0; i < appCsFileAllLines.Length; i++)
			{
				var line = appCsFileAllLines[i];
				if (line.Contains("AdRotatorUnitySDK"))
				{
					// File is already modified.						
					break;
				}
				else if (line.Contains(".InitializeD3DXAML("))
				{
					appCsFileAllLines[i] = line + Environment.NewLine + "AppCallbacks.Instance.InvokeOnAppThread(new AppCallbackItem(() => { AdRotatorUnitySDK.Integration.AdRotatorBridge.Register(mainPage, toCall => AppCallbacks.Instance.InvokeOnUIThread(new AppCallbackItem(toCall), false)); }), false);" + Environment.NewLine;
					saveFile = true;
				}
			}
            //var mainCsFile = Path.Combine(generatedCodeDir.FullName, "MainPage.xaml.cs");
            //var mainCsFileAllLines = File.ReadAllLines(mainCsFile);
            //for (int i = 0; i < mainCsFileAllLines.Length; i++)
            //{
            //    var line = mainCsFileAllLines[i];
            //    if (line.Contains("Win8AdRotator"))
            //    {
            //        // File is already modified.						
            //        return;
            //    }
            //    else if (line.Contains("public MainPage(SplashScreen splashScreen)"))
            //    {
            //        mainCsFileAllLines[i] = line + 
            //            Environment.NewLine + "            this.InitializeComponent();" + Environment.NewLine +
            //            Environment.NewLine + "            splash = splashScreen;" + Environment.NewLine +
            //            Environment.NewLine + "            GetSplashBackgroundColor();" + Environment.NewLine +
            //            Environment.NewLine + "            OnResize();" + Environment.NewLine +
            //            Environment.NewLine + "            Window.Current.SizeChanged += onResizeHandler = new WindowSizeChangedEventHandler((o, e) => OnResize());" + Environment.NewLine +
            //            "            Win8AdRotator.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.PubCenter, typeof(Microsoft.Advertising.WinRT.UI.AdControl));" + Environment.NewLine;
            //        saveFile = true;
            //    }
            //}

			if (saveFile)
			{
				File.WriteAllLines(appCsFile, appCsFileAllLines);
			}
		}

        /// <summary>
        /// Update Unity generated source code to include the Unity to Windows 8 hook used for AdRotator.
        /// </summary>
        /// <param name="generatedCodeDir">Path of the generated source code.</param>
        private static void UpdateSourceCodeWP8(DirectoryInfo generatedCodeDir)
        {
            var appCsFile = Path.Combine(generatedCodeDir.FullName, "MainPage.xaml.cs");
            var appCsFileAllLines = File.ReadAllLines(appCsFile);
            var saveFile = false;
            for (int i = 0; i < appCsFileAllLines.Length; i++)
            {
                var line = appCsFileAllLines[i];
                if (line.Contains("AdRotatorUnitySDK"))
                {
                    // File is already modified.						
                    break;
                }
                if (line.Contains("private void Unity_Loaded()"))
                {
                    appCsFileAllLines[i] = "void ExecuteOnUIThread(Delegate todo)" + Environment.NewLine + "{" + Environment.NewLine + "Dispatcher.BeginInvoke(todo);" + Environment.NewLine + "}" + Environment.NewLine + Environment.NewLine + line;
                    appCsFileAllLines[i + 1] = "{" + Environment.NewLine + "UnityApp.BeginInvoke(() => AdRotatorUnitySDK.Integration.AdRotatorBridge.Register(this, toCall => ExecuteOnUIThread(toCall)));" + Environment.NewLine;
                    saveFile = true;
                }
            }

            if (saveFile)
            {
                File.WriteAllLines(appCsFile, appCsFileAllLines);
            }
        }

        /// <summary>
        /// Update Unity generated source code to include the Unity to Windows 8 hook used for AdRotator.
        /// </summary>
        /// <param name="generatedCodeDir">Path of the generated source code.</param>
        private static void UpdateCapabilitiesWP8(DirectoryInfo generatedCodeDir)
        {
            var appCapsFile = Path.Combine(generatedCodeDir.FullName, "Properties\\WMAppManifest.xml");
            var appCapsFileAllLines = File.ReadAllLines(appCapsFile);
            var saveFile = false;
            for (int i = 0; i < appCapsFileAllLines.Length; i++)
            {
                var line = appCapsFileAllLines[i];
                if (line.Contains("ID_CAP_WEBBROWSERCOMPONENT"))
                {
                    // Web browser capability already in project					
                    break;
                }
                if (line.Contains("</Capabilities>"))
                {
                    appCapsFileAllLines[i] = "      <Capability Name=\"ID_CAP_WEBBROWSERCOMPONENT\" />" + Environment.NewLine + line;
                    saveFile = true;
                }
            }

            if (saveFile)
            {
                File.WriteAllLines(appCapsFile, appCapsFileAllLines);
            }
        }
        
        /// <summary>
		/// Update target file containing XML content files for Ad settings.
		/// </summary>
		/// <param name="targetFile">Target file to modify.</param>
		/// <param name="files">XML files to add.</param>
		private static void UpdateTargetFileContent(string targetFile, string filename)
		{
			// <Content Include="defaultAdSettings.xml">
		    //    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
            // </Content>
			XElement xmlTarget = XElement.Load(targetFile);
			var defaultNamespace = xmlTarget.GetDefaultNamespace().NamespaceName;
			var itemGroup = xmlTarget.Elements(XName.Get("ItemGroup", defaultNamespace)).FirstOrDefault();

            if (itemGroup != null)
            {
                itemGroup.RemoveAll();
                itemGroup.Add(new XElement(XName.Get("Content", defaultNamespace), new XAttribute("Include", filename),
                new XElement(XName.Get("CopyToOutputDirectory", defaultNamespace), "Always")));
            }
			xmlTarget.Save(targetFile);
		}

		/// <summary>
		/// Update target file containing XAML Page files for Ad settings.
		/// </summary>
		/// <param name="targetFile">Target file to modify.</param>
		/// <param name="files">XAML files to add.</param>
		private static void UpdateTargetFileXamlPage(string targetFile, IEnumerable<string> files)
		{
			// <Page Include="MyPage.xaml">
			//    <SubType>Designer</SubType>
			//    <Generator>MSBuild:Compile</Generator>
			// </Page>
			XElement xmlTarget = XElement.Load(targetFile);
			var defaultNamespace = xmlTarget.GetDefaultNamespace().NamespaceName;
			var itemGroup = xmlTarget.Elements(XName.Get("ItemGroup", defaultNamespace)).FirstOrDefault();

			if (itemGroup != null)
			{
				itemGroup.RemoveAll();
				foreach (var file in files)
				{
					itemGroup.Add(new XElement(XName.Get("Page", defaultNamespace), new XAttribute("Include", file),
					new XElement(XName.Get("SubType", defaultNamespace), "Designer"),
					new XElement(XName.Get("Generator", defaultNamespace), "MSBuild:Compile")));
				}
			}
			xmlTarget.Save(targetFile);
		}

		/// <summary>
		/// Copy files with a search pattern from one location to another, including searching in 
		/// the subdirectories.
		/// </summary>
		/// <param name="source">Source folder where to search for files.</param>
		/// <param name="destination">Destination folder where the files will be stored.</param>
		/// <param name="searchPattern">Wildcard search pattern to use.</param>
		private static void CopyFiles(string source,
									  string destination,
									  string searchPattern)
		{
			var sourceDir = new DirectoryInfo(source);
			var files = sourceDir.GetFiles(searchPattern,  SearchOption.AllDirectories);
			foreach (var file in files)
			{
				file.CopyTo(Path.Combine(destination, file.Name), true);
			}
		}

		/// <summary>
		/// Fetch all files matching the search pattern in the provided source path.
		/// </summary>
		/// <param name="source">Path where to search.</param>
		/// <param name="searchPattern">Wildcard to use to search.</param>
		/// <returns>All files matching the pattern.</returns>
		private static IEnumerable<FileInfo> GetFiles(string source, string searchPattern)
		{
			var sourceDir = new DirectoryInfo(source);
			return sourceDir.GetFiles(searchPattern, SearchOption.AllDirectories);
		}

		/// <summary>
		/// Insert Import statement in the CsProj.
		/// </summary>
		/// <param name="xmlProject">Parent where to add the Import.</param>
		/// <param name="rotatorTargetFile">Target file to add.</param>
		private static void InsertImportTarget(XElement xmlProject, string targetFile)
		{
			// Format: <Import Project=".\AdRotatorUnitySDK.targets" />
			xmlProject.Add(new XElement(XName.Get("Import", xmlProject.GetDefaultNamespace().NamespaceName), new XAttribute("Project", ".\\" + targetFile)));
		}
	}
}