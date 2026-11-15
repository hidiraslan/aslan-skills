# Workspace Development Rules - Classic Aero Enterprise UI

## 1. Absolute Design System Exclusivity
When working on this project or whenever the `classic-aero-ui` design system is requested:
- **NEVER USE `wpf-ui`** (lepo.co), WinUI 3, Windows 11 Fluent 2, MaterialDesignThemes, MahApps.Metro, or generic flat web frameworks.
- **NEVER MIX DESIGN LANGUAGES**: All windows, sidebars, buttons, tabs, tables, charts, and icons must come **EXCLUSIVELY** from the `classic-aero-ui` skill assets (`Themes/`, `Assets/Icons/ClassicIcons.xaml`, and `examples/`).
- If `wpf-ui` is installed globally or discovered on the system, **IGNORE IT COMPLETELY**.

## 2. Mandatory Structural Standards
1. **Window Base Anatomy:**
   - Window Canvas Background: `#F4F6F9`.
   - Content Panel / Work Area: Pure White `#FFFFFF` inside 1px border.
   - Inner Scrollable Canvas: `#F8FAFD`.
   - High-DPI ClearType: Root window must declare `SnapsToDevicePixels="True"`, `UseLayoutRounding="True"`, `TextOptions.TextFormattingMode="Display"`, `TextOptions.TextRenderingMode="ClearType"`, `TextOptions.TextHintingMode="Fixed"`, `RenderOptions.ClearTypeHint="Enabled"`.
   - Window Favicon: `Icon="pack://application:,,,/Assets/app_icon.png"`.
2. **Settings / Navigation Windows & Scroll Management:**
   - **Default & Primary Mode:** Classic List Navigation (`Width="180"` sidebar, 32px row items, 16x16 icon on left, label on right, classic blue Aero selection gradient).
   - **Optional Tile Mode:** Square Tile Navigation (`Width="130"` with 96x68px square items, 26x26px vector icon on top, label underneath, Windows 7 Royal Blue `#1A84E3` to `#0B6EC6` selected state) when explicitly requested.
   - `HorizontalScrollBarVisibility="Disabled"` must be enforced on Window, ListBox, and ScrollViewer.
   - **Zero Horizontal Jump on Focus/Expand:** ScrollViewers must suppress horizontal shifts via `RequestBringIntoView` (`e.Handled = true`), and Expanders must declare `Focusable="False"` on their header toggle site to eliminate page jumping when expanded.
3. **Sub-Header Bars:**
   - Strict `34px` height on Row 0 with background `#FFFFFF`, bottom border `1px solid #D0DFEE` (`SnapsToDevicePixels="True"`), and responsive horizontal `ScrollViewer`.
4. **DataGrids (Global Precision Standard):**
   - **Gridlines & Borders:** Single 1px crisp vertical gridlines (`VerticalGridLinesBrush="#C2D3E4"` matching header border, `BorderThickness="0"` on cells to prevent 2px double-border thickness).
   - **Row Height & Balanced Buttons:** Standard `RowHeight="24"`. In-row tool buttons must have `Height="18"` with `VerticalAlignment="Center"` providing perfectly equal top and bottom gaps (never touching horizontal gridlines).
   - **Centering & Resizing:** `CanUserResizeColumns="True"` across all tables and columns. Action buttons, status icons, and check indicators must use centered cell & header styles (`ClassicDataGridCellCentered`, `ClassicDataGridColumnHeaderCentered`). Header checkbox toggles all row selections, 3D Crystal LED status spheres.
5. **Data Visualization:**
   - Donut charts must use seamless fluid Aero gloss (`RadialGradientBrush` with `StrokeThickness="0"` on paths, borderless and divider-free) with centered telemetry labels and compact Aero pill legends.
   - ProgressBars must have flush fill (`Margin="0"` and `ClipToBounds="True"` inside `#8B9EB5` border).
6. **New / Unseen Components (Component DNA):**
   - Synthesize using 1px borders, subtle 2-4px corner radii, cylindrical top-to-bottom Aero gradients, Segoe UI typography hierarchy, and DrawingBrush vector icons. Never fall back to flat modern styles.
7. **Global Menu & TreeView Behavior:**
   - Enforce zero menu hover auto-open (top-level menus require an explicit click to open; hovering over adjacent sibling menus while open never auto-opens them) and zero horizontal jump on TreeView selection via global class handlers registered in `App.xaml.cs`.
