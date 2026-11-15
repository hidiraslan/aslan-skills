using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
