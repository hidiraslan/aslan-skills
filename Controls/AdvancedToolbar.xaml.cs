using System.Windows;
using System.Windows.Controls;

namespace ClassicAeroStudio.Controls
{
    public partial class AdvancedToolbar : UserControl
    {
        public event RoutedEventHandler? SettingsRequested;

        public AdvancedToolbar()
        {
            InitializeComponent();
        }

        private void OnSettingsClicked(object sender, RoutedEventArgs e)
        {
            SettingsRequested?.Invoke(this, e);
        }
    }
}
