Welcome to AdRotator for XAML projects includng Windows 8 and Windows Phone Silverlight

Included in this package is:

* This Readme :D
* ReleaseNotes
* AdRotator DLL's
* Sample "DefaultAdSetings.XML" configuration file

**Note
We no longer include Ad Provider specific DLL's, you need to obtain the latest version from each provider, here is a list of Provider Dev pages:
(Except AdDuplex as they provide a NuGet package it is included by default)

* PubCenter - (Win8) http://go.microsoft.com/fwlink/p/?LinkID=285861 (WP) Inc with Windows Phone SDK **See notes below
* Smaato - http://www.smaato.com/sdks/
* MobFox - (Contact Mobfox)
* AdMob - https://developers.google.com/mobile-ads-sdk/download
* Innerative - https://console.inner-active.com/iamp/wiki (need to be logged in)
* Inmobi - http://www.inmobi.com/developers/download-center/
* DefaulthouseAds - Build your own :D

---
**At this time we have not tested AdRotator with Universal projects, please feed back any issues to the GitHub page - https://github.com/Adrotator/AdrotatorV2


To implement this control add the following XAML to your project:
Add a custom namespace to the AdRotator Control, e.g.:
	<!--Windows Phone -->
	xmlns:adRotator="clr-namespace:AdRotator;assembly=AdRotator"

	<!--Windows 8 -->
	xmlns:adRotator="using:AdRotator"

Then add the control to your page:
(We recommend using a UserControl in your project for implementation especially if you intend to use it on several pages)

	<adRotator:AdRotatorControl 
		x:Name="AdRotator"
		AdHeight="90"
		AdWidth="728"
		LocalSettingsLocation="defaultAdSettings.xml"
		RemoteSettingsLocation="http://<your site here>/defaultAdSettings.xml"
		AutoStartAds="True" />

A default config XML file is added for you with this project with the correct Build action and copy options, just update the configuration appropriate to your installation.

If you add it manually on Windows Phone, set the build action of the config file to "Content" and use the configuration above 
AdRotator at this time does NOT support relative paths so the local configuration must be deployed in he project, for example

	DefaultSettingsFileUri="/<project assembly name>;component/defaultAdSettings.xml" <-- Not supported

If you need to share a configuration file between projects then "Link" the configuration file from a central folder or just use remote configuration.
 
Lastly configure the XML configuration file (optionally host it on the web to allow remote configuration) for your Ad Provider settings or alternatively set the providers in the XAML

For more instructions on how to implement this control and all the other configuration options checkout the AdRotator host site
http://getadrotator.com

** PubCenter and AdDuplex support for Windows 8
In order to support PubCenter due to the way the control is implemented in Windows 8/8.1, you need to pass the reference to the assembly in your project.

Add a using statement to the top of your code behind:
	using AdRotator.Model;
	
For Pubcenter, install the Windows 8/8.1 SDK as normal and reference the "Microsoft Advertising SDK", then include the following in the page code behind constructor.
	//Note - name of your control, this uses your XAML or programmatic instance
	AdRotatorControl.PlatformAdProviderComponents.Add(AdType.PubCenter, typeof(Microsoft.Advertising.WinRT.UI.AdControl));
	
For AdDuplex, add the control via NuGet using the AdDuplex SDK for Windows8 (Xaml edition) NuGet package, then include the following in the page code behind constructor.
	//Note - name of your control, this uses your XAML or programmatic instance
	AdRotatorControl.PlatformAdProviderComponents.Add(AdType.AdDuplex, typeof(AdDuplex.Controls.AdControl));

This enables AdRotator to access the local WinMD components for each control.
For further examples check the example projects in the GitHub source @ https://github.com/Adrotator/AdrotatorV2

***Note
This version of V2 also now supports our Unity plug-in for Windows 8 & Windows Phone 8 solutions.  Further details on the above site.

Final Note
----------
If you are using AdRotator and it is helping you to be more profitable, please consider donating to help support further development and support.
http://getadrotator.com/

The AdRotator Team
Simon Jackson @SimonDarksideJ (http://darkgenesis.zenithmoon.com)
Gergely Orosz @GergelyOrosz (http://gregdoesit.com)