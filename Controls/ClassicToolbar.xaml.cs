using System.Windows;
using System.Windows.Controls;

namespace ClassicAeroStudio.Controls
{
    public partial class ClassicToolbar : UserControl
    {
        public event RoutedEventHandler? TimeSettingsRequested;
        public event RoutedEventHandler? DeviceSettingsRequested;
        public event RoutedEventHandler? InputControlsRequested;
        public event RoutedEventHandler? StudioRequested;
        public event RoutedEventHandler? ProgressBarsRequested;
        public event RoutedEventHandler? SplitMenuRequested;
        public event RoutedEventHandler? AlertDialogRequested;
        public event RoutedEventHandler? SettingsRequested;
        public event RoutedEventHandler? TileSettingsRequested;
        public event RoutedEventHandler? RebootRequested;
        public event RoutedEventHandler? PingRequested;

        public ClassicToolbar()
        {
            InitializeComponent();
        }

        private void OnInputControlsClicked(object sender, RoutedEventArgs e)
        {
            InputControlsRequested?.Invoke(this, e);
        }

        private void OnProgressBarsClicked(object sender, RoutedEventArgs e)
        {
            ProgressBarsRequested?.Invoke(this, e);
        }

        private void OnSplitMenuClicked(object sender, RoutedEventArgs e)
        {
            SplitMenuRequested?.Invoke(this, e);
        }

        private void OnAlertDialogClicked(object sender, RoutedEventArgs e)
        {
            AlertDialogRequested?.Invoke(this, e);
        }

        private void OnStudioClicked(object sender, RoutedEventArgs e)
        {
            StudioRequested?.Invoke(this, e);
        }

        private void OnTimeSettingsClicked(object sender, RoutedEventArgs e)
        {
            TimeSettingsRequested?.Invoke(this, e);
        }

        private void OnDeviceSettingsClicked(object sender, RoutedEventArgs e)
        {
            DeviceSettingsRequested?.Invoke(this, e);
        }

        private void OnSettingsClicked(object sender, RoutedEventArgs e)
        {
            SettingsRequested?.Invoke(this, e);
        }

        private void OnTileSettingsClicked(object sender, RoutedEventArgs e)
        {
            TileSettingsRequested?.Invoke(this, e);
        }

        private void OnRebootClicked(object sender, RoutedEventArgs e)
        {
            RebootRequested?.Invoke(this, e);
        }

        private void OnPingClicked(object sender, RoutedEventArgs e)
        {
            PingRequested?.Invoke(this, e);
        }
    }
}
