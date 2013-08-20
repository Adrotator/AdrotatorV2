Welcome to AdRotator for XAML projects includng Windows 8 and Windows Phone Silverlight

Included in this package is:

* This Readme :D
* AdRotator DLL's
* Sample "DefaultAdSetings.XML" configuration file

**Note
We no longer include Ad Provider specific DLL's, you need to obtain the latest version from each provider, here is a list of Provider Dev pages:
(Except AdDuplex as they provide a NuGet package it is included by default)

* PubCenter - (Win8) http://go.microsoft.com/fwlink/p/?LinkID=285861 (WP) Inc with Windows Phone SDK
* Smaato - http://www.smaato.com/sdks/
* MobFox - (Contact Mobfox)
* AdMob - https://developers.google.com/mobile-ads-sdk/download
* Nokia Ad Exchange (NAX) / innerative - https://nax.nokia.com/iamp/nokia/publisher/dashboard#ui-tabs-5 (need to be logged in)
* Inmobi - http://www.inmobi.com/developers/download-center/

---

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

If add it manually on Windows Phone either set the build action of the config file to "Content" and use the configuration above 
or, set the build action to "Resource" and configure the "DefaultSettingsFileUri" to also include the project name space as follows:

	DefaultSettingsFileUri="/<project assembly name>;component/defaultAdSettings.xml"
 
Lastly configure the XML configuration file (optionally host it on the web to allow remote configuration) for your Ad Provider settings or alternatively set the providers in the XAML

For more instructions on how to implement this control and all the other configuration options checkout the AdRotator host site
http://getadrotator.codeplex.com


The AdRotator Team
Simon Jackson (http://darkgenesis.zenithmoon.com)
Gergely Orosz (http://gregdoesit.com)