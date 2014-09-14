// Version 2.0.0.0
using System;

namespace AdRotatorUnitySDK.Assets.Plugins
{
	/// <summary>
	/// Supported Ad Provider.
	/// </summary>
	public enum AdProvider
	{
		None,
		PubCenter,
		AdDuplex,
		InMobi,
		AdMob,
		Smaato,
		InnerActive,
		DefaultHouseAd,
	}

	/// <summary>
	/// Direction of the Ad sliding. Use None value to disable.
	/// </summary>
	public enum SlidingAdDirection
	{
		None,
		Bottom,
		Left,
		Right,
		Top,
	}

	/// <summary>
	/// Position of the Ad on the screen.
	/// </summary>
	public enum AdPosition
	{
		TopLeftCorner,
		TopRightCorner,
		BottomLeftCorner,
		BottomRightCorner,
		TopCenter,
		BottomCenter,
		LeftCenter,
		RightCenter,
		Center,
	}

	/// <summary>
	/// Supported Ad sizes.
	/// </summary>
	public enum AdSize
	{
		MediumRectangle300x250,
		Leaderboard728x90,
		WideSkyscraper160x600,
		SquarePopUp250x250,
		SplitViewBanner500x130,
		SnapViewBanner292x60,
		HalfTile250x125,
	}

	/// <summary>
	/// Support Ad retrieval operation
	/// </summary>
	public enum AdMode
	{
		Random,
		Ordered,
		Stepped
	}

	/// <summary>
	/// Application settings of Ad Rotator, used at initialization only.
	/// Store in "Assets\Plugins".
	/// </summary>
	[Serializable]
	public class AppSettings
	{
		/// <summary>
		/// Initializes a new instance of AppSettings.
		/// </summary>
		/// <param name="src">Settings to clone.</param>
		public AppSettings(AppSettings src)
		{
			this.SettingsUrl = src.SettingsUrl;
			this.IsTest = src.IsTest;
			this.AdMode = src.AdMode;
		}

		/// <summary>
		/// Initializes a new instance of AppSettings.
		/// </summary>
		public AppSettings()
		{
			this.SettingsUrl = "";
			this.IsTest = false;
		}

		/// <summary>
		/// URL to the remote XML file that controls the probability of ad providers shown. 
		/// Strongly advised to set this property. Example: http://mydomain.com/myAdSettings.xml
		/// </summary>
		public string SettingsUrl;

		/// <summary>
		/// URI to a local XML file that will be used if the remote file specified with Ad Settings Url could not be loaded. Example: defaultAdSettings.xml
		/// </summary>
		//public string DefaultSettingsFileUri;

		/// <summary>
		/// Enable or disable ad Test mode. See AdRotator documentation for more information.
		/// </summary>
		public bool IsTest;

		public AdMode AdMode;
	}

	/// <summary>
	/// Currently showing Ad settings.
	/// Store in "Assets\Plugins".
	/// </summary>
	[Serializable]
	public class AdSettings
	{
		/// <summary>
		/// Initializes a new instance of AdSettings.
		/// </summary>
		/// <param name="src">Settings to clone.</param>
		public AdSettings(AdSettings src)
		{
			this.IsEnabled = src.IsEnabled;
			this.Position = src.Position;
			this.Size = src.Size;
			this.SlidingAdDirection = src.SlidingAdDirection;
			this.SlidingAdDisplaySeconds = src.SlidingAdDisplaySeconds;
			this.SlidingAdHiddenSeconds = src.SlidingAdHiddenSeconds;
		}

		/// <summary>
		/// Initializes a new instance of AdSettings.
		/// </summary>
		public AdSettings()
		{
			this.IsEnabled = true;
			this.Position = AdPosition.TopCenter;
			this.Size = AdSize.Leaderboard728x90;
			this.SlidingAdDirection = SlidingAdDirection.None;
			this.SlidingAdDisplaySeconds = 0;
			this.SlidingAdHiddenSeconds = 0;
		}

		/// <summary>
		/// Use to enable Ad Rotator. Ads will not be displayed if disabled.
		/// </summary>
		public bool IsEnabled;

		/// <summary>
		/// Screen position of the Ad banner. 
		/// Note that Ad will be displayed in front of the scene, potentially hiding scene content. Select position accordingly.
		/// </summary>
		public AdPosition Position;
		
		/// <summary>
		/// Size of the Ad Banner.
		/// </summary>
		public AdSize Size;

		/// <summary>
		/// Set Sliding Ad Direction to either Left, Right, Bottom or Top to have the ad slide in, stay for 
		/// Sliding Ad Display in Seconds, slide out and stay hidden for Sliding Ad Hidden in Seconds. 
		/// If Sliding Ad Direction is set to None, this behaviour does not take place, the ad remains static.
		/// </summary>
		public SlidingAdDirection SlidingAdDirection;

		/// <summary>
		/// Display time of the ad, in seconds. 
		/// See SlidingAdDirection for more information.
		/// </summary>
		public int SlidingAdHiddenSeconds;

		/// <summary>
		/// Hidden time of the ad, in seconds. 
		/// See SlidingAdDirection for more information.
		/// </summary>
		public int SlidingAdDisplaySeconds;
	}	
}
