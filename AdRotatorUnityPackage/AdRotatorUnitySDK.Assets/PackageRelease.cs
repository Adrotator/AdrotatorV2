using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AdRotatorUnitySDK.Assets
{
	/// <summary>
	/// Script used to generate SDK and Demo Unity packages.
	/// </summary>
	class PackageRelease : ScriptableObject
	{
		[MenuItem("Edit/Export SDK")]
		static void ExportSdk()
		{
			AssetDatabase.ExportPackage(new []
			{
				"Assets\\AdRotator\\AdRotatorUnitySDK.targets", 
				"Assets\\AdRotator\\AdRotatorUnitySDK.AdSettings.targets", 
				"Assets\\AdRotator\\AdRotatorUnitySDK.HouseAds.targets", 
				"Assets\\AdRotator\\defaultAdSettingsWin8.xml", 
				"Assets\\AdRotator\\defaultAdSettingsWP8.xml", 
				"Assets\\AdRotator\\AdRotatorBridgeWin8.cs.ignore", 
				"Assets\\AdRotator\\AdRotatorBridgeWP8.cs.ignore", 
				"Assets\\Editor\\AdRotator\\AdRotatorPostBuild.cs", 
				"Assets\\Editor\\AdRotator\\AdRotatorManagementCreationScript.cs", 
				"Assets\\Editor\\AdRotator\\AdRotatorManagementEditor.cs", 
				"Assets\\Plugins\\AdRotator\\AdRotatorManagement.cs", 
				"Assets\\Plugins\\AdRotator\\AdRotatorSettings.cs",
			}, 
			"AdRotatorUnitySDK.unitypackage", 
			ExportPackageOptions.Default);
		}

		[MenuItem("Edit/Export Demo")]
		static void ExportDemo()
		{
			AssetDatabase.ExportPackage(new[]
			{
				"Assets\\AdRotatorDemo.unity", 
				"Assets\\green.unity", 
				"Assets\\red.unity", 
				"Assets\\Editor\\ConfigureDemoScript.cs", 
				"Assets\\AdRotator\\HouseAdDemo.xaml", 
				"Assets\\Plugins\\DisplayDemo.cs", 
			},
			"AdRotatorUnitySDKDemo.unitypackage",
			ExportPackageOptions.Default);
		}
	}
}
