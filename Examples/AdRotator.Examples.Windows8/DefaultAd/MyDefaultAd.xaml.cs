using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AdRotator.Examples.Windows8.DefaultAd
{
    public sealed partial class MyDefaultAd : UserControl
    {
        public MyDefaultAd()
        {
            this.InitializeComponent();
        }

        private async void UserControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await new MessageDialog("Thank you for Clicking my Ad").ShowAsync();
        }
    }
}
