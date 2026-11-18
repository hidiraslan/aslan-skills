---
name: classic-aero-ui
description: >-
  EXCLUSIVE Windows Classic Aero & Enterprise Desktop UI design system for WPF (.NET).
  MANDATORY ENFORCEMENT: When this skill is active, you MUST EXCLUSIVELY use the styles, templates,
  colors, and vector icons defined inside this skill. You are STRICTLY FORBIDDEN from using, importing,
  or referencing wpf-ui, WinUI, Windows 11 Fluent 2, MaterialDesign, or flat web dashboard patterns.
  Even if wpf-ui is installed on the machine, IGNORE IT COMPLETELY. All windows, sidebars, buttons,
  tabs, progress bars, tables, charts, dialogs, and icons must come solely from classic-aero-ui.
metadata:
  version: 2.1.0
  author: Antigravity AI
---

# Windows Classic Aero Enterprise UI Design System (Exclusive System)

> [!CAUTION]
> **ABSOLUTE EXCLUSIVITY DIRECTIVE (ZERO OUTSIDE STYLES):**
> When `classic-aero-ui` is invoked or active, **DO NOT PULL STYLES FROM ANY OTHER SOURCE**:
> - **DO NOT USE `wpf-ui`** (Ignore lepo.co, `ui:NavigationView`, `ui:SymbolIcon`, Fluent 2 pills, Mica/Acrylic).
> - **DO NOT USE MaterialDesignThemes**, MahApps.Metro, or modern flat web themes.
> - **DO NOT USE DEFAULT OS FLAT CONTROLS**.
> Every single window, dialog, toolbar, tab, sub-header, table, chart, button, slider, and icon **MUST COME EXCLUSIVELY** from the templates and resource dictionaries in this skill folder (`examples/`).

---

## 1. Quick Integration Guide: How to Apply to Any Project

When a user asks to apply `classic-aero-ui` to a project (existing or new), execute the automated setup or copy files directly:

### Method A: Single-Command Automated Setup
Run the installer script directly from the project directory (PowerShell):
```powershell
powershell -ExecutionPolicy Bypass -File "$env:USERPROFILE\.gemini\config\skills\classic-aero-ui\scripts\install.ps1" -TargetDir (Get-Location).Path
```
*Or extract the self-contained zip bundle:*
```powershell
powershell -ExecutionPolicy Bypass -Command "Expand-Archive -Path '$env:USERPROFILE\.gemini\config\skills\classic-aero-ui\classic-aero-ui-bundle.zip' -DestinationPath . -Force"
```

### Method B: Manual File Locations (If copying directly)
Copy the following folders/files into the target project:
1. `Assets/Icons/ClassicIcons.xaml` (67 native High-DPI DrawingBrush vector icons)
2. `Assets/app_icon.png` & `Assets/app_icon.ico` (Application icons)
3. `Themes/ClassicWindowsTheme.xaml` (Default Classic Aero Light theme)
4. `Themes/DarkAeroTheme.xaml` (Dark Aero Obsidian theme)
5. `Themes/*Styles.xaml` (All control dictionaries: Buttons, DataGrid, Tabs, Sliders, ComboBoxes, etc.)
6. `Services/ThemeManager.cs` (Dynamic runtime theme switcher)
7. `AGENTS.md` $\to$ project root directory (Strict design exclusivity rules)

