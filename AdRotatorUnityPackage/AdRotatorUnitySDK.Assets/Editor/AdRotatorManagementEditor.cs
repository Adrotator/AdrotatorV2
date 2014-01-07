// Version 1.0.0.0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using AdRotatorUnitySDK.Assets.Plugins;

namespace AdRotatorUnitySDK.Assets.Editor
{
	using System.Collections;

	/// <summary>
	/// Ad Rotator settings editor.
	/// Store in "Assets\Editor".
	/// </summary>
	[CustomEditor(typeof(AdRotatorManagement))]
	public class AdRotatorManagementEditor : UnityEditor.Editor
	{
		/// <summary>
		/// Ad Rotator inspector GUI layout and behavior.
		/// </summary>
		public override void OnInspectorGUI()
		{
			var customTarget = (AdRotatorManagement)base.target;

			GUILayout.BeginVertical();
			customTarget.AdSettings.IsEnabled = EditorGUILayout.Toggle(new GUIContent("Enabled?", "Check to enable Ad Rotator. Ad will not be displayed if disabled."), customTarget.AdSettings.IsEnabled);
			if (!customTarget.AdSettings.IsEnabled)
			{
				GUI.contentColor = Color.gray;
			}

			GUILayout.Label("Layout", EditorStyles.boldLabel);
			customTarget.AdSettings.Position = (AdPosition)EditorGUILayout.EnumPopup(new GUIContent("  Ad Position", "Screen position of the Ad banner."), customTarget.AdSettings.Position);
			customTarget.AdSettings.Size = (AdSize)EditorGUILayout.EnumPopup(new GUIContent("  Ad Size", "Size of the Ad Banner. Only supported sizes are listed."), customTarget.AdSettings.Size);
			GUILayout.Label("");
			GUILayout.Label("Sliding Ads", EditorStyles.boldLabel);
			customTarget.AdSettings.SlidingAdDirection = (SlidingAdDirection)EditorGUILayout.EnumPopup(new GUIContent("  Direction", "Set SlidingAdDirection to Left, Right, Bottom or Top to have the ad slide in, stay for SlidingAdDisplaySeconds, slide out and stay hidden for SlidingAdHiddenSeconds. If SlidingAdDirection is set to None (this is the default), this behaviour does not take place, the ad remains static."), customTarget.AdSettings.SlidingAdDirection);

			if (customTarget.AdSettings.SlidingAdDirection == SlidingAdDirection.None && customTarget.AdSettings.IsEnabled)
			{
				GUI.contentColor = Color.gray;
			}

			customTarget.AdSettings.SlidingAdDisplaySeconds = EditorGUILayout.IntSlider(new GUIContent("  Ad Display in seconds", "When Sliding Ad is enabled, ad stays for SlidingAdDisplaySeconds, slide out and stay hidden for SlidingAdHiddenSeconds."), customTarget.AdSettings.SlidingAdDisplaySeconds, 0, 1800);
			customTarget.AdSettings.SlidingAdHiddenSeconds = EditorGUILayout.IntSlider(new GUIContent("  Ad Hidden in seconds", "When Sliding Ad is enabled, ad stays for SlidingAdDisplaySeconds, slide out and stay hidden for SlidingAdHiddenSeconds."), customTarget.AdSettings.SlidingAdHiddenSeconds, 0, 1800);	

			GUI.contentColor = Color.white;
			GUILayout.Label("");
			GUILayout.Label("Ad Providers Settings", EditorStyles.boldLabel);
			customTarget.AppSettings.DefaultAdType = (AdProvider)EditorGUILayout.EnumPopup(new GUIContent("  Default Ad Type", "What ad type should be shown if either the ad settings file could not be loaded or other ad providers have failed to load."), customTarget.AppSettings.DefaultAdType);
			//customTarget.AppSettings.DefaultSettingsFileUri = EditorGUILayout.TextField(new GUIContent("  Default Ad Settings URL", "URI to a local XML file that will be used if the remote file specified with Ad Settings Url could not be loaded. Example: defaultAdSettings.xml"), customTarget.AppSettings.DefaultSettingsFileUri);
			customTarget.AppSettings.SettingsUrl = EditorGUILayout.TextField(new GUIContent("  Ad Settings URL", "URL to the remote XML file that controls the probability of ad providers shown. Strongly advised to set this property. Example: http://mydomain.com/myAdSettings.xml"), customTarget.AppSettings.SettingsUrl);

			GUILayout.Label("  House Ad");
			customTarget.AppSettings.DefaultHouseAdBody = EditorGUILayout.TextField(new GUIContent("    Default House Ad Body", "Name of the XAML control to use (format: <namespace>.<object name>), need to be inside your application assembly or referenced by your project. See AdRotator documentation for more information. Example: AdRotatorExample.MyDefaultAd"), customTarget.AppSettings.DefaultHouseAdBody);
			customTarget.AppSettings.DefaultHouseAdUri = EditorGUILayout.TextField(new GUIContent("    Default House Ad URL", "URL to the remote XAML file to use as the House Ad. See AdRotator documentation for more information. Example: http://mydomain.com/myHouseAd.xaml"), customTarget.AppSettings.DefaultHouseAdUri);
			
			GUILayout.EndVertical();
			
			if (GUI.changed)
			{
				EditorUtility.SetDirty(customTarget);
			}			
		}
	}
}