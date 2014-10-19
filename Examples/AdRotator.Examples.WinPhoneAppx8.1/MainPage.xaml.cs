using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AdRotator.Examples.WinPhoneAppx8._1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool AdRotatorHidden = true;
        AdRotator.AdRotatorControl myAdControl;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            MyAdRotatorControl.PlatformAdProviderComponents.Add(Model.AdType.PubCenter, typeof(Microsoft.Advertising.Mobile.UI.AdControl));
            MyAdRotatorControl.PlatformAdProviderComponents.Add(Model.AdType.AdDuplex, typeof(AdDuplex.Universal.Controls.WinPhone.XAML.AdControl));

            Loaded += MainPage_Loaded;
        }

        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            AdRotatorControl.Log += (s) => AdRotatorControl_Log(s);
            await AdRotatorControl_Log("Page Loaded");
            HideButton_Tapped(null, null);
        }

        async Task AdRotatorControl_Log(string message)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() => MessagesListBox.Items.Insert(0, message)));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void HideButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdRotatorHidden = !AdRotatorHidden;
            MyAdRotatorControl.Visibility = AdRotatorHidden ? Visibility.Collapsed : Visibility.Visible;
            HideButton.Content = AdRotatorHidden ? "UnHide AdRotator" : "Hide AdRotator";
        }

        void InitialiseAdRotatorProgramatically()
        {
            myAdControl = new AdRotatorControl(1);
            myAdControl.LocalSettingsLocation = "ProgramaticdefaultAdSettings.xml";
            //myAdControl.RemoteSettingsLocation = "http://adrotator.apphb.com/V2defaultAdSettings.xml";
            myAdControl.AdWidth = 400;
            myAdControl.AdHeight = 80;
            myAdControl.AutoStartAds = true;
            ProgramaticAdRotator.Children.Add(myAdControl);
            //AdRotatorControl.Log += (s) => { tracker.TrackEventAsync("AdRotator", "AdLogEvent", "an Ad", 0); tracker.TrackPageViewAsync("My API - Create", "api/view"); };
        }
    }
}