### Step 2: Register in `App.xaml` and `App.xaml.cs`
Ensure `App.xaml` merges the complete dictionary suite:
```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="pack://application:,,,/Assets/Icons/ClassicIcons.xaml"/>
            <ResourceDictionary Source="pack://application:,,,/Themes/ClassicWindowsTheme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

And in `App.xaml.cs` inside `OnStartup`, register global handlers for zero menu hover auto-open and zero TreeView horizontal jump:
```csharp
private static readonly System.Reflection.PropertyInfo? MenuBaseIsMenuModeProperty =
    typeof(System.Windows.Controls.Primitives.MenuBase)
        .GetProperty("IsMenuMode", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    // 1. Prevent top-level Menu hover auto-open (Strictly typeof(Menu), NOT MenuBase)
    EventManager.RegisterClassHandler(
        typeof(Menu),
        UIElement.PreviewMouseMoveEvent,
        new MouseEventHandler((s, args) =>
        {
            if (s is Menu menu)
                MenuBaseIsMenuModeProperty?.SetValue(menu, false);
        }));

    // 2. Prevent TreeView unwanted horizontal jump on focus
    EventManager.RegisterClassHandler(
        typeof(TreeViewItem),
        FrameworkElement.RequestBringIntoViewEvent,
        new RequestBringIntoViewEventHandler((s, args) => args.Handled = true));

    // 3. Apply Immersive Dark Titlebar when Dark Theme is active
    EventManager.RegisterClassHandler(
        typeof(Window),
        FrameworkElement.LoadedEvent,
        new RoutedEventHandler((s, args) =>
        {
            if (s is Window w)
                ThemeManager.ApplyWindowTheme(w, ThemeManager.CurrentTheme == AppTheme.DarkAero);
        }));
}
```

### Step 3: Configure Root Window Properties & Application Icon
Every Window (`MainWindow`, Dialog, Settings, Wizard) MUST declare these exact rendering attributes:
```xml
<Window ...
        Icon="pack://application:,,,/Assets/app_icon.ico"
        FontFamily="Segoe UI, Tahoma, sans-serif"
        FontSize="11"
        Background="#F4F6F9"
        SnapsToDevicePixels="True"
        UseLayoutRounding="True"
        TextOptions.TextFormattingMode="Display"
        TextOptions.TextRenderingMode="ClearType"
        TextOptions.TextHintingMode="Fixed"
        RenderOptions.ClearTypeHint="Enabled">
```

> [!IMPORTANT]
> **Domain-Aware Application Icon Selection (Program Türüne Göre İkon Seçimi):**
> 1. **Preserve Existing Icon:** If the target project already has an application icon, **PRESERVE IT**. Never overwrite existing project branding.
> 2. **Domain-Specific Icon Assignment:** Analyze what kind of application is being built and select a semantically matching icon from `ClassicIcons.xaml`:
>    - *Database & Data Tools:* `IconDatabase`, `IconStorage`, `IconServer`
>    - *Developer & Code Tools:* `IconCode`, `IconTerminal`, `IconStudio`
>    - *Security, VPN & Vaults:* `IconShield`, `IconKey`, `IconLock`
>    - *Media, Audio & Entertainment:* `IconAudio`, `IconPlay`
>    - *Analytics & Telemetry:* `IconAnalytics`, `IconChart`
>    - *General Desktop Utility:* Use the neutral Classic Aero Crystal Diamond emblem (`IconAppLogo` / `Assets/app_icon.ico`).
> 3. **Strict Lettered Icon Prohibition:** NEVER use a hardcoded lettered icon (such as a legacy "D" icon) unless the program name specifically starts with that letter and the user explicitly requests an initial/monogram icon. Use the Vector Icon Creation Guide (`references/icon-creation-guide.md`) to synthesize matching Aero vector icons for any new domain.

### Step 4: Drop In Ready Templates
- **Main Shell:** Use `examples/MainWindowTemplate.xaml`
- **Windows Explorer Shell:** Use `examples/ExplorerLayoutTemplate.xaml` (Address bar, left navigation pane, toolbar, detail table)
- **MMC Console Shell:** Use `examples/MmcLayoutTemplate.xaml` (File menu, MMC toolbar, 3-pane TreeView/Center/Right Action Bar)
- **Multi-Document Studio Shell:** Use `examples/StudioLayoutTemplate.xaml` (Visual Studio style docked document tabs, Output panel, Solution Explorer)
- **Settings with Left Sidebar:** Use `examples/ClassicSettingsWindowTemplate.xaml` (Default Classic List) or `examples/TileSettingsWindowTemplate.xaml` (Square Tile Mode)
- **Dialogs / Wizards:** Use `examples/WizardDialogTemplate.xaml`
- **Confirmation / Alert Modals:** Use `examples/TaskDialogTemplate.xaml`
- **Sade Mesaj ve Bilgi Diyalogları:** Use `examples/AeroMessageBoxTemplate.xaml` (Tek/Çift/Üç butonlu, Info, Success, Warning, Error)
- **Hareketli İlerleme & Görev Diyalogları:** Use `examples/AeroProgressDialogTemplate.xaml` (Canlı Aero ProgressBar, yüzde, iptal ve arka planda yürütme)

---

## 2. Complete Component & Layout Rules

### 2.1 Settings Windows & Left Sidebar Architecture (MANDATORY)
Never use modern flat pill menus, Hamburger drawers, or WinUI `NavigationView`. Always use one of these two authentic Aero sidebars:

#### Mode A: Classic List Navigation (Yatay Liste Navigasyon Modu - DEFAULT & PRIMARY)
Use this as the primary/default sidebar mode for all configuration, options, and settings windows:
- **Sidebar Column Width:** Fixed `180px` (`<ColumnDefinition Width="180"/>`). Background: `#F0F4F8`, Right Border: `1px solid #C5D5E6`.
- **Item Height:** `32px`, Margin `0,1`.
- **Internal Item Layout:** Horizontal `StackPanel` with a `16x16px` vector DrawingBrush icon on LEFT (`Margin="0,0,8,0"`), followed by label text (`Segoe UI, 11pt, #1E395B`).
- **Visual States:**
  - **Hover:** Background LinearGradient `#FFFFFF` $\to$ `#EAF2FA`, Border `1px solid #B8D6F5`.
  - **Selected:** Classic Aero Blue Gradient (`#4A93D8` $\to$ `#2072BE`), Border `1px solid #185E9F`, Foreground `#FFFFFF`.
