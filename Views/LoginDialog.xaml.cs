using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class LoginDialog : Window
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
