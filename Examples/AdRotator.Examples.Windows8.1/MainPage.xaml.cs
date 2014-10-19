using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AdRotator.Examples.Windows8._1
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

            AdRotatorControl.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.PubCenter, typeof(Microsoft.Advertising.WinRT.UI.AdControl));
            AdRotatorControl.PlatformAdProviderComponents.Add(AdRotator.Model.AdType.AdDuplex, typeof(AdDuplex.Controls.AdControl));

            AdRotatorControl.Log += (s) => System.Diagnostics.Debug.WriteLine(s);
            Loaded += (s, e) => HideButton_Tapped(null,null);

            //InitialiseAdRotatorProgramatically();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Started");
        }

        private void HideButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AdRotatorHidden = !AdRotatorHidden;
            AdRotatorControl.Visibility = AdRotatorHidden ? Visibility.Collapsed : Visibility.Visible;
            HideButton.Content = AdRotatorHidden ? "UnHide AdRotator" : "Hide AdRotator";
        }

        void InitialiseAdRotatorProgramatically()
        {
            myAdControl = new AdRotatorControl();
            //myAdControl.LocalSettingsLocation = "defaultAdSettings.xml";
            myAdControl.RemoteSettingsLocation = "http://adrotator.apphb.com/V2defaultAdSettings.xml";
            myAdControl.AdWidth = 150;
            myAdControl.AdHeight = 150;
            myAdControl.AutoStartAds = true;
            ProgramaticAdRotator.Children.Add(myAdControl);
        }
    }
}
