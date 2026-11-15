using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class DeviceSettingsWindow : Window
    {
        public DeviceSettingsWindow()
        {
            InitializeComponent();
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            AeroMessageBox.ShowSuccess(
                this,
                "Device configuration saved successfully to non-volatile memory (NVRAM).",
                "Configuration Saved",
                "Configuration Saved Successfully");
        }

        private void OnPingTestClicked(object sender, RoutedEventArgs e)
        {
            var pingDlg = new DiagnosticPingDialog("192.168.1.1")
            {
                Owner = this
            };
            pingDlg.ShowDialog();
        }
    }
}
