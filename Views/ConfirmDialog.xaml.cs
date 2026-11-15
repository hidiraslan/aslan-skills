using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class ConfirmDialog : Window
    {
        public bool IsConfirmed { get; private set; }

        public ConfirmDialog()
        {
            InitializeComponent();
        }

        private void OnConfirmClicked(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            DialogResult = true;
            Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            DialogResult = false;
            Close();
        }
    }
}
