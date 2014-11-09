Welcome to AdRotator for XAML projects including Windows 8/8.1, Windows Phone 8.1, Universal and Windows Phone Silverlight

Included in this package is:

* This Readme :D
* ReleaseNotes
* AdRotator DLL's
* Sample "DefaultAdSetings.XML" configuration file
* Notice on WinRT projects (Win 8, 8.1, WP 8.1 & Universal apps)
* Unity installation instructions - Jump to the end of this readme

**Note
We no longer include Ad Provider specific DLL's, you need to obtain the latest version from each provider.
Here is a list of Provider Dev pages:

* PubCenter - (Win8) http://go.microsoft.com/fwlink/p/?LinkID=285861 (WP) Inc with Windows Phone SDK **See notes below
(Direct links to MS Ad SDK's - WP http://bit.ly/MSAdSDKWP8 & Windows 8/8.1 http://bit.ly/MSAdSDK81)
* Smaato - http://www.smaato.com/sdks/
* MobFox - (Contact Mobfox)
* AdMob - https://developers.google.com/mobile-ads-sdk/download
* Innerative - https://console.inner-active.com/iamp/wiki (need to be logged in)
* Inmobi - http://www.inmobi.com/developers/download-center/
* DefaulthouseAds - Build your own :D
* None, disable AdRotator remotely through the configuration file

---
To implement this control add the following XAML to your project:
Add a custom namespace to the XAML page you want to display AdRotator on, e.g.:
	<!--Windows Phone 7 / 8 -->
	xmlns:adRotator="clr-namespace:AdRotator;assembly=AdRotator"

	<!--Windows 8, 8.1, Windows Phone 8.1 and Universal projects -->
	xmlns:adRotator="using:AdRotator"

Then add the control to your page:
(We recommend embedding AdRotator in a UserControl for your project for implementation, especially if you intend to use it on several pages)

	<adRotator:AdRotatorControl 
		x:Name="MyAdRotatorControl"
		AdHeight="90"
		AdWidth="728"
		LocalSettingsLocation="defaultAdSettings.xml"
		RemoteSettingsLocation="http://<your site here>/defaultAdSettings.xml"
		AutoStartAds="True" />

A default configuration XML file is added for you with this project, with the correct Build action and copy options, just update the configuration appropriate to your installation.

If you add your configuration file manually on Windows Phone, set the build action of the config file to "Content" and use the configuration above 
AdRotator at this time does NOT support relative paths so the local configuration must be deployed in he project, for example

	DefaultSettingsFileUri="/<project assembly name>;component/defaultAdSettings.xml" <-- Not supported

If you need to share a configuration file between projects then "Link" the configuration file in the solution explorer, from a central folder or just use remote configuration.
 
Lastly, configure the XML configuration file (optionally host it on the web to allow remote configuration) for your Ad Provider settings or alternatively set the providers in the XAML

For more instructions on how to implement this control and all the other configuration options checkout the AdRotator host site
http://getadrotator.com

** PubCenter and AdDuplex support for Windows 8 / Windows 8.1 / WP 8.1
In order to support WinRT controls on WinRT platforms, like those now used by PubCenter and AdDuplex, you need to pass a reference to the assembly in your project.

For Pubcenter, install the MS Ad SDK as normal (Win 8 http://bit.ly/MSAdSDK81 and WP8.1 http://bit.ly/MSAdSDKWP8) and reference the "Microsoft Advertising SDK", then include the following in the page code behind constructor.
Note - Check the name of your control, this uses your XAML (above) or programmatic instance

	MyAdRotatorControl.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.PubCenter, typeof(AdControl)); //<- Resolve to Microsoft AdControl
	
For AdDuplex, add the control via the new AdDuplex AD SDK (http://bit.ly/AdDuplexVSIX), then include the following in the page code behind constructor.
Note - Check the name of your control, this uses your XAML (above) or programmatic instance

	MyAdRotatorControl.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.AdDuplex, typeof(AdControl)); //<- Resolve to AdDuplex AdControl

This enables AdRotator to access the local WinMD components for each control.

***For Unity, add the following in your App.XAML.cs constructor, as follows:
	//PubCenter
	AdRotatorUnitySDK.Integration.AdRotatorBridge.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.PubCenter, typeof(AdControl)); //<- Resolve to Microsoft AdControl
	//AdDuplex
	AdRotatorUnitySDK.Integration.AdRotatorBridge.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.AdDuplex, typeof(AdControl)); //<- Resolve to AdDuplex AdControl

***Unity Universal games
Unity by default puts both your App.XAML and MainPage.XAML pages in the shared project. 
TO configure AdRotator with Unity, you will need to copy these to each of the Windows Phone and Windows 8 projects because they need to be configured separately.



For further examples check the example projects in the GitHub source @ https://github.com/Adrotator/AdrotatorV2

***Note
This version of V2 also now supports our Unity plug-in for Windows 8 & Windows Phone 8 solutions.  Further details on the above site.

Final Note
----------
If you are using AdRotator and it is helping you to be more profitable, please consider donating to help support further development and maintenance.
http://getadrotator.com/

The AdRotator Team
Simon Jackson @SimonDarksideJ (http://darkgenesis.zenithmoon.com)
Gergely Orosz @GergelyOrosz (http://gregdoesit.com)