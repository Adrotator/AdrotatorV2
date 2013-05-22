using Microsoft.Phone.Controls;

namespace AdRotator.Examples.WinPhone7
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.AdRotatorControl.Log += (s) => AdRotatorControl_Log(s);
            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AdRotatorControl_Log("Page Loaded");

        }

        void AdRotatorControl_Log(string message)
        {
            Dispatcher.BeginInvoke(() => MessagesListBox.Items.Insert(0, message));
        }
    }
}