// Version 1.0.0.0
using UnityEditor;
using UnityEngine;
using AdRotatorUnitySDK.Assets.Plugins;

namespace AdRotatorUnitySDK.Assets.Editor
{
	/// <summary>
	/// Class responsible of configuring the Unity project with Ad Rotator.
	/// Store in "Assets\Editor".
	/// </summary>
	public class AdRotatorManagementCreationScript : ScriptableObject
	{
		/// <summary>
		/// Install Ad Rotator in the current scene.
		/// </summary>
		[MenuItem("GameObject/Create Other/Ad Rotator")]
		static void InstallAdRotator()
		{
			const string adManagementObjectName = "AdRotatorManagement";
			var go = GameObject.Find(adManagementObjectName);
			if (go == null)
			{
				// Create game object and configure prefab in the assets to ease adding it later on.			
				go = new GameObject(adManagementObjectName);
				go.AddComponent<AdRotatorManagement>();
				go.SetActive(true);
				Selection.activeGameObject = go;
				Debug.Log("Activating Windows 8 Internet Client capability to enable usage of Ad Rotator.");
				PlayerSettings.Metro.SetCapability(PlayerSettings.MetroCapability.InternetClient, true);

				Debug.Log(string.Format("Ad Rotator Management game object added to scene '{0}'.", EditorApplication.currentScene));
			}
			else
			{
				// Already existing game object, just select it for edition.
				Selection.activeGameObject = go;
				Debug.Log(string.Format("Ad Rotator Management game object selected in scene '{0}'.", EditorApplication.currentScene));
			}
		}
	}
}
