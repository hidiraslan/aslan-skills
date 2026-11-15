# Windows Classic Aero UI - Color Palette & Visual Tokens Reference

This document provides the definitive color tokens, gradients, and lighting constants for the Classic Aero UI design system.

---

## 1. Background Surfaces

| Token | Hex Value | Visual Characteristics | Usage |
| :--- | :--- | :--- | :--- |
| `BgWindow` | `#F4F6F9` | Neutral Cool Slate Gray | Top-level Window background, dialog canvas |
| `BgPanel` | `#FFFFFF` | Crisp Pure White | Tab panel container, DataGrid background, cards |
| `BgCanvas` | `#F8FAFD` | Very Light Aero Ice Blue | Scrollable gallery areas, chart canvases |
| `BgKpiStrip` | `#F6F9FD` | Subtle Ice Mist Blue | Workspace KPI quick-strip, secondary banners |
| `BgHeader` | `#FFFFFF` | Pure White | Sub-header bar background (Height 34) |

---

## 2. Borders & Dividing Gridlines

| Token | Hex Value | Purpose |
| :--- | :--- | :--- |
| `BorderSubHeader` | `#D0DFEE` | 1px horizontal sub-header divider & subtle separators |
| `BorderMainFrame` | `#708FA8` | 1px outer window container border, tab content frame |
| `BorderTrack` | `#8B9EB5` | 1px progress bar outer frame, circular donut border |
| `BorderCard` | `#B0C8DF` | 1px GroupBox, info card, and banner outline |
| `BorderControl` | `#7E9DB9` | 1px TextBox, ComboBox, and DataGrid cell border |
| `BorderFocused` | `#3D7BAD` | 1px focused control highlight border |

---

## 3. Typography Colors

| Token | Hex Value | Role |
| :--- | :--- | :--- |
| `TextNavyTitle` | `#1E395B` | Primary titles, active tab headers, prominent stats |
| `TextSlateSubtitle` | `#6B88A4` / `#6C839C` | Sub-header subtitles, metadata, breadcrumb links |
| `TextBody` | `#222222` | Standard data table cells, input text, dialog text |
| `TextMuted` | `#556677` | Descriptive explanatory paragraphs, helper labels |
| `TextDisabled` | `#8899AA` | Grayed-out labels and inactive options |

---

## 4. Aero Status Gradients (Progress Bars & Donut Charts)

### 4.1 LinearGradient (Horizontal ProgressBars)
StartPoint="0,0" EndPoint="0,1" (Top-to-Bottom cylindrical reflection)

#### Normal (Green)
- `Offset 0.00`: `#9AEF4E` (Top highlight)
- `Offset 0.50`: `#40D622` (Mid gloss)
- `Offset 0.80`: `#24B812` (Core green)
- `Offset 1.00`: `#1A9308` (Bottom shadow)

#### Transfer (Blue)
- `Offset 0.00`: `#6ECBFD` (Top highlight)
- `Offset 0.50`: `#269DE6` (Mid gloss)
- `Offset 0.80`: `#107EC7` (Core blue)
- `Offset 1.00`: `#095996` (Bottom shadow)

#### Warning (Amber)
- `Offset 0.00`: `#FFE162` (Top highlight)
- `Offset 0.50`: `#FFB812` (Mid gloss)
- `Offset 0.80`: `#F09B00` (Core amber)
- `Offset 1.00`: `#C97B00` (Bottom shadow)

#### Critical (Red)
- `Offset 0.00`: `#FF8484` (Top highlight)
- `Offset 0.50`: `#F03333` (Mid gloss)
- `Offset 0.80`: `#D61818` (Core red)
- `Offset 1.00`: `#A30E0E` (Bottom shadow)

#### Standby (Gray)
- `Offset 0.00`: `#CBD4DE` (Top highlight)
- `Offset 0.50`: `#9EAEBE` (Mid gloss)
- `Offset 0.80`: `#889BB0` (Core gray)
- `Offset 1.00`: `#657A90` (Bottom shadow)

#### Inset Track (Boşluk / Free Space)
- `Offset 0.00`: `#DEE5EC` (Top inset shadow)
- `Offset 0.50`: `#EFF4F8` (Inner trough)
- `Offset 1.00`: `#F6FAFC` (Base track)

---

### 4.2 RadialGradient (Circular Donut Charts)
MappingMode="Absolute" Center="55,55" GradientOrigin="55,55" RadiusX="53" RadiusY="53"

Notice the inverted offsets because Radius 53 is the outer perimeter (Offset 1.0) and Radius 37 is the inner perimeter (Offset 0.698):
- **Normal Green:**
  - `Offset 1.00`: `#9AEF4E` (Outer highlight rim)
  - `Offset 0.86`: `#40D622` (Mid body)
  - `Offset 0.78`: `#24B812` (Inner body)
  - `Offset 0.69`: `#1A9308` (Inner shadow rim)
- **Transfer Blue:**
  - `Offset 1.00`: `#6ECBFD`
  - `Offset 0.86`: `#269DE6`
  - `Offset 0.78`: `#107EC7`
  - `Offset 0.69`: `#095996`
- **Warning Amber:**
  - `Offset 1.00`: `#FFE162`
  - `Offset 0.86`: `#FFB812`
  - `Offset 0.78`: `#F09B00`
  - `Offset 0.69`: `#C97B00`
- **Critical Red:**
  - `Offset 1.00`: `#FF8484`
  - `Offset 0.86`: `#F03333`
  - `Offset 0.78`: `#D61818`
  - `Offset 0.69`: `#A30E0E`
- **Standby Gray:**
  - `Offset 1.00`: `#CBD4DE`
  - `Offset 0.86`: `#9EAEBE`
  - `Offset 0.78`: `#889BB0`
  - `Offset 0.69`: `#657A90`
- **Boşluk Track:**
  - `Offset 1.00`: `#DEE5EC`
  - `Offset 0.85`: `#EFF4F8`
  - `Offset 0.69`: `#F6FAFC`

---

## 5. 3D Crystal LED Spheres (DataGrid Status Indicators)

RadialGradientBrush with GradientOrigin="0.35,0.3" (Off-center glass highlight):
- **Green (Online):** Gradient `#70FF40` $\to$ `#108000`
- **Amber (Warning):** Gradient `#FFE050` $\to$ `#B07000`
- **Red (Critical):** Gradient `#FF6060` $\to$ `#B00000`
- **Blue (Maintenance):** Gradient `#60C0FF` $\to$ `#0050B0`
- **Gray (Inactive):** Gradient `#E0E8F0` $\to$ `#607080`
- **Black (Offline):** Gradient `#606060` $\to$ `#101010`