- **Right Detail Container:** `Margin="16,14,24,14"` with vertical scrollbar clearance.
- **Full Template File:** `examples/ClassicSettingsWindowTemplate.xaml`.

#### Mode B: Square Tile Navigation (Kare Menü Navigasyon Modu - OPTIONAL TILE MODE)
Use when the user specifically requests "kare menü", "tile navigation", or large icon-based navigation:
- **Sidebar Column Width:** Fixed `130px` (`<ColumnDefinition Width="130"/>`). Background: `#F0F4F8`, Right Border: `1px solid #C5D5E6`.
- **Tile Dimensions:** Width `96px`, Height `68px`, CornerRadius `4px`.
- **Internal Tile Layout:** Vertical `StackPanel` with a `26x26px` DrawingBrush vector icon on TOP, followed by an 11pt `TextBlock` UNDERNEATH with `TextAlignment="Center" Margin="0,5,0,0"`.
- **Visual States:**
  - **Normal:** Background `#FFFFFF`, Border `1px solid #DFE7F0`.
  - **Hover:** LinearGradient `#FFFFFF` $\to$ `#E6F0FA`, Border `1px solid #A0C5EB`.
  - **Selected (Windows 7 Royal Blue):** LinearGradient `#1A84E3` $\to$ `#0B6EC6`, Border `1px solid #095AA5`, Foreground `#FFFFFF`.
- **Scrollbar Elimination:** Enforce `ScrollViewer.HorizontalScrollBarVisibility="Disabled"` on Window, ListBox, and ScrollViewer.
- **Full Template File:** `examples/TileSettingsWindowTemplate.xaml`.

---

### 2.2 Sub-Header Bar Uniformity (Strict 34px Formula)
Every single tab or module view MUST use this exact layout on Row 0:
- Height: `34px`.
- Background: `#FFFFFF`.
- Bottom Divider: `1px solid #D0DFEE` with `SnapsToDevicePixels="True"`.
- Responsive Wrapper: Enclosed in a horizontal `ScrollViewer` (`HorizontalScrollBarVisibility="Auto"`, `VerticalScrollBarVisibility="Disabled"`, `Focusable="False"`, `BorderThickness="0"`).
- Typography:
  - Title: `Segoe UI, 12pt, SemiBold, #1E395B`
  - Subtitle: `Segoe UI, 11pt, Regular, #6B88A4, Margin="6,0,0,0"`
- **Full Template File:** `examples/SubHeaderBarTemplate.xaml`.

---

