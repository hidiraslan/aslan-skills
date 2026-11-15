# Windows Classic Aero UI - Architecture & Common Pitfalls Guide

This guide documents the critical technical edge cases, bugs, and visual defects encountered during the development of this design system, along with the proven engineering solutions.

---

## 1. Sub-Header Bar Consistency Across All Tabs

### The Problem
When building multi-tab interfaces (e.g., `Device List`, `Alarms`, `Workspace`, `Analytics`, `Icons`), different tabs often end up with slight differences in header height, border color, or DPI rendering. For example:
- One tab has `RowDefinition Height="32"` while others have `34`.
- One tab's border lacks `SnapsToDevicePixels="True"`, causing the 1px bottom border to blur or vanish under Windows display scaling (125%, 150%).
- One tab lacks a `ScrollViewer`, causing action buttons or search boxes to get clipped when the user narrows the window.

### The Solution
Implement the exact same 4-rule template on Row 0 of EVERY view panel:
1. `RowDefinition Height="34"` (Strictly 34px across all views).
2. `<Border Grid.Row="0" BorderBrush="#D0DFEE" BorderThickness="0,0,0,1" Background="#FFFFFF" SnapsToDevicePixels="True">`.
3. Wrap inner Grid in a non-focusable horizontal `ScrollViewer`:
   ```xml
   <ScrollViewer HorizontalScrollBarVisibility="Auto"
                 VerticalScrollBarVisibility="Disabled"
                 Focusable="False"
                 BorderThickness="0">
   ```
4. Unified typography:
   - Primary Title: `FontFamily="Segoe UI" FontSize="12" FontWeight="SemiBold" Foreground="#1E395B"`
   - Subtitle: `FontFamily="Segoe UI" FontSize="11" Foreground="#6B88A4" Margin="6,0,0,0"`

---

## 2. Circular Donut Chart 1px Radial Dividers (Eliminating Double Stroke)

### The Problem
When drawing an annular donut chart composed of multiple adjacent `Path` sectors, setting `Stroke="#8B9EB5" StrokeThickness="1"` on each `Path` causes the shared border between two adjacent segments to be drawn TWICE. The result is a 2px to 3px thick, fuzzy radial line that violates the 1px aesthetic of the segmented progress bar.

### The Solution
1. Set `StrokeThickness="0"` on all 6 annular sector `Path` elements.
2. Calculate the exact boundary coordinates $(X, Y)$ at each transition angle:
   - Outer point: $(55.0 + 53\sin\theta, 55.0 - 53\cos\theta)$
   - Inner point: $(55.0 + 37\sin\theta, 55.0 - 37\cos\theta)$
3. Overlay discrete `<Line>` elements with `Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"` directly over the seam:
   ```xml
   <Line X1="55.0" Y1="2.0"   X2="55.0" Y2="18.0" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"/>
   <Line X1="105.4" Y1="71.4" X2="90.2" Y2="66.4" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"/>
   ...
   ```
4. Place the outer `<Ellipse Width="106" Height="106"/>` and inner `<Ellipse Width="74" Height="74"/>` with `StrokeThickness="1"` on top of the lines to cleanly cap their endpoints.

---

## 3. Segmented Progress Bar Border Gaps (Flush Fill)

### The Problem
Putting an inner `<Grid Margin="1">` inside the outer `<Border>` leaves a 1px white border around the progress bar segments, causing the colored fills to look detached from the `#8B9EB5` frame.

### The Solution
- Set `<Grid Margin="0" ClipToBounds="True">` on the inner container.
- The colored rectangles now render flush against the 1px outer border with zero gap.

---

## 4. Vertical Slider Jitter & Shaking

### The Problem
When dragging a slider, if the readout label (e.g. `9%`, `10%`, `100%`) is auto-sized (`Width="Auto"`), the digit count changes cause the label's width to expand and contract. In a horizontal layout, this pushes and pulls the slider's layout slot on every tick, causing severe visual jitter.

### The Solution
Always provide an explicit fixed width and right alignment to the percentage readout text:
```xml
<TextBlock Text="{Binding Value, ElementName=SliderCtrl, StringFormat='{}{0:0}%'}"
           Width="35"
           TextAlignment="Right"
           VerticalAlignment="Center"/>
```

---

## 5. Form Input & Dropdown Width Collapse (Auto vs. Stretch)

### The Problem
When a `ComboBox` is placed in a Grid column with `Width="*"` or `Width="Auto"`, if `HorizontalAlignment="Left"` is specified without a fixed `Width`, selecting a shorter item (e.g. `VID 1` instead of `VID 4094 (Management)`) causes the ComboBox to physically shrink in width.

### The Solution
Always use `HorizontalAlignment="Stretch"` for form inputs, or specify an explicit `Width` (e.g., `Width="220"`).

---

## 6. Scrollbars Overlapping Content Cards

### The Problem
In settings windows or card galleries, when a vertical `ScrollViewer` activates, the scrollbar thumb can directly overlap or touch the right border of cards if there is no margin budgeted for it.

### The Solution
1. On the inner container, provide right margin or padding (e.g. `Margin="16,16,24,16"`).
2. For tile/square sidebars, explicitly set `HorizontalScrollBarVisibility="Disabled"` to prevent unwanted horizontal scrollbars when resizing.

---

## 7. GroupBox Header White Box Artifact

