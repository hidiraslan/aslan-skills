# Windows Classic Aero UI - Layout Metrics, Alignment & Anti-Clipping Standards

This document establishes the official mathematical spacing grid, padding/margin ratios, alignment rules, and anti-clipping patterns for the Classic Aero Enterprise UI design system.

---

## 1. The Aero 4px/8px Geometric Spacing Grid

To ensure rock-solid visual rhythm and avoid subpixel interpolation blur, all margins, paddings, and element gaps MUST be derived from the **4px / 8px scale**:

| Token Scale | Value | Primary Usage |
| :--- | :--- | :--- |
| **Micro (Gap-2)** | `2px` | Spacing between stacked stepper buttons, icon-to-badge spacing |
| **Tight (Gap-4)** | `4px` | Spacing between icon and label in small tool buttons, chip margins |
| **Compact (Gap-6)** | `6px` | Icon-to-label gap in standard buttons, subtitle margin under title |
| **Standard (Gap-8)** | `8px` | Space between sibling form buttons (`OK`, `Cancel`), input-to-label gaps |
| **Base (Gap-10)** | `10px` | Horizontal padding inside sub-headers, internal card padding |
| **Medium (Gap-12)** | `12px` | Outer dialog margin, spacing between major Form Grid rows |
| **Comfort (Gap-14)** | `14px` | Banner and Info Card internal padding, GroupBox bottom margins |
| **Generous (Gap-16)** | `16px` | Canvas padding around cards, settings page outer container margin |
| **Scroll Buffer** | `24px` | Right padding on scrollable content to prevent scrollbar collision |

---

## 2. Preventing Text Truncation & Control Clipping

### 2.1 The Dialog Window Sizing Formula
To guarantee that form fields, NTP servers, IP addresses, and localized strings NEVER truncate or require horizontal scrolling:
1. **Always define both `Width`/`Height` and `MinWidth`/`MinHeight`:**
   ```xml
   Width="820" Height="560"
   MinWidth="740" MinHeight="490"
   ```
2. **Never hardcode small widths on text inputs:**
   - Inside a two-column property form, allocate the label to `Width="130"` or `Width="140"` and the input to `Width="*"`.
   ```xml
   <Grid.ColumnDefinitions>
       <ColumnDefinition Width="140"/> <!-- Fixed Label Column -->
       <ColumnDefinition Width="*"/>   <!-- Flexible Input Column -->
   </Grid.ColumnDefinitions>
   ```
3. **Always set `HorizontalAlignment="Stretch"` on ComboBoxes:**
   - Setting `HorizontalAlignment="Left"` without a fixed width causes the ComboBox to shrink when shorter text is selected (e.g. `VID 1` vs `VID 4094`).

---

## 3. Vertical Centering & Icon Alignment Laws

### The 100% Optical Centering Rule
In Classic Aero, icons and text labels must be optically and mathematically centered on the exact same baseline:

1. **Horizontal Button Content:**
   ```xml
   <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
       <Rectangle Width="16" Height="16" Fill="{DynamicResource IconSettings}"
                  Margin="0,0,6,0"
                  VerticalAlignment="Center"/>
       <TextBlock Text="Settings"
                  FontFamily="Segoe UI"
                  FontSize="11"
                  VerticalAlignment="Center"/>
   </StackPanel>
   ```
   - Both the `<Rectangle>` and the `<TextBlock>` must specify `VerticalAlignment="Center"`.
   - Never use vertical margins (e.g. `Margin="0,2,6,0"`) to "nudge" text; this breaks High-DPI scaling.

2. **Square Tile Navigation Items (Vertical Stacking):**
   ```xml
   <StackPanel Orientation="Vertical" HorizontalAlignment="Center" VerticalAlignment="Center">
       <Rectangle Width="26" Height="26" Fill="{DynamicResource IconGeneral}" HorizontalAlignment="Center"/>
       <TextBlock Text="General" FontFamily="Segoe UI" FontSize="10.5"
                  TextAlignment="Center"
                  Margin="0,5,0,0"/>
   </StackPanel>
   ```
   - Top icon: `26x26px`, centered.
   - Bottom label: `10.5pt`, `TextAlignment="Center"`, `Margin="0,5,0,0"`.

---

## 4. Button Dimensions & Padding Matrix

| Button Type | Height | Min Width | Internal Padding | Font Size / Weight | Usage |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Dialog Primary (OK/Next)** | `24px` | `75px` | `10,2` | `11pt` Regular/SemiBold | Modal dialog confirmation |
| **Dialog Secondary (Cancel)** | `24px` | `75px` | `10,2` | `11pt` Regular | Modal dialog dismiss |
| **Form Standard Button** | `26px` | `85px` | `10,3` | `11pt` Regular | Form trigger (`Browse...`, `Test`) |
| **Sub-Header Small Tool** | `22px` | `Auto` | `8,1` | `11pt` Regular | `Refresh`, `Export`, `Reset` |
| **Ribbon Large Tool Button** | `54px` | `64px` | `4,4` | `11pt` Regular | Main Window top toolbar |

---

## 5. Scrollbar Safety & Overflow Clearance

### The Scrollbar Collision Problem
When a vertical `ScrollViewer` activates, the scrollbar occupies `17px` of width. If content inside the scroll container has `Margin="16"`, the scrollbar will render directly on top of or flush against the right border of your cards.

