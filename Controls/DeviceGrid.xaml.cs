using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ClassicAeroStudio.Models;
using ClassicAeroStudio.Views;

namespace ClassicAeroStudio.Controls
{
    public partial class DeviceGrid : UserControl
    {
        public static readonly DependencyProperty DevicesProperty =
            DependencyProperty.Register(
                nameof(Devices),
                typeof(ObservableCollection<DemoDevice>),
                typeof(DeviceGrid),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedDeviceProperty =
            DependencyProperty.Register(
                nameof(SelectedDevice),
                typeof(DemoDevice),
                typeof(DeviceGrid),
                new PropertyMetadata(null));

        public static readonly DependencyProperty AlarmsProperty =
            DependencyProperty.Register(
                nameof(Alarms),
                typeof(ObservableCollection<DemoAlarm>),
                typeof(DeviceGrid),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedAlarmProperty =
            DependencyProperty.Register(
                nameof(SelectedAlarm),
                typeof(DemoAlarm),
                typeof(DeviceGrid),
                new PropertyMetadata(null));

        public event RoutedEventHandler? DeviceSettingsRequested;
        public event RoutedEventHandler? TimeSettingsRequested;
        public event RoutedEventHandler? RebootRequested;
        public event RoutedEventHandler? PingRequested;

        public ObservableCollection<DemoDevice> Devices
        {
            get => (ObservableCollection<DemoDevice>)GetValue(DevicesProperty);
            set => SetValue(DevicesProperty, value);
        }

        public DemoDevice SelectedDevice
        {
            get => (DemoDevice)GetValue(SelectedDeviceProperty);
            set => SetValue(SelectedDeviceProperty, value);
        }

        public ObservableCollection<DemoAlarm> Alarms
        {
            get => (ObservableCollection<DemoAlarm>)GetValue(AlarmsProperty);
            set => SetValue(AlarmsProperty, value);
        }

        public DemoAlarm SelectedAlarm
        {
            get => (DemoAlarm)GetValue(SelectedAlarmProperty);
            set => SetValue(SelectedAlarmProperty, value);
        }

        private ObservableCollection<DemoAlarm> _allAlarms;

        public DeviceGrid()
        {
            InitializeComponent();
            _allAlarms = DemoAlarm.GetSampleAlarms();
            Alarms = new ObservableCollection<DemoAlarm>(_allAlarms);
            if (Alarms.Count > 0)
            {
                SelectedAlarm = Alarms[0];
            }
        }

        public void SelectTab(int index)
        {
            if (ViewTabStrip != null && index >= 0 && index < ViewTabStrip.Items.Count)
            {
                ViewTabStrip.SelectedIndex = index;
            }
        }

        private void OnViewTabStripSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DeviceListPanel == null || AlarmsPanel == null || WorkspacePanel == null) return;

            int index = ViewTabStrip?.SelectedIndex ?? 0;
            WorkspacePanel.Visibility = index == 0 ? Visibility.Visible : Visibility.Collapsed;
            if (AnalyticsPanel != null)
            {
                AnalyticsPanel.Visibility = index == 1 ? Visibility.Visible : Visibility.Collapsed;
            }
            if (IconsPanel != null)
            {
                IconsPanel.Visibility = index == 2 ? Visibility.Visible : Visibility.Collapsed;
            }
            DeviceListPanel.Visibility = index == 3 ? Visibility.Visible : Visibility.Collapsed;
            AlarmsPanel.Visibility = index == 4 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnExportAnalyticsClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            Views.AeroMessageBox.ShowSuccess(
                win,
                "All analytics metrics, charts, and KPI summary report successfully exported:\n\nFormat: Excel (.xlsx) / CSV / PDF Summary\nPeriod: Last 24 Hours (Enterprise Dashboard)",
                "Export Analytics & Charts",
                "Export Process Complete");
        }

        #region Device List Operations