### 2.3 ProgressBars & Segmented Multi-Color (Flush 1px Borders)
- **Flush Fill:** Outer border is `#8B9EB5` (1px). Inner container must use `Margin="0"` and `ClipToBounds="True"` to eliminate white gaps.
- **Colors & Aero Cylindrical Reflection:**
  - **Green (Normal):** `#9AEF4E` (0.0) $\to$ `#40D622` (0.5) $\to$ `#24B812` (0.8) $\to$ `#1A9308` (1.0)
  - **Blue (Transfer):** `#6ECBFD` (0.0) $\to$ `#269DE6` (0.5) $\to$ `#107EC7` (0.8) $\to$ `#095996` (1.0)
  - **Amber (Warning):** `#FFE162` (0.0) $\to$ `#FFB812` (0.5) $\to$ `#F09B00` (0.8) $\to$ `#C97B00` (1.0)
  - **Red (Critical):** `#FF8484` (0.0) $\to$ `#F03333` (0.5) $\to$ `#D61818` (0.8) $\to$ `#A30E0E` (1.0)
  - **Gray (Standby):** `#CBD4DE` (0.0) $\to$ `#9EAEBE` (0.5) $\to$ `#889BB0` (0.8) $\to$ `#657A90` (1.0)
  - **Track (Boşluk):** `#DEE5EC` (0.0) $\to$ `#EFF4F8` (0.5) $\to$ `#F6FAFC` (1.0)
- **Segmented Multi-Color:** Connect all segments inside a single border separated by **1px wide `#8B9EB5` vertical lines**.

---

### 2.4 Circular Donut Chart 1b (Seamless Fluid Aero Donut)
- **Seamless Fluid Aero Gloss:** Use `RadialGradientBrush` (`MappingMode="Absolute"` Center="53,53" Origin="53,53" RadiusX="51" RadiusY="51"). Outer edge is highlight (Offset 1.0), inner hole is shadow (Offset 0.69).
- **Pure Fluid Glass (No Dividers, No Border Strokes):** All annular `Path` elements have `StrokeThickness="0"` with zero divider lines and zero outer/inner ellipse borders, providing a continuous fluid cylindrical ring.
- **Center Telemetry & Compact Legend:** Center displays live summary label (e.g. `100% Traffic`), flanked by compact Aero pill indicators.

---

### 2.5 DataGrid & Table Standards
- Always set `CanUserResizeColumns="True"` globally on the DataGrid and ensure every column can be resized by the user (do not lock columns with `CanUserResize="False"` unless strictly a 24px checkbox column).
- **Single 1px Vertical Gridlines:** `VerticalGridLinesBrush="#C2D3E4"` matching header border color, and `BorderThickness="0"` on cells to prevent double-border thickness.
- **Row Height & Symmetrical Spacing:** Standard table `RowHeight="24"`. Row action buttons must use `Height="18"` with `VerticalAlignment="Center"` to guarantee equal vertical top and bottom margins (never touching horizontal gridlines).
- Header checkbox on leftmost column toggles selection for all rows (`Select All`).
- Status column displays 3D Crystal LED Spheres (Green, Amber, Red, Blue, Gray, Black).
- **In-Row Controls (Zero Clipping & True Centering):**
  - **The 18px Law:** In a 24px DataGrid row, buttons MUST be strictly `Height="18"` to maintain symmetrical 3px top and 3px bottom margins. Standard WPF buttons (23-24px) WILL clip and appear cut off/incomplete.
  - **Standard Row Button Style:** Always use `{StaticResource ClassicRowButton}` (or `{StaticResource ClassicRowIconButton}` for 20x18 icon buttons, `{StaticResource ClassicRowButtonDanger}` for delete).
  - **Cell Centering:** Always apply `CellStyle="{StaticResource ClassicDataGridCellCentered}"` to the `DataGridTemplateColumn`.
  ```xml
  <DataGridTemplateColumn Header="İşlem" Width="75"
                          CellStyle="{StaticResource ClassicDataGridCellCentered}">
      <DataGridTemplateColumn.CellTemplate>
          <DataTemplate>
              <Button Style="{StaticResource ClassicRowButton}" Content="Ayrıntı"/>
          </DataTemplate>
      </DataGridTemplateColumn.CellTemplate>
  </DataGridTemplateColumn>
  ```
  - **ListBox Item Controls:** In `ListBox`, always wrap item contents in a `DockPanel` with `HorizontalContentAlignment="Stretch"`:
  ```xml
  <ListBox.ItemTemplate>
      <DataTemplate>
          <DockPanel LastChildFill="True">
              <Button DockPanel.Dock="Right" Style="{StaticResource ClassicRowButton}" Content="Kaldır"/>
              <TextBlock Text="{Binding Title}" VerticalAlignment="Center"/>
          </DockPanel>
      </DataTemplate>
  </ListBox.ItemTemplate>
  ```
