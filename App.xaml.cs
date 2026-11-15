using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ClassicAeroStudio.Views;

namespace ClassicAeroStudio
{
    public partial class App : Application
    {
        private static readonly System.Reflection.PropertyInfo? MenuBaseIsMenuModeProperty =
            typeof(System.Windows.Controls.Primitives.MenuBase)
                .GetProperty("IsMenuMode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += (s, args) =>
            {
                File.WriteAllText("crash.log", args.ExceptionObject.ToString());
            };
            DispatcherUnhandledException += (s, args) =>
            {
                File.WriteAllText("crash.log", args.Exception.ToString());
            };

            // Global Menu Rule 1: Reset MenuMode on mouse move over Menu bars (NOT ContextMenu)
            // This guarantees OpenOnMouseOver is false, so hovering over sibling MenuItems will NEVER auto-open them.
            EventManager.RegisterClassHandler(
                typeof(Menu),
                UIElement.PreviewMouseMoveEvent,
                new MouseEventHandler((s, args) =>
                {
                    if (s is Menu menu)
                    {
                        MenuBaseIsMenuModeProperty?.SetValue(menu, false);
                    }
                }));

            // Global Menu Rule 2: When any top-level MenuItem in a Menu bar opens, close any sibling items and reset IsMenuMode
            EventManager.RegisterClassHandler(
                typeof(MenuItem),
                MenuItem.SubmenuOpenedEvent,
                new RoutedEventHandler((s, args) =>
                {
                    if (s is MenuItem mi && (mi.Role == MenuItemRole.TopLevelHeader || mi.Role == MenuItemRole.TopLevelItem))
                    {
                        ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(mi);
                        if (parent is Menu menu)
                        {
                            foreach (var item in menu.Items)
                            {
                                MenuItem? sibling = menu.ItemContainerGenerator.ContainerFromItem(item) as MenuItem ?? item as MenuItem;
                                if (sibling != null && sibling != mi && sibling.IsSubmenuOpen)
                                {
                                    sibling.IsSubmenuOpen = false;
                                }
                            }

                            MenuBaseIsMenuModeProperty?.SetValue(menu, false);
                        }
                    }
                }));

            // Global Menu Rule 3: Prevent top-level MenuItems in Menu bars from automatically opening on mouse hover
            EventManager.RegisterClassHandler(
                typeof(MenuItem),
                UIElement.MouseEnterEvent,
                new MouseEventHandler((s, args) =>
                {
                    if (s is MenuItem mi && (mi.Role == MenuItemRole.TopLevelHeader || mi.Role == MenuItemRole.TopLevelItem))
                    {
                        ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(mi);
                        if (parent is Menu menu)
                        {
                            MenuBaseIsMenuModeProperty?.SetValue(menu, false);
                            if (mi.IsSubmenuOpen && Mouse.LeftButton != MouseButtonState.Pressed)
                            {
                                mi.IsSubmenuOpen = false;
                            }
                        }
                    }
                }));

            // When an item inside a menu is clicked, close the parent top-level menu
            EventManager.RegisterClassHandler(
                typeof(MenuItem),
                MenuItem.ClickEvent,
                new RoutedEventHandler((s, args) =>
                {
                    if (s is MenuItem mi && mi.Role != MenuItemRole.TopLevelHeader && mi.Role != MenuItemRole.TopLevelItem)
                    {
                        DependencyObject current = mi;
                        while (current != null)
                        {
                            if (current is MenuItem ancestor && ancestor.Role == MenuItemRole.TopLevelHeader)
                            {
                                ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(ancestor);
                                if (parent is Menu)
                                {
                                    ancestor.IsSubmenuOpen = false;
                                }
                                break;
                            }
                            current = LogicalTreeHelper.GetParent(current) ?? VisualTreeHelper.GetParent(current);
                        }
                    }
                }));

            // Global TreeView Rule: Prevent unwanted horizontal scrolling / jumping when TreeViewItems are focused or selected
            EventManager.RegisterClassHandler(
                typeof(TreeViewItem),
                FrameworkElement.RequestBringIntoViewEvent,
                new RequestBringIntoViewEventHandler((s, args) =>
                {
                    args.Handled = true;
                }));

            // Global ScrollViewer Rule: Intelligent Mouse Wheel Mapping
            // If vertical scroll is not needed/available (ScrollableHeight == 0) but horizontal scroll IS needed (ScrollableWidth > 0),
            // OR if the user holds Shift, map mouse wheel scrolling to smooth horizontal scrolling without vertical conflicts.
            EventManager.RegisterClassHandler(
                typeof(ScrollViewer),
                UIElement.PreviewMouseWheelEvent,
                new MouseWheelEventHandler((sender, args) =>
                {
                    if (sender is not ScrollViewer sv) return;

                    // Only handle on the innermost ScrollViewer under the cursor
                    if (args.OriginalSource is DependencyObject source)
                    {
                        var targetSv = source as ScrollViewer ?? FindVisualParent<ScrollViewer>(source);
                        if (targetSv != null && targetSv != sv)
                        {
                            return; // Let event tunnel to inner ScrollViewer
                        }
                    }

                    bool isShift = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
                    bool canScrollVertically = sv.VerticalScrollBarVisibility != ScrollBarVisibility.Disabled 
                                               && sv.ScrollableHeight > 0;
                    bool canScrollHorizontally = sv.HorizontalScrollBarVisibility != ScrollBarVisibility.Disabled 
                                                 && sv.ScrollableWidth > 0;

                    if (isShift || (!canScrollVertically && canScrollHorizontally))
                    {
                        if (canScrollHorizontally)
                        {
                            double step = 48.0 * (Math.Abs(args.Delta) / 120.0);
                            if (step < 16.0) step = 16.0;

                            if (args.Delta < 0)
                            {
                                sv.ScrollToHorizontalOffset(Math.Min(sv.ScrollableWidth, sv.HorizontalOffset + step));
                            }
                            else
                            {
                                sv.ScrollToHorizontalOffset(Math.Max(0, sv.HorizontalOffset - step));
                            }

                            args.Handled = true;
                        }
                    }
                }));

            // Global Window Rule: Apply Immersive Dark Titlebar and ensure Application Icon
            EventManager.RegisterClassHandler(
                typeof(Window),
                FrameworkElement.LoadedEvent,
                new RoutedEventHandler((s, args) =>
                {
                    if (s is Window w)
                    {
                        if (w.Icon == null)
                        {
                            try
                            {
                                w.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Assets/app_icon.ico", UriKind.RelativeOrAbsolute));
                            }
                            catch
                            {
                                try
                                {
                                    w.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/Assets/app_icon.png", UriKind.RelativeOrAbsolute));
                                }
                                catch { }
                            }
                        }

                        ClassicAeroStudio.Services.ThemeManager.ApplyWindowTheme(
                            w,
                            ClassicAeroStudio.Services.ThemeManager.CurrentTheme == ClassicAeroStudio.Services.AppTheme.DarkAero);
                    }
                }));

            try
            {
                if (e.Args.Length > 0 && e.Args[0] == "--screenshot")
                {
                    // Render MainWindow (Common Tab)
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main.png");

                    // Open Application Menu and capture
                    var menu = mainWindow.AppMenuButton.ContextMenu;
                    if (menu != null)
                    {
                        menu.PlacementTarget = mainWindow.AppMenuButton;
                        menu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                        menu.IsOpen = true;
                        DoEvents();
                        menu.UpdateLayout();
                        int mw = (int)Math.Max(menu.ActualWidth, 240);
                        int mh = (int)Math.Max(menu.ActualHeight, 260);
                        SaveVisualAsPng(menu, mw, mh, "screenshot_app_menu.png");
                        menu.IsOpen = false;
                        DoEvents();
                    }

                    // Open Device List Right-Click Context Menu and capture (Device List is Index 3)
                    mainWindow.DeviceGridArea.SelectTab(3);
                    DoEvents();
                    var deviceMenu = mainWindow.DeviceGridArea.MainDataGrid.ContextMenu;
                    if (deviceMenu != null)
                    {
                        deviceMenu.PlacementTarget = mainWindow.DeviceGridArea.MainDataGrid;
                        deviceMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
                        deviceMenu.IsOpen = true;
                        DoEvents();
                        deviceMenu.UpdateLayout();
                        int dmw = (int)Math.Max(deviceMenu.ActualWidth, 240);
                        int dmh = (int)Math.Max(deviceMenu.ActualHeight, 360);
                        SaveVisualAsPng(deviceMenu, dmw, dmh, "screenshot_device_context_menu.png");
                        deviceMenu.IsOpen = false;
                        DoEvents();
                    }

                    // Switch to Alarms Tab (Index 4) and capture
                    mainWindow.DeviceGridArea.SelectTab(4);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_alarms.png");

                    // Test Responsive behavior at narrower window width (e.g. 740px)
                    mainWindow.Width = 740;
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_alarms_responsive.png");
                    mainWindow.Width = 1100;
                    DoEvents();

                    // Switch to Workspace / Topology Tab (Index 0) and capture
                    mainWindow.DeviceGridArea.SelectTab(0);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace.png");

                    // Scroll down into the component catalog and capture
                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(340);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace_dialogs_launcher.png");

                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(570);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace_buttons.png");

                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(690);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace_buttons2.png");

                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(950);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace_catalog.png");

                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(720);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace_catalog2.png");

                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(1200);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace_progressbars.png");

                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(1350);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace_forms.png");

                    // Test DatePicker Dropdown Open in Light Theme
                    if (mainWindow.DeviceGridArea.DemoDatePicker != null)
                    {
                        mainWindow.DeviceGridArea.DemoDatePicker.IsDropDownOpen = true;
                        DoEvents();
                        var popup = mainWindow.DeviceGridArea.DemoDatePicker.Template.FindName("PART_Popup", mainWindow.DeviceGridArea.DemoDatePicker) as System.Windows.Controls.Primitives.Popup;
                        if (popup != null && popup.Child is Visual childVisual)
                        {
                            SaveVisualAsPng(childVisual, 200, 180, "screenshot_datepicker_dropdown_light.png");
                        }
                        mainWindow.DeviceGridArea.DemoDatePicker.IsDropDownOpen = false;
                        DoEvents();
                    }

                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(1850);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_workspace_lists.png");

                    mainWindow.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(0);
                    DoEvents();

                    // Switch to Analytics & Charts Tab (Index 1) and capture
                    mainWindow.DeviceGridArea.SelectTab(1);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_analytics.png");

                    // Scroll to Compact Aero Bar Chart (GroupBox 2b) and Donut comparison
                    mainWindow.DeviceGridArea.AnalyticsScrollViewer.ScrollToVerticalOffset(180);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_analytics_compact_bars.png");

                    // Scroll down to middle section (Heatmap, Timeline, Funnel, Treemap, Waterfall)
                    mainWindow.DeviceGridArea.AnalyticsScrollViewer.ScrollToVerticalOffset(440);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_analytics_scroll1.png");

                    // Scroll down to bottom section (Data table + sparklines)
                    mainWindow.DeviceGridArea.AnalyticsScrollViewer.ScrollToVerticalOffset(820);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_analytics_scroll2.png");

                    mainWindow.DeviceGridArea.AnalyticsScrollViewer.ScrollToVerticalOffset(0);
                    DoEvents();

                    // Switch to Icons Gallery Tab (Index 2) and capture
                    mainWindow.DeviceGridArea.SelectTab(2);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_icons.png");

                    // Scroll down to group 2 and 3 in Icons gallery
                    mainWindow.DeviceGridArea.IconsScrollViewer.ScrollToVerticalOffset(380);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_icons_scroll1.png");

                    // Scroll to Key and Security group
                    mainWindow.DeviceGridArea.IconsScrollViewer.ScrollToVerticalOffset(850);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_icons_key.png");

                    // Scroll to Group 4 (Hardware & Status Monitors)
                    mainWindow.DeviceGridArea.IconsScrollViewer.ScrollToVerticalOffset(1250);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_icons_hardware.png");

                    // Scroll to Group 5 (Programming Icons)
                    mainWindow.DeviceGridArea.IconsScrollViewer.ScrollToVerticalOffset(1650);
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_icons_programming.png");

                    // Scroll down to Group 6 (User, Auth & Session Exit Icons)
                    mainWindow.DeviceGridArea.IconsScrollViewer.ScrollToBottom();
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_icons_auth.png");

                    mainWindow.DeviceGridArea.IconsScrollViewer.ScrollToVerticalOffset(0);
                    DoEvents();

                    // Switch back to Device List tab
                    mainWindow.DeviceGridArea.SelectTab(0);
                    DoEvents();

                    // Switch to Advanced Tab and capture
                    mainWindow.RibbonTabStrip.SelectedIndex = 1;
                    DoEvents();
                    SaveWindowAsPng(mainWindow, "screenshot_main_advanced.png");
                    mainWindow.Hide();

                    // Render LoginDialog
                    var loginDlg = new LoginDialog();
                    loginDlg.Show();
                    DoEvents();
                    SaveWindowAsPng(loginDlg, "screenshot_login_dialog.png");
                    loginDlg.Hide();

                    // Render FilePickerDialog
                    var fileDlg = new FilePickerDialog();
                    fileDlg.Show();
                    DoEvents();
                    SaveWindowAsPng(fileDlg, "screenshot_file_picker_dialog.png");
                    fileDlg.Hide();

                    // Render DemoModalDialog
                    var modalDlg = new DemoModalDialog();
                    modalDlg.Show();
                    DoEvents();
                    SaveWindowAsPng(modalDlg, "screenshot_demo_modal_dialog.png");
                    modalDlg.Hide();

                    // Render AboutWindow
                    var aboutWin = new AboutWindow();
                    aboutWin.Show();
                    DoEvents();
                    SaveWindowAsPng(aboutWin, "screenshot_about.png");
                    aboutWin.Hide();

                    // Render TimeSettingsWindow (Step 1)
                    var timeSettings = new TimeSettingsWindow();
                    timeSettings.Show();
                    timeSettings.ShowStep(1);
                    DoEvents();
                    SaveWindowAsPng(timeSettings, "screenshot_time_settings.png");
                    SaveWindowAsPng(timeSettings, "screenshot_time_settings_step1.png");

                    // Render TimeSettingsWindow (Step 2: Confirm Summary)
                    timeSettings.ShowStep(2);
                    DoEvents();
                    SaveWindowAsPng(timeSettings, "screenshot_time_settings_step2.png");
                    timeSettings.Hide();

                    // Render DiagnosticPingDialog
                    var pingDlg = new DiagnosticPingDialog("192.168.1.20");
                    pingDlg.Show();
                    DoEvents();
                    SaveWindowAsPng(pingDlg, "screenshot_ping_dialog.png");

                    // Test MinWidth resizing
                    pingDlg.Width = pingDlg.MinWidth;
                    DoEvents();
                    SaveWindowAsPng(pingDlg, "screenshot_ping_dialog_min_width.png");
                    pingDlg.Hide();

                    // Render ConfirmDialog (TaskDialog style Reboot confirmation)
                    var confirmDlg = new ConfirmDialog();
                    confirmDlg.Show();
                    DoEvents();
                    SaveWindowAsPng(confirmDlg, "screenshot_confirm_dialog.png");
                    confirmDlg.Hide();

                    // Render AeroMessageBox (Info / Success)
                    var msgBoxInfo = new AeroMessageBox { Title = "Configuration Saved" };
                    msgBoxInfo.TxtMainInstruction.Text = "Device Configuration Saved Successfully";
                    msgBoxInfo.TxtContent.Text = "Device configuration saved successfully to non-volatile memory (NVRAM).\nPrimary Flash Bank 1 synchronized with checksum verification.";
                    msgBoxInfo.IconRect.Fill = (DrawingBrush)msgBoxInfo.FindResource("IconAlarmSuccess");
                    msgBoxInfo.Btn1.Content = "Tamam";
                    msgBoxInfo.Btn1.Margin = new Thickness(0);
                    msgBoxInfo.Btn2.Visibility = Visibility.Collapsed;
                    msgBoxInfo.Btn3.Visibility = Visibility.Collapsed;
                    msgBoxInfo.Show();
                    DoEvents();
                    SaveWindowAsPng(msgBoxInfo, "screenshot_messagebox_info.png");
                    msgBoxInfo.Hide();

                    // Render AeroMessageBox (Confirm / Two Buttons)
                    var msgBoxConfirm = new AeroMessageBox { Title = "Confirm VLAN Deployment" };
                    msgBoxConfirm.TxtMainInstruction.Text = "Apply Switch Configuration?";
                    msgBoxConfirm.TxtContent.Text = "Do you want to deploy the pending VLAN and Trunking parameters to 'DGS-1210-20'?\nActive network ports may momentarily renegotiate link speed for 3-5 seconds.";
                    msgBoxConfirm.IconRect.Fill = (DrawingBrush)msgBoxConfirm.FindResource("IconAlarmQuestion");
                    msgBoxConfirm.Btn1.Content = "Evet";
                    msgBoxConfirm.Btn1.Margin = new Thickness(0, 0, 8, 0);
                    msgBoxConfirm.Btn2.Content = "Hayır";
                    msgBoxConfirm.Btn2.Visibility = Visibility.Visible;
                    msgBoxConfirm.Btn2.Margin = new Thickness(0);
                    msgBoxConfirm.Btn3.Visibility = Visibility.Collapsed;
                    msgBoxConfirm.Show();
                    DoEvents();
                    SaveWindowAsPng(msgBoxConfirm, "screenshot_messagebox_confirm.png");
                    msgBoxConfirm.Hide();

                    // Render AeroMessageBox (Warning / Thermal Alert)
                    var msgBoxWarning = new AeroMessageBox { Title = "Critical Thermal Warning" };
                    msgBoxWarning.TxtMainInstruction.Text = "Hardware Thermal Threshold Exceeded";
                    msgBoxWarning.TxtContent.Text = "Power supply unit 2 (PSU-2) internal sensor has exceeded 78°C. Ensure cooling fans are unobstructed or reduce PoE power load immediately!";
                    msgBoxWarning.IconRect.Fill = (DrawingBrush)msgBoxWarning.FindResource("IconAlarmWarning");
                    msgBoxWarning.Btn1.Content = "Tamam";
                    msgBoxWarning.Btn1.Margin = new Thickness(0);
                    msgBoxWarning.Btn2.Visibility = Visibility.Collapsed;
                    msgBoxWarning.Btn3.Visibility = Visibility.Collapsed;
                    msgBoxWarning.Show();
                    DoEvents();
                    SaveWindowAsPng(msgBoxWarning, "screenshot_messagebox_warning.png");
                    msgBoxWarning.Hide();

                    // Render AeroProgressDialog
                    var progressDlg = new AeroProgressDialog("Device Configuration Sync", "Writing Configuration to Device...", "Please wait while settings are serialized and committed to NVRAM.");
                    progressDlg.SetProgress(62, "Writing sector block 25 of 40 (checksum ok)...");
                    progressDlg.Show();
                    DoEvents();
                    SaveWindowAsPng(progressDlg, "screenshot_progress_dialog.png");
                    progressDlg.Hide();

                    // Render DeviceSettingsWindow (Tab 1: System & General)
                    var deviceSettings = new DeviceSettingsWindow();
                    deviceSettings.Show();
                    deviceSettings.SettingsTabControl.SelectedIndex = 0;
                    DoEvents();
                    SaveWindowAsPng(deviceSettings, "screenshot_device_settings.png");

                    // Render DeviceSettingsWindow (Tab 2: Network & IP - RadioButtons & ComboBoxes)
                    deviceSettings.SettingsTabControl.SelectedIndex = 1;
                    DoEvents();
                    SaveWindowAsPng(deviceSettings, "screenshot_device_settings_tab2.png");

                    // Render DeviceSettingsWindow (Tab 3: Port Configuration - TreeView & Stretched ComboBox Form)
                    deviceSettings.SettingsTabControl.SelectedIndex = 2;
                    DoEvents();
                    SaveWindowAsPng(deviceSettings, "screenshot_device_settings_tab3.png");

                    // Render DeviceSettingsWindow (Tab 4: Resources & Diagnostics - ProgressBars, Sliders, Buttons)
                    deviceSettings.SettingsTabControl.SelectedIndex = 3;
                    DoEvents();
                    SaveWindowAsPng(deviceSettings, "screenshot_device_settings_tab4.png");

                    deviceSettings.Hide();

                    // Render SettingsWindow (Ayarlar Window with Left Sidebar)
                    var settingsWin = new SettingsWindow();
                    settingsWin.Show();
                    DoEvents();
                    SaveWindowAsPng(settingsWin, "screenshot_settings.png");
                    settingsWin.Hide();

                    // Render TileSettingsWindow (Ayarlar Window with Square Tile Navigation Sidebar)
                    var tileSettingsWin = new TileSettingsWindow();
                    tileSettingsWin.Show();
                    DoEvents();
                    SaveWindowAsPng(tileSettingsWin, "screenshot_tile_settings.png");

                    // Test narrowed width to confirm no horizontal scrollbar
                    tileSettingsWin.Width = 720;
                    DoEvents();
                    SaveWindowAsPng(tileSettingsWin, "screenshot_tile_settings_narrow.png");
                    tileSettingsWin.Hide();

                    // Render ExplorerLayoutWindow (Classic Windows Explorer Shell)
                    var explorerWin = new ExplorerLayoutWindow();
                    explorerWin.Show();
                    DoEvents();
                    SaveWindowAsPng(explorerWin, "screenshot_layout_explorer.png");
                    explorerWin.Hide();

                    // Render MmcLayoutWindow (MMC Admin Console Shell)
                    var mmcWin = new MmcLayoutWindow();
                    mmcWin.Show();
                    DoEvents();
                    SaveWindowAsPng(mmcWin, "screenshot_layout_mmc.png");
                    mmcWin.Hide();

                    // Render StudioLayoutWindow (Multi-Document Studio Shell)
                    var studioWin = new StudioLayoutWindow();
                    studioWin.Show();
                    DoEvents();
                    SaveWindowAsPng(studioWin, "screenshot_layout_studio.png");
                    studioWin.Hide();

                    // Render Dark Aero Theme Screenshots
                    ClassicAeroStudio.Services.ThemeManager.ApplyTheme(ClassicAeroStudio.Services.AppTheme.DarkAero);
                    DoEvents();

                    var darkMain = new MainWindow();
                    darkMain.Show();
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_main_dark.png");

                    // Tab 4: Alarms
                    darkMain.DeviceGridArea.SelectTab(4);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_alarms_dark.png");

                    // Tab 0: Workspace / Topology
                    darkMain.DeviceGridArea.SelectTab(0);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_workspace_dark.png");

                    darkMain.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(340);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_workspace_dialogs_dark.png");

                    darkMain.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(570);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_workspace_buttons_dark.png");

                    darkMain.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(950);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_workspace_catalog_dark.png");

                    darkMain.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(1350);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_workspace_forms_dark.png");

                    darkMain.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(1850);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_workspace_lists_dark.png");

                    darkMain.DeviceGridArea.WorkspaceScrollViewer.ScrollToVerticalOffset(0);
                    DoEvents();

                    // Tab 1: Analytics & Charts
                    darkMain.DeviceGridArea.SelectTab(1);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_analytics_dark.png");

                    darkMain.DeviceGridArea.AnalyticsScrollViewer.ScrollToVerticalOffset(440);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_analytics_scroll1_dark.png");

                    // Tab 2: Icons Gallery
                    darkMain.DeviceGridArea.SelectTab(2);
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_icons_dark.png");

                    // Advanced Ribbon tab
                    darkMain.DeviceGridArea.SelectTab(0);
                    darkMain.RibbonTabStrip.SelectedIndex = 1;
                    DoEvents();
                    SaveWindowAsPng(darkMain, "screenshot_advanced_dark.png");
                    darkMain.Hide();

                    var darkSettings = new SettingsWindow();
                    darkSettings.Show();
                    darkSettings.SidebarListBox.SelectedIndex = 5;
                    DoEvents();
                    SaveWindowAsPng(darkSettings, "screenshot_settings_dark.png");
                    darkSettings.Hide();

                    var darkTileSettings = new TileSettingsWindow();
                    darkTileSettings.Show();
                    darkTileSettings.SidebarListBox.SelectedIndex = 5;
                    DoEvents();
                    SaveWindowAsPng(darkTileSettings, "screenshot_tile_settings_dark.png");
                    darkTileSettings.Hide();

                    var darkExplorer = new ExplorerLayoutWindow();
                    darkExplorer.Show();
                    DoEvents();
                    SaveWindowAsPng(darkExplorer, "screenshot_explorer_dark.png");
                    darkExplorer.Hide();

                    var darkMmc = new MmcLayoutWindow();
                    darkMmc.Show();
                    DoEvents();
                    SaveWindowAsPng(darkMmc, "screenshot_mmc_dark.png");
                    darkMmc.Hide();

                    var darkStudio = new StudioLayoutWindow();
                    darkStudio.Show();
                    DoEvents();
                    SaveWindowAsPng(darkStudio, "screenshot_studio_dark.png");
                    darkStudio.Hide();

                    var darkDevSettings = new DeviceSettingsWindow();
                    darkDevSettings.Show();
                    DoEvents();
                    SaveWindowAsPng(darkDevSettings, "screenshot_device_settings_dark.png");
                    darkDevSettings.Hide();

                    var darkPing = new DiagnosticPingDialog("192.168.1.20");
                    darkPing.Show();
                    DoEvents();
                    SaveWindowAsPng(darkPing, "screenshot_ping_dialog_dark.png");
                    darkPing.Hide();

                    var darkLogin = new LoginDialog();
                    darkLogin.Show();
                    DoEvents();
                    SaveWindowAsPng(darkLogin, "screenshot_login_dialog_dark.png");
                    darkLogin.Hide();

                    var darkTimeSettings = new TimeSettingsWindow();
                    darkTimeSettings.Show();
                    DoEvents();
                    SaveWindowAsPng(darkTimeSettings, "screenshot_time_settings_dark.png");
                    darkTimeSettings.ShowStep(2);
                    DoEvents();
                    SaveWindowAsPng(darkTimeSettings, "screenshot_time_settings_dark_step2.png");
                    darkTimeSettings.Hide();

                    var darkDemoModal = new DemoModalDialog();
                    darkDemoModal.Show();
                    DoEvents();
                    SaveWindowAsPng(darkDemoModal, "screenshot_demo_modal_dialog_dark.png");
                    darkDemoModal.Hide();

                    var darkFilePicker = new FilePickerDialog();
                    darkFilePicker.Show();
                    DoEvents();
                    SaveWindowAsPng(darkFilePicker, "screenshot_file_picker_dialog_dark.png");
                    darkFilePicker.Hide();

                    // Switch back to Classic Aero Light
                    ClassicAeroStudio.Services.ThemeManager.ApplyTheme(ClassicAeroStudio.Services.AppTheme.ClassicAero);
                    DoEvents();

                    Shutdown();
                    return;
                }

                var win = new MainWindow();
                win.Show();
            }
            catch (Exception ex)
            {
                File.WriteAllText("crash.log", ex.ToString());
                Console.Error.WriteLine("FATAL CRASH: " + ex);
                Environment.Exit(1);
            }
        }

