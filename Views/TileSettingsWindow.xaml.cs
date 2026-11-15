using System;
using System.Windows;
using System.Windows.Controls;
using ClassicAeroStudio.Services;

namespace ClassicAeroStudio.Views
{
    public partial class TileSettingsWindow : Window
    {
        private bool _isInitialized = false;

        public TileSettingsWindow()
        {
            InitializeComponent();
            TileThemeComboBox.SelectedIndex = ThemeManager.CurrentTheme == AppTheme.DarkAero ? 1 : 0;
            _isInitialized = true;
            Loaded += TileSettingsWindow_Loaded;
        }

        private void OnTileThemeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isInitialized) return;

            if (TileThemeComboBox.SelectedIndex == 1)
            {
                ThemeManager.ApplyTheme(AppTheme.DarkAero);
            }
            else
            {
                ThemeManager.ApplyTheme(AppTheme.ClassicAero);
            }
        }

        private void TileSettingsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdjustWindowSizeToFitTiles();
        }

        /// <summary>
        /// Calculates the default window height to display exactly 5 tiles comfortably on launch,
        /// while enabling the vertical scrollbar for the remaining tiles. When resized/maximized by the user,
        /// the vertical scrollbar automatically vanishes.
        /// </summary>
        public void AdjustWindowSizeToFitTiles()
        {
            int tileCount = SidebarListBox != null ? SidebarListBox.Items.Count : 8;
            
            // Each square tile item is 68px tall + 6px vertical margin = 74px pitch
            const double tilePitch = 74.0;
            
            // Title banner (52px) + Bottom actions bar (42px) + Window chrome & borders (~38px) + ListBox vertical padding (12px)
            const double overhead = 144.0;
            
            // Default launch height opens with exactly 5 tiles visible
            int effectiveTilesToFit = Math.Min(tileCount, 5);
            double desiredHeight = (effectiveTilesToFit * tilePitch) + overhead;

            // Constrain against the current monitor's working area (excluding Windows taskbar)
            double maxScreenHeight = SystemParameters.WorkArea.Height > 0 
                ? SystemParameters.WorkArea.Height * 0.88 
                : 760.0;

            double finalHeight = Math.Max(MinHeight, Math.Min(desiredHeight, maxScreenHeight));
            this.Height = finalHeight;
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
