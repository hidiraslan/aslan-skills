using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class DemoModalDialog : Window
    {
        public DemoModalDialog()
        {
            InitializeComponent();
        }

        private void OnMtuUpClicked(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MtuBox.Text, out int val) && val < 9216)
            {
                MtuBox.Text = (val + 100).ToString();
            }
        }

        private void OnMtuDownClicked(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(MtuBox.Text, out int val) && val > 64)
            {
                MtuBox.Text = (val - 100).ToString();
            }
        }

        private void OnApplyClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
