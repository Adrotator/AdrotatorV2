// Version 1.0.0.0
using UnityEditor;
using UnityEngine;
using AdRotatorUnitySDK.Demo.Assets.Plugins;
using AdRotatorUnitySDK.Assets.Plugins;

namespace AdRotatorUnitySDK.Demo.Assets.Editor
{
	/// <summary>
	/// Class responsible of configuring the Unity project with appropriate build settings for the demo.
	/// </summary>
	public class ConfigureDemoScript : ScriptableObject
	{
		private const string MainSceneFile = "Assets/AdRotatorDemo.unity";
		private const string GreenSceneFile = "Assets/green.unity";
		private const string RedSceneFile = "Assets/red.unity";

		/// <summary>
		/// Install Ad Rotator in the current scene.
		/// </summary>
		[MenuItem("Edit/Configure Ad Rotator Demo")]
		static void ConfigureDefaultSettings()
		{
			ConfigureAdRotatorSettings();
			ConfigureBuild();
		}

		/// <summary>
		/// Install Ad Rotator in the current scene.
		/// </summary>
		private static void ConfigureAdRotatorSettings()
		{
			EditorApplication.SaveScene();
			if (!EditorApplication.OpenScene(MainSceneFile))
			{
				Debug.LogError(string.Format("Scene not found: {0}", EditorApplication.currentScene));
			}
			else
			{
				// Install Game Object
				EditorApplication.ExecuteMenuItem("GameObject/Create Other/Ad Rotator");

				// Configure
				var ads = GameObject.Find("AdRotatorManagement");
				if (ads != null)
				{
					var adMgt = ads.GetComponent<AdRotatorManagement>();
					adMgt.AdSettings.IsEnabled = true;
					adMgt.AdSettings.Position = AdPosition.TopCenter;
					adMgt.AdSettings.Size = AdSize.Leaderboard728x90;
					adMgt.AdSettings.SlidingAdDirection = SlidingAdDirection.Top;
					adMgt.AdSettings.SlidingAdDisplaySeconds = 5;
					adMgt.AdSettings.SlidingAdHiddenSeconds = 1;

					adMgt.AppSettings.SettingsUrl = "";
					Selection.activeGameObject = ads;
				}
				else
				{
					Debug.LogError(string.Format("AdRotatorManagement game object cannot be found in scene '{0}'. Add it to the scene using menu 'GameObject->Create Other->Ad Rotator'.", EditorApplication.currentScene));
				}
				EditorApplication.SaveScene();
			}
		}

		/// <summary>
		/// Configure build and add display demo.
		/// </summary>
		private static void ConfigureBuild()
		{
			EditorApplication.SaveScene();
			var initialScene = EditorApplication.currentScene;

			EditorBuildSettings.scenes = new[]
			{
				new EditorBuildSettingsScene(MainSceneFile, true),
				new EditorBuildSettingsScene(GreenSceneFile, true),
				new EditorBuildSettingsScene(RedSceneFile, true),
			};

			EditorUserBuildSettings.metroBuildType = MetroBuildType.VisualStudioCSharp;
			EditorUserBuildSettings.selectedMetroTarget = BuildTarget.MetroPlayer;
			
			foreach (var scene in EditorBuildSettings.scenes)
			{
				if (!EditorApplication.OpenScene(scene.path))
				{
					Debug.LogError(string.Format("Scene '{0}' is not found.", scene.path));
					continue;
				}

				var mainCam = GameObject.Find("Main Camera");
				if (mainCam != null)
				{
					var displayDemo = mainCam.GetComponent<DisplayDemo>();
					if (displayDemo == null)
					{
						mainCam.AddComponent<DisplayDemo>();
					}
				}
				else
				{
					Debug.LogError(string.Format("Main Camera not found in scene '{0}'.", scene.path));
					continue;
				}
				EditorApplication.SaveScene();
			}
			EditorApplication.OpenScene(initialScene);
		}
	}
}
