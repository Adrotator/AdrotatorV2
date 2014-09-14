// Version 2.0.0.0
using System;
using UnityEngine;

namespace AdRotatorUnitySDK.Assets.Plugins
{
	/// <summary>
	/// Component responsible of managing interactions and settings of Ad Rotator.
	/// Store in "Assets\Plugins".
	/// </summary>
	[Serializable]
	public class AdRotatorManagement : MonoBehaviour
	{
		/// <summary>
		/// This delegate is primarily intended for internal use only to communicate with Windows 8. Do not use.
		/// </summary>
		/// <param name="arg">Ad layout settings to send to Windows 8.</param>
		public delegate void OnSettingsUpdatedHandler(AdSettings ad, AppSettings app);

		/// <summary>
		/// This variable is primarily intended for internal use only to communicate with Windows 8. Do not modify this variable.
		/// </summary>
		public OnSettingsUpdatedHandler OnSettingsUpdated = null;

		/// <summary>
		/// Ad Layout Settings. 
		/// </summary>
		[SerializeField]
		public AdSettings AdSettings;

		/// <summary>
		/// Ad Provider Settings.
		/// </summary>
		[SerializeField]
		public AppSettings AppSettings;

		/// <summary>
		/// Initializes a new instance of AdRotatorManagement.
		/// </summary>
		public AdRotatorManagement()
		{
			this.AdSettings = new AdSettings();
			this.AppSettings = new AppSettings();
		}

		/// <summary>
		/// Script instance is being loaded.
		/// </summary>
		void Awake()
		{
			// Make sure ad management instance and related link is kept when switching scene.
			this.name = "AdRotatorManagement";
			UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
		}

		/// <summary>
		/// Reset to default values.
 		/// </summary>
		void Reset()
		{
			this.AdSettings = new AdSettings();
			this.AppSettings = new AppSettings();
		}

		/// <summary>
		/// Notify the UI Shell that Ad Rotator layout settings need to be updated.
		/// Prior calling this method, AdSettings values needs to be updated.
		/// </summary>
		public void UpdateAd()
		{
			if (OnSettingsUpdated != null)
			{
				Debug.Log("Update Ad Rotator Display Settings.");
				OnSettingsUpdated(new AdSettings(this.AdSettings), new AppSettings(this.AppSettings));
			}
			else
			{
				Debug.LogError("Ad Rotator Management OnSettingsUpdated handler has not been registered.");
			}
		}
	}
}