        private void MainDataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dep = (DependencyObject)e.OriginalSource;
            while (dep != null && dep is not DataGridRow)
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep is DataGridRow row && row.Item is DemoDevice dev)
            {
                SelectedDevice = dev;
                row.IsSelected = true;
            }
        }

        private void OnConfigureDeviceClicked(object sender, RoutedEventArgs e)
        {
            DeviceSettingsRequested?.Invoke(this, e);
        }

        private void OnTimeSettingsClicked(object sender, RoutedEventArgs e)
        {
            TimeSettingsRequested?.Invoke(this, e);
        }

        private void OnPingDeviceClicked(object sender, RoutedEventArgs e)
        {
            PingRequested?.Invoke(this, e);
        }

        private void OnRebootDeviceClicked(object sender, RoutedEventArgs e)
        {
            RebootRequested?.Invoke(this, e);
        }

        private void OnCopyMacClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedDevice != null && !string.IsNullOrEmpty(SelectedDevice.MacAddress))
            {
                Clipboard.SetText(SelectedDevice.MacAddress);
            }
        }

        private void OnCopyIpClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedDevice != null && !string.IsNullOrEmpty(SelectedDevice.IPv4))
            {
                Clipboard.SetText(SelectedDevice.IPv4);
            }
        }

        private void OnDeviceRefreshClicked(object sender, RoutedEventArgs e)
        {
            Devices = DemoDevice.GetSampleDevices();
        }

        #endregion

        #region Alarm Operations

        private void AlarmsDataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dep = (DependencyObject)e.OriginalSource;
            while (dep != null && dep is not DataGridRow)
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep is DataGridRow row && row.Item is DemoAlarm alm)
            {
                SelectedAlarm = alm;
                row.IsSelected = true;
            }
        }

        private void OnAcknowledgeSelectedAlarmClicked(object sender, RoutedEventArgs e)
        {
            // Acknowledge checked alarms or selected alarm
            var checkedAlarms = Alarms.Where(a => a.IsSelected).ToList();
            if (checkedAlarms.Count > 0)
            {
                foreach (var alm in checkedAlarms)
                {
                    alm.IsAcknowledged = true;
                    alm.IsSelected = false;
                }
            }
            else if (SelectedAlarm != null)
            {
                SelectedAlarm.IsAcknowledged = true;
            }
            UpdateAlarmBadges();
        }

        private void OnClearResolvedAlarmsClicked(object sender, RoutedEventArgs e)
        {
            var resolved = Alarms.Where(a => a.IsAcknowledged).ToList();
            foreach (var r in resolved)
            {
                Alarms.Remove(r);
                _allAlarms.Remove(r);
            }
            UpdateAlarmBadges();
        }

        private void OnAlarmSeverityFilterChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlarmsFilterCombo == null || _allAlarms == null) return;

            switch (AlarmsFilterCombo.SelectedIndex)
            {
                case 1: // Critical Only
                    Alarms = new ObservableCollection<DemoAlarm>(_allAlarms.Where(a => a.Severity == AlarmSeverity.Critical));
                    break;
                case 2: // Warning Only
                    Alarms = new ObservableCollection<DemoAlarm>(_allAlarms.Where(a => a.Severity == AlarmSeverity.Warning));
                    break;
                case 3: // Unacknowledged Only
                    Alarms = new ObservableCollection<DemoAlarm>(_allAlarms.Where(a => !a.IsAcknowledged));
                    break;
                default: // All
                    Alarms = new ObservableCollection<DemoAlarm>(_allAlarms);
                    break;
            }
        }

        private void OnExportAlarmsClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            Views.AeroMessageBox.ShowSuccess(
                win,
                $"Active alarm logs exported successfully:\n\nFormat: CSV / Syslog Compatible\nEntries: {Alarms.Count}",
                "Export Alarm Logs",
                "Log Export Completed");
        }

        private void OnCopyAlarmDescClicked(object sender, RoutedEventArgs e)
        {
            if (SelectedAlarm != null && !string.IsNullOrEmpty(SelectedAlarm.Description))
            {
                Clipboard.SetText($"{SelectedAlarm.AlarmId} [{SelectedAlarm.SeverityText}] {SelectedAlarm.DeviceName}: {SelectedAlarm.Description}");
            }
        }

        private void UpdateAlarmBadges()
        {
            int crit = _allAlarms.Count(a => a.Severity == AlarmSeverity.Critical && !a.IsAcknowledged);
            int warn = _allAlarms.Count(a => a.Severity == AlarmSeverity.Warning && !a.IsAcknowledged);
            int info = _allAlarms.Count(a => a.Severity == AlarmSeverity.Information);

            if (CriticalBadgeText != null) CriticalBadgeText.Text = $"● {crit} Critical";
            if (WarningBadgeText != null) WarningBadgeText.Text = $"▲ {warn} Warning";
            if (InfoBadgeText != null) InfoBadgeText.Text = $"ℹ {info} Informational";

            if (AlarmsTabItem != null)
            {
                AlarmsTabItem.Header = $"Alarms ({crit + warn})";
            }
        }

        #endregion

        #region Workspace Operations

        private void OnAddWidgetClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var dlg = new DemoModalDialog { Owner = win };
            dlg.ShowDialog();
        }

        private void OnResetLayoutClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            Views.AeroMessageBox.ShowInfo(
                win,
                "Workspace layout reset to default Enterprise overview.",
                "Workspace Layout",
                "Layout Restored");
        }

        private void OnOpenLoginDialogClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var dlg = new Views.LoginDialog { Owner = win };
            dlg.ShowDialog();
        }

        private void OnOpenFilePickerDialogClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var dlg = new Views.FilePickerDialog { Owner = win };
            dlg.ShowDialog();
        }

        private void OnOpenModalDialogClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var dlg = new Views.DemoModalDialog { Owner = win };
            dlg.ShowDialog();
        }

        private void OnOpenSettingsDialogClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var dlg = new Views.SettingsWindow { Owner = win };
            dlg.ShowDialog();
        }

        private void OnOpenExplorerLayoutClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var dlg = new Views.ExplorerLayoutWindow { Owner = win };
            dlg.ShowDialog();
        }

        private void OnOpenMmcLayoutClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var dlg = new Views.MmcLayoutWindow { Owner = win };
            dlg.ShowDialog();
        }

        private void OnOpenStudioLayoutClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var dlg = new Views.StudioLayoutWindow { Owner = win };
            dlg.ShowDialog();
        }

        private void OnOpenInfoDialogClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            Views.AeroMessageBox.ShowInfo(
                win,
                "Device configuration saved successfully to non-volatile memory (NVRAM).\n\nPrimary Flash Bank 1 synchronized with checksum verification.",
                "Configuration Saved",
                "Device Configuration Saved Successfully");
        }

        private void OnOpenQuestionDialogClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            bool confirmed = Views.AeroMessageBox.ShowConfirm(
                win,
                "Do you want to deploy the pending VLAN and Trunking parameters to 'DGS-1210-20'?\n\nActive network ports may momentarily renegotiate link speed for 3-5 seconds.",
                "Confirm VLAN Deployment",
                "Apply Switch Configuration?");

            if (confirmed)
            {
                Views.AeroMessageBox.ShowSuccess(win, "Configuration parameters deployed to hardware ports successfully.", "Configuration Applied");
            }
        }

        private void OnOpenWarningDialogClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            Views.AeroMessageBox.Show(
                win,
                "Power supply unit 2 (PSU-2) internal sensor has exceeded 78°C. Ensure cooling fans are unobstructed or reduce PoE power load immediately!",
                "Critical Thermal Warning",
                "Hardware Thermal Threshold Exceeded",
                Views.AeroMessageBoxButton.Ok,
                Views.AeroMessageBoxIcon.Warning);
        }

        private void OnOpenProgressDialogClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            var progressDlg = new Views.AeroProgressDialog(
                "Device Configuration Sync",
                "Writing Configuration to Device...",
                "Please wait while settings are serialized and committed to NVRAM.")
            {
                Owner = win
            };
            progressDlg.ShowDialog();
        }

        private void OnSplitDiagnosticsActionClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            Views.AeroMessageBox.ShowSuccess(win, "Quick Network Diagnostics Started:\nSending ICMP echo ping requests across 192.168.1.1/24 subnet.", "Diagnostic Ping Started");
        }

        private void OnSplitDiagnosticsArrowClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                btn.ContextMenu.IsOpen = true;
            }
        }

        private void OnDiagnosticOptionClicked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                var win = Window.GetWindow(this);
                Views.AeroMessageBox.Show(win, $"Executing diagnostic profile:\n{item.Header}", "Diagnostic Mode", "Advanced Diagnostic Profile", Views.AeroMessageBoxButton.Ok, Views.AeroMessageBoxIcon.Information);
            }
        }

        private void OnDatabaseBackupButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                btn.ContextMenu.IsOpen = true;
            }
        }

        private void OnBackupOptionClicked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                var win = Window.GetWindow(this);
                Views.AeroMessageBox.ShowSuccess(win, $"Backup operation dispatched:\n{item.Header}", "Database Backup");
            }
        }

        private void OnAppPillButtonClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.ContextMenu != null)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                btn.ContextMenu.IsOpen = true;
            }
        }

        private void OnAppPillOptionClicked(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                var win = Window.GetWindow(this);
                Views.AeroMessageBox.Show(win, $"Menu action executed:\n{item.Header}", "System Menu", "Application Action", Views.AeroMessageBoxButton.Ok, Views.AeroMessageBoxIcon.Information);
            }
        }

        private void OnCommandLinkClicked(object sender, RoutedEventArgs e)
        {
            var win = Window.GetWindow(this);
            Views.AeroMessageBox.ShowSuccess(win, "Command link invoked and queued for execution.", "Command Link Action");
        }

        private void OnDismissNotificationClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.TemplatedParent == null)
            {
                var parent = VisualTreeHelper.GetParent(btn);
                while (parent != null && parent is not Border)
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }
                if (parent is Border b)
                {
                    b.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void OnNumericUpClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is TextBox tb && int.TryParse(tb.Text, out int val))
            {
                tb.Text = (val + 1).ToString();
            }
        }

        private void OnNumericDownClicked(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is TextBox tb && int.TryParse(tb.Text, out int val) && val > 0)
            {
                tb.Text = (val - 1).ToString();
            }
        }

        private void OnSelectAllHeaderClicked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox chk && Devices != null)
            {
                bool isChecked = chk.IsChecked ?? false;
                foreach (var device in Devices)
                {
                    device.IsSelected = isChecked;
                }
            }
        }

        private void OnScrollViewerRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            // Prevent WPF from horizontally jumping/shifting the scroll viewer when child elements gain focus or expand
            e.Handled = true;
        }

        #endregion
    }
}