### The Standard Fix
Always provide **`24px` right margin** on content hosted inside vertical ScrollViewers:
```xml
<ScrollViewer VerticalScrollBarVisibility="Auto" HorizontalScrollBarVisibility="Disabled">
    <!-- Notice the right margin is 24px instead of 16px -->
    <StackPanel Margin="16,14,24,14">
        <!-- Content Cards -->
    </StackPanel>
</ScrollViewer>
```

### Eliminating Accidental Horizontal Scrollbars
Main Window canvases, Settings sidebars, and Dialogs must explicitly enforce:
```xml
ScrollViewer.HorizontalScrollBarVisibility="Disabled"
```
This forces all text and panels to wrap or resize proportionally without ever popping up an annoying horizontal scrollbar.

---

## 6. Sub-Header Bar Layout Specification (Strict 34px Rule)

Row 0 of every panel MUST declare:
- `RowDefinition Height="34"`
- Border: `Height="34" BorderBrush="#D0DFEE" BorderThickness="0,0,0,1" Background="#FFFFFF" SnapsToDevicePixels="True"`
- Inner margin: `Margin="10,0,10,0"`
- Vertical centering: `VerticalAlignment="Center"` on the inner `Grid`.
- Responsive horizontal `ScrollViewer` enclosing the header controls:
  ```xml
  <ScrollViewer HorizontalScrollBarVisibility="Auto"
                VerticalScrollBarVisibility="Disabled"
                Focusable="False"
                BorderThickness="0">
  ```

---

## 7. Dialog Command Footers & Button Separation Law

### The DockPanel Anti-Pattern
Never use `<DockPanel LastChildFill="False">` with `DockPanel.Dock="Right"` for dialog footer action buttons (`OK`, `Cancel`, `Apply`).
Because WPF right-docks children in sequential order:
- The first button docks to the right border with its right margin applied against the window boundary.
- The second button docks against the left edge of the first button with default `0px` margin, causing the buttons to appear **glued together**.

### The Mandatory Footer Standard
All dialog footers (TaskDialog, FilePicker, Login, Wizard, Options) MUST use:
- Footer Border: `Height="46"` (or `Padding="16,0"`), `Background="#F0F0F0"` or `#F0F4F8`, `BorderBrush="#D0DFEE"`, `BorderThickness="0,1,0,0"`, `SnapsToDevicePixels="True"`.
- Layout: `<Grid VerticalAlignment="Center">` containing:
  - Left-aligned status or information text: `HorizontalAlignment="Left"`, `VerticalAlignment="Center"`, `Foreground="#888888"`.
  - Right-aligned buttons inside a horizontal `StackPanel`:
    ```xml
    <StackPanel Orientation="Horizontal" HorizontalAlignment="Right" VerticalAlignment="Center">
        <Button Content="OK" Style="{StaticResource ClassicDefaultButton}" IsDefault="True" Width="75" Height="24" Margin="0,0,8,0"/>
        <Button Content="Cancel" Style="{StaticResource ClassicStandardButton}" IsCancel="True" Width="75" Height="24"/>
    </StackPanel>
    ```
- All sibling buttons except the last must have **`Margin="0,0,8,0"`** (or `Margin="8,0,0,0"` if left-to-right ordered).

---

## 8. ComboBox Sizing & Grid Layout Budgeting

1. **Default MinWidth:** `ClassicComboBox` defines `MinWidth="50"` (never hardcode large `MinWidth` like 140px globally, as it overflows small grid columns and gets covered by neighboring controls).
2. **Column Budgeting:** When placing ComboBoxes inside multi-column grids:
   - Fixed text ComboBoxes (e.g. `4 Packets`, `1000 ms`) need at least `95px - 110px`.
   - Descriptive ComboBoxes (e.g. `32 Bytes (Standard)`, `Baud Rate (115200)`) need `135px - 160px`.
   - Dropdown toggle arrow occupies `17px` on the right border. Always verify adjacent column controls have explicit left margins or dedicated columns so they never overlap the toggle arrow button.

---

## 9. Window Resizing Limits & Form-Action Layout Isolation

### 9.1 Mandatory MinWidth & MinHeight Enforcement
In classic desktop UI, windows must never allow the user to resize them below the minimum bounding box of their contents:
1. **Always calculate and declare explicit `MinWidth` and `MinHeight`:**
   ```xml
   Height="470" Width="580"
   MinHeight="450" MinWidth="565"
   ```
2. The operating system's window manager (DWM) directly respects `MinWidth` and `MinHeight`, physically preventing the user from dragging the window border smaller than the safe layout threshold.
3. If a window contains fixed-width controls, its `MinWidth` must be set to:
   $$\text{MinWidth} \ge \sum \text{ColumnWidths} + \text{Margins} + \text{Padding} + \text{OS Border Overhead (16px)}$$

### 9.2 Form vs Action Buttons Layout Isolation (Grid-in-Grid Pattern)
Never place flexible input controls and fixed action buttons side-by-side in the same linear column set where `*` sizing can starve the button column:
- **Anti-pattern:** Placing labels, inputs, and action buttons in a single 6-column Grid where column 5 is `*`. When resized smaller, column 5 shrinks below the button's width, causing the button to render on top of column 4.
- **Mandatory Pattern:** Use a parent 2-column Grid:
  - **Column 0 (`Width="*"`):** Houses an inner Grid with form labels, text boxes, and combo boxes.
  - **Column 1 (`Width="Auto"`):** Houses the action button stack (`Start Ping`, `Clear Log`) with `Margin="14,0,0,0"`.
  Because Column 1 is `Width="Auto"`, the layout engine will **never** compress it or allow inputs from Column 0 to overlap it under any window dimension.