### The Problem
On non-white backgrounds (such as `#F8FAFD` canvas or `#F0F4F9` dialogs), the standard WPF GroupBox header often has a hardcoded white background (`Background="#FFFFFF"`), creating an unsightly white box behind the header text.

### The Solution
Set the GroupBox header container background to `Background="Transparent"` and provide clean 1px borders with `SnapsToDevicePixels="True"`.

---

## 8. Hardware Capacity Dial / Gauge Centering & Label Positioning

### The Problem
- The red needle indicator pivot defaults to `(0, 0)` rather than the center of the gauge base, causing the needle to swing wildly outside the dial.
- The `0%` and `100%` labels placed inside the arc overlay the colored gauge track, making them unreadable.

### The Solution
1. Set `RenderTransformOrigin="0.5, 1.0"` on the needle line, positioning its base at the exact center of the half-circle arc.
2. Place the `0%` and `100%` labels underneath the arc endpoints with clean vertical margins, completely separated from the graphic track.

---

## 9. Global Menu vs. ContextMenu Event Trapping

### The Problem
Registering global class event handlers for `MenuBase` (e.g. `EventManager.RegisterClassHandler(typeof(MenuBase), UIElement.PreviewMouseMoveEvent, ...)`) inadvertently intercepts `ContextMenu` instances. Because `ContextMenu` inherits from `MenuBase`, resetting menu state or submenu tracking breaks right-click popups, table context menus, and tool buttons.

### The Solution
Scope global class handlers strictly to `typeof(Menu)` (never `typeof(MenuBase)`):
```csharp
EventManager.RegisterClassHandler(
    typeof(Menu),
    UIElement.PreviewMouseMoveEvent,
    new MouseEventHandler((s, args) => { ... }));
```

---

## 10. Multi-Theme Tokenization & The Hardcoded Color Anti-Pattern

### The Problem
Hardcoding hex codes (e.g. `#E2E7ED`, `#1E395B`) directly into UserControl XAML or chart elements breaks them when switching to Dark Aero or another theme. The controls remain glaringly light or illegible in dark mode.

### The Solution
1. Always define semantic palette keys in both `ClassicWindowsTheme.xaml` and `DarkAeroTheme.xaml` (e.g. `ClassicBulletRange1`, `ClassicBulletBorder`, `ClassicBulletBarPrimary`).
2. Bind elements exclusively using `{DynamicResource KeyName}`, never raw hex codes.
3. When creating a 3rd or 4th theme, you only need to declare these standard keys in the new dictionary without editing user controls.

---

## 11. Tri-State CheckBox Indeterminate Visual States

### The Problem
WPF CheckBox states include `true`, `false`, and `{x:Null}` (Indeterminate). Custom templates that only handle `IsChecked="True"` fail when bound to nullable booleans (like table header multi-select), leaving the box empty or rendering default flat WPF boxes.

### The Solution
Always provide an explicit visual glyph (e.g. a 3D indented square or dash) and trigger for `{x:Null}`:
```xml
<Trigger Property="IsChecked" Value="{x:Null}">
    <Setter TargetName="indeterminateBox" Property="Visibility" Value="Visible"/>
</Trigger>
```

---

## 12. Native Windows Title Bar & DWM Immersive Dark Mode

### The Problem
On Windows 10/11, WPF window title bars default to white even when the window's content is dark. Stripping window borders (`WindowStyle="None"`) loses native Aero Snap, shadow animations, and resize handles.

### The Solution
Use the Win32 Desktop Window Manager attribute `DWMWA_USE_IMMERSIVE_DARK_MODE` (attribute 20, fallback 19) via `DwmSetWindowAttribute(hwnd, 20, ref darkMode, 4)`:
- Hook it to `Window.Loaded` and `ThemeManager.ThemeChanged`.
- Renders the OS title bar, minimize/maximize/close buttons, and title text in native dark mode with zero visual glitches or resizing bugs.

---

## 13. Expander / Accordion Header Text Contrast

### The Problem
Expander headers that use system default foreground or dark accent brushes become invisible against dark container backgrounds.

### The Solution
Set `TextElement.Foreground="{DynamicResource ClassicTextTitle}"` (`#E2ECF8` in dark mode) and `FontWeight="SemiBold"` on the header toggle site, and use a crisp vector `Path` with dynamic stroke (`#8CBEE8` -> `#FFFFFF`) for the chevron icon instead of raster/bitmap icons.

---

## 14. PropertySheet TabControl Tab Strip Left Alignment (Eliminating Stepped Notch)

### The Problem
In property sheet dialogs (`ClassicPropertySheetTabControl`), setting a left margin on the TabStrip host (e.g. `StackPanel Margin="4,0,4,-1"`) indents the first tab by 4px from the content frame's outer border. This leaves an ugly horizontal 4px stair-step ledge with a sharp 90° corner at the top-left of the content box, making the active tab look detached and unaligned with the content panel below it.

### The Solution
Set `Margin="0,0,0,-1"` on the tab strip `StackPanel`:
```xml
<StackPanel Grid.Row="0"
            Orientation="Horizontal"
            IsItemsHost="True"
            Margin="0,0,0,-1"
            Panel.ZIndex="5"
            SnapsToDevicePixels="True"/>
```
With `Margin="0,0,0,-1"`, the left border of the active first tab (x=0) and the left border of the content panel (x=0) form a single, continuous, unbroken vertical line with a smooth rounded corner at the top of the tab.
