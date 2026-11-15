using System;
using System.Windows;
using ClassicAeroStudio.Models;

namespace ClassicAeroStudio.Views
{
    public partial class MainWindow : Window
    {
        private DateTime _menuClosedTimestamp = DateTime.MinValue;

        public MainWindow()
        {
            InitializeComponent();

            // Hook ContextMenu Closed event to support clean toggle closing
            if (AppMenuButton.ContextMenu != null)
            {
                AppMenuButton.ContextMenu.Closed += (s, e) =>
                {
                    _menuClosedTimestamp = DateTime.UtcNow;
                };
            }

            // Pre-warm the ContextMenu visual tree asynchronously right after window loads
            // to completely eliminate the first-click delay / latency.
            Loaded += (s, e) =>
            {
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                {
                    if (AppMenuButton.ContextMenu != null)
                    {
                        AppMenuButton.ContextMenu.ApplyTemplate();
                        foreach (var item in AppMenuButton.ContextMenu.Items)
                        {
                            if (item is FrameworkElement fe)
                            {
                                fe.ApplyTemplate();
                            }
                        }
                    }
                }));
            };

            // Populate static demo devices matching screenshot 1
            var devices = DemoDevice.GetSampleDevices();
            DeviceGridArea.Devices = devices;
            if (devices.Count > 0)
            {
                DeviceGridArea.SelectedDevice = devices[0];
            }

            // Sync Ribbon Theme Pill Switch with initial and dynamic theme changes
            UpdateThemePillState(ClassicAeroStudio.Services.ThemeManager.CurrentTheme);
            ClassicAeroStudio.Services.ThemeManager.ThemeChanged += UpdateThemePillState;
        }

        private void UpdateThemePillState(ClassicAeroStudio.Services.AppTheme theme)
        {
            if (ThemePillSwitch == null) return;
            bool isDark = (theme == ClassicAeroStudio.Services.AppTheme.DarkAero);
            ThemePillSwitch.ToolTip = isDark
                ? "Active: Dark Aero Mode (Click to switch to Classic Light Aero)"
                : "Active: Classic Light Aero (Click to switch to Dark Aero)";
        }

        private void OnThemePillSwitchClicked(object sender, RoutedEventArgs e)
        {
            var nextTheme = ClassicAeroStudio.Services.ThemeManager.CurrentTheme == ClassicAeroStudio.Services.AppTheme.DarkAero
                ? ClassicAeroStudio.Services.AppTheme.ClassicAero
                : ClassicAeroStudio.Services.AppTheme.DarkAero;
            ClassicAeroStudio.Services.ThemeManager.ApplyTheme(nextTheme);
        }

        private void OnInputControlsRequested(object sender, RoutedEventArgs e)
        {
            var loginDlg = new LoginDialog
            {
                Owner = this
            };
            loginDlg.ShowDialog();
        }

        private void OnProgressBarsRequested(object sender, RoutedEventArgs e)
        {
            var progressDlg = new AeroProgressDialog(
                title: "Aero Progress Indicator",
                header: "Processing Component State & Serialization...",
                message: "Demonstrating Classic Windows Aero smooth progress bar with manual completion verification.")
            {
                Owner = this
            };
            progressDlg.ShowDialog();
        }

        private void OnSplitMenuRequested(object sender, RoutedEventArgs e)
        {
            var picker = new FilePickerDialog
            {
                Owner = this
            };
            picker.ShowDialog();
        }

        private void OnAlertDialogRequested(object sender, RoutedEventArgs e)
        {
            var modal = new DemoModalDialog
            {
                Owner = this
            };
            modal.ShowDialog();
        }

        private void OnTimeSettingsRequested(object sender, RoutedEventArgs e)
        {
            var timeSettingsWindow = new TimeSettingsWindow
            {
                Owner = this
            };
            timeSettingsWindow.ShowDialog();
        }

        private void OnDeviceSettingsRequested(object sender, RoutedEventArgs e)
        {
            var deviceSettingsWindow = new DeviceSettingsWindow
            {
                Owner = this
            };
            deviceSettingsWindow.ShowDialog();
        }

        private void OnSettingsRequested(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow
            {
                Owner = this
            };
            settingsWindow.ShowDialog();
        }

        private void OnTileSettingsRequested(object sender, RoutedEventArgs e)
        {
            var tileSettingsWindow = new TileSettingsWindow
            {
                Owner = this
            };
            tileSettingsWindow.ShowDialog();
        }

        private void OnStudioRequested(object sender, RoutedEventArgs e)
        {
            var studioWin = new StudioLayoutWindow
            {
                Owner = this
            };
            studioWin.ShowDialog();
        }

        private void OnRebootRequested(object sender, RoutedEventArgs e)
        {
            var confirmDlg = new ConfirmDialog
            {
                Owner = this
            };
            confirmDlg.ShowDialog();
        }

        private void OnPingRequested(object sender, RoutedEventArgs e)
        {
            var targetIp = DeviceGridArea.SelectedDevice?.IPv4 ?? "192.168.1.20";
            var pingDlg = new DiagnosticPingDialog(targetIp)
            {
                Owner = this
            };
            pingDlg.ShowDialog();
        }

        private void OnAppMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            if (AppMenuButton.ContextMenu != null)
            {
                // If menu was just closed by clicking on this button (within 250ms),
                // the user intended to toggle it closed. Do not re-open.
                if ((DateTime.UtcNow - _menuClosedTimestamp).TotalMilliseconds < 250)
                {
                    return;
                }

                AppMenuButton.ContextMenu.PlacementTarget = AppMenuButton;
                AppMenuButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                AppMenuButton.ContextMenu.IsOpen = true;
            }
        }

        private void OnRibbonTabSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CommonToolbar == null || AdvancedToolbar == null) return;

            if (RibbonTabStrip.SelectedIndex == 1)
            {
                CommonToolbar.Visibility = Visibility.Collapsed;
                AdvancedToolbar.Visibility = Visibility.Visible;
            }
            else
            {
                CommonToolbar.Visibility = Visibility.Visible;
                AdvancedToolbar.Visibility = Visibility.Collapsed;
            }
        }

        private void OnAboutClicked(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow
            {
                Owner = this
            };
            about.ShowDialog();
        }

        private void OnNewScanClicked(object sender, RoutedEventArgs e)
        {
            // Visual scan trigger simulation
            DeviceGridArea.Devices = DemoDevice.GetSampleDevices();
        }

        private void OnOpenTopologyClicked(object sender, RoutedEventArgs e)
        {
            DeviceGridArea.SelectTab(2);
        }

        private void OnSaveDeviceListClicked(object sender, RoutedEventArgs e)
        {
        }

        private void OnExportClicked(object sender, RoutedEventArgs e)
        {
        }

        private void OnPrintClicked(object sender, RoutedEventArgs e)
        {
        }

        private void OnExitClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