- Selection gradient: Royal Blue `#2672B5` to `#1B548B` with white foreground.
- Style: `{StaticResource ClassicDataGrid}`.

---

### 2.6 Form Controls, Steppers & Switches
- **NumericUpDown:** 22px height. Text box has right border 0, stacked stepper container has 1px left border and 1px horizontal separator between up and down buttons.
- **ComboBox:** Always use `HorizontalAlignment="Stretch"` in grid layouts so the control never shrinks when selecting short text (e.g. `VID 1`).
- **Sliders:** Percentage text label beside slider MUST have a fixed width (e.g. `Width="35" TextAlignment="Right"`) to eliminate jitter during dragging.
- **Switches:** Use `{StaticResource ClassicAeroPillSwitch}` with crisp `#7A96B2` border and subtle 1.5px drop shadow on the white knob (no blurry white halos).
- **GroupBoxes:** Header background is `Transparent` to prevent ugly white box artifacts on tinted canvases (`#F8FAFD`).

---

### 2.7 Native High-DPI Vector Icons (DrawingBrush)
- Project includes 60+ native WPF `DrawingBrush` vector icons in `Assets/Icons/ClassicIcons.xaml` (Core UI, Network, Hardware/Monitors, Software Development, and User/Auth suites).
- Zero pixelation on 4K/high-DPI screens.
- Standard sizes: `13x13` (tools/table), `16x16` (buttons/lists), `22x22` (headers), `26x26` (tiles), `32x32` (banners).
- Syntax: `<Rectangle Width="26" Height="26" Fill="{DynamicResource IconSettings}"/>`.
- Developer & Auth keys: `IconCode`, `IconTerminal`, `IconDatabase`, `IconGit`, `IconBug`, `IconBuild`, `IconApi`, `IconPackage`, `IconRun`, `IconStop`, `IconBreakpoint`, `IconFunction`, `IconUser`, `IconLogin`, `IconExit`.

---

### 2.8 Aero Component Generation DNA (When Synthesizing Custom / New Controls)
If the user requests a control or view that is NOT explicitly included in the templates (e.g. Log Viewer, Kanban Board, Syntax Editor, Calendar, Topology Graph, Speedometer, Badge/Toast):
- **DO NOT INVENT FLAT MODERN STYLES.**
- Synthesize the new component using the **Aero Component DNA** defined in `references/component-dna.md`:
  1. **Surfaces:** Outer `#F4F6F9` $\to$ Cards `#FFFFFF` (1px `#B0C8DF` border) $\to$ Inner Canvas `#F8FAFD`.
  2. **The 1px Law:** All gridlines, dividing lines, and frames must be strictly 1px with `SnapsToDevicePixels="True"`.
  3. **Aero Lighting:** LinearGradient (0,0 $\to$ 0,1) with top reflection highlight to bottom shaded rim.
  4. **Corner Radii:** Subtle 2px to 4px. Never large pills or borderless flat cards.
  5. **Typography:** Segoe UI. Navy `#1E395B` for titles, Slate `#6B88A4` for subtitles, Charcoal `#222222` for body text.

### 2.9 Automated Design Audit & Defect Remediation (Fixing Existing Flaws)
When applying `classic-aero-ui` to an existing codebase or reviewing existing XAML:
- **Oversized In-Row Buttons:** Any button inside a `DataGrid` or `ListBox` row that exceeds `18px` in height or touches gridlines must be immediately refactored to `{StaticResource ClassicSmallToolButton}` (`Height="18" VerticalAlignment="Center"`) with a 10x10 or 12x12 vector icon. Never allow large blocky buttons inside tabular data.
- **Table / List Row Cramping & Bloat:** Enforce standard `RowHeight="24"` on DataGrids and `32px` on Navigation ListBoxes.
- **Misaligned Icons & Controls:** Checkboxes and action buttons in tables must use `{StaticResource ClassicDataGridCellCentered}` and `{StaticResource ClassicDataGridColumnHeaderCentered}`.
- **Flat Borders / Modern Halos:** Eliminate flat, borderless cards, 12px pill buttons, or blurry drop shadows. Convert to 1px `#B0C8DF` or `#7A96B2` Aero borders with top-to-bottom reflection gradients.
- **Scroll & Focus Jumps:** Any scrollable area must have horizontal jumping suppressed via `HorizontalScrollBarVisibility="Disabled"` or `RequestBringIntoView` handler (`e.Handled = true`).
- **Missing or Broken Scrollbars:** Ensure DataGrids and multi-item lists declare `VerticalScrollBarVisibility="Auto"` and `HorizontalScrollBarVisibility="Auto"`.