        private static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(f =>
            {
                ((DispatcherFrame)f).Continue = false;
                return null;
            }), frame);
            Dispatcher.PushFrame(frame);
        }

        private static void SaveWindowAsPng(Window window, string filePath)
        {
            window.UpdateLayout();

            FrameworkElement? fe = window.Content as FrameworkElement;
            int w = fe != null && fe.ActualWidth > 0 ? (int)Math.Round(fe.ActualWidth) : (int)Math.Round(window.ActualWidth);
            int h = fe != null && fe.ActualHeight > 0 ? (int)Math.Round(fe.ActualHeight) : (int)Math.Round(window.ActualHeight);
            if (w <= 0) w = 1100;
            if (h <= 0) h = 700;

            var rtb = new RenderTargetBitmap(w, h, 96, 96, PixelFormats.Pbgra32);
            if (fe != null)
            {
                var dv = new DrawingVisual();
                using (var dc = dv.RenderOpen())
                {
                    if (window.Background != null)
                    {
                        dc.DrawRectangle(window.Background, null, new Rect(0, 0, w, h));
                    }
                    dc.DrawRectangle(new VisualBrush(fe) { Stretch = Stretch.None }, null, new Rect(0, 0, w, h));
                }
                rtb.Render(dv);
            }
            else
            {
                rtb.Render(window);
            }

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using (var fs = File.Create(filePath))
            {
                encoder.Save(fs);
            }
        }

        private static void SaveVisualAsPng(Visual visual, int w, int h, string filePath)
        {
            var rtb = new RenderTargetBitmap(w, h, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            using (var fs = File.Create(filePath))
            {
                encoder.Save(fs);
            }
        }

        private static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var current = child;
            while (current != null)
            {
                if (current is Visual || current is System.Windows.Media.Media3D.Visual3D)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else if (current is FrameworkContentElement fce)
                {
                    current = fce.Parent;
                }
                else
                {
                    break;
                }

                if (current is T typedParent)
                    return typedParent;
            }
            return null;
        }
    }
}
