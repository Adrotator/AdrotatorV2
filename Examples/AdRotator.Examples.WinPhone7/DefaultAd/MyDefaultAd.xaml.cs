using System.Windows;
using System.Windows.Controls;

namespace AdRotator.Examples.WinPhone7.DefaultAd
{
    public partial class MyDefaultAd : UserControl
    {
        public MyDefaultAd()
        {
            InitializeComponent();
        }

        private void UserControl_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            // TODO: Add event handler implementation here.
            MessageBox.Show("You clicked My AD, Great Stuff :D");
        }
    }
}
