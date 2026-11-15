using System.Windows;
using System.Windows.Controls;
using ClassicAeroStudio.Services;

namespace ClassicAeroStudio.Views
{
    public partial class SettingsWindow : Window
    {
        private bool _isInitialized = false;

        public SettingsWindow()
        {
            InitializeComponent();
            ThemeComboBox.SelectedIndex = ThemeManager.CurrentTheme == AppTheme.DarkAero ? 1 : 0;
            _isInitialized = true;
        }

        private void OnThemeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;

            if (ThemeComboBox.SelectedIndex == 1)
            {
                ThemeManager.ApplyTheme(AppTheme.DarkAero);
            }
            else
            {
                ThemeManager.ApplyTheme(AppTheme.ClassicAero);
            }
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