### 2.10 Multi-Theme & Dark Aero (Obsidian Glass) Architecture
`classic-aero-ui` includes first-class multi-theming with an authentic Windows 7 Dark Aero ("Obsidian Glass & Steel") palette that maintains identical Aero glass highlights, 1px borders, and 3D bevels in a dark environment.

1. **Theme Files:**
   - `Themes/ClassicWindowsTheme.xaml`: Standard Windows 7 Aero Light theme (`#F4F6F9` canvas, `#FFFFFF` cards, `#EDF2F8` ribbons, `#1E395B` titles).
   - `Themes/DarkAeroTheme.xaml`: Windows 7 Dark Aero / Obsidian Glass theme (`#181C22` canvas, `#212730` cards, `#1A1F26` inner, `#28313D` steel ribbons, `#E2ECF8` titles).

2. **Semantic Theme Dynamic Brushes:**
   Always bind container backgrounds and borders to semantic dynamic brushes:
   - `Background="{DynamicResource ClassicCanvasBackground}"` (Window root canvas)
   - `Background="{DynamicResource ClassicCardBackground}"` (Work areas, main panels)
   - `Background="{DynamicResource ClassicInnerBackground}"` (Sidebars, inner panels)
   - `BorderBrush="{DynamicResource ClassicDividerBrush}"` (1px partition lines)
   - `BorderBrush="{DynamicResource ClassicBorderBrush}"` (Outer control borders)
   - `Foreground="{DynamicResource ClassicTextTitle}"` (Section headers, titles)
   - `Foreground="{DynamicResource ClassicTextSubtitle}"` (Muted captions, secondary labels)
   - `VerticalGridLinesBrush="{DynamicResource ClassicGridLineVertical}"` (DataGrid column lines)
   - `HorizontalGridLinesBrush="{DynamicResource ClassicGridLineHorizontal}"` (DataGrid row lines)

3. **Dynamic Theme Switching (`ThemeManager`):**
   The centralized `Services/ThemeManager.cs` swaps merged dictionaries in memory instantly with zero flickering:
   ```csharp
   // Switch theme at runtime
   ThemeManager.ApplyTheme(AppTheme.DarkAero);
   // or
   ThemeManager.ApplyTheme(AppTheme.ClassicAero);
   ```
   Both `SettingsWindow` (List mode) and `TileSettingsWindow` (Tile mode) provide a theme selection ComboBox on the Display & Language page.

---

## 3. Mandatory Verification Checklist

Before considering any task complete under `classic-aero-ui`, verify:
1. [ ] Did you avoid all WinUI/`wpf-ui`/MaterialDesign references? (Must be YES)
2. [ ] Is the window background bound to `{DynamicResource ClassicCanvasBackground}` (or `#F4F6F9`) with ClearType rendering and `SnapsToDevicePixels="True"`?
3. [ ] Are all sub-header bars exactly 34px with a 1px border and responsive `ScrollViewer`?
4. [ ] Does the Settings window use Classic List Navigation (or Square Tile mode if explicitly requested)?
5. [ ] Is `HorizontalScrollBarVisibility="Disabled"` set on the Settings window and ListBox?
6. [ ] Are all in-row table action buttons strictly `Height="18"` with `{StaticResource ClassicRowButton}`?
7. [ ] Are all Donut chart dividing lines strictly 1px wide with `StrokeThickness="0"` on paths?
8. [ ] Do all ComboBoxes maintain consistent width without shrinking on short text?
9. [ ] Do all sliders have fixed-width value labels without shaking?
10. [ ] When dark mode is supported, are dynamic semantic brushes (`{DynamicResource ...}`) used for window background and panel canvases?
11. [ ] Does the application compile with 0 Errors and 0 Warnings?
