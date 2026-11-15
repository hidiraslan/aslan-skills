# Windows Classic Aero UI - Component Generation DNA (Tasarım Genomu)

This guide defines the fundamental architectural rules for synthesizing **NEW, CUSTOM, or UNSEEN** UI components in a way that guarantees 100% visual and structural harmony with the Classic Aero Enterprise UI design system.

Whenever a user requests a component not explicitly defined in the template library (e.g. a Log Viewer, a Kanban Board, a Syntax-Highlighted Editor, a Timeline, a Custom Speedometer, an Interactive Topology Map, a Toast Notification, or an Accordion):
**DO NOT INVENT FLAT MODERN STYLES. FOLLOW THIS COMPONENT DNA.**

---

## 1. Surface & Canvas Layering Hierarchy

Every interface element must live on a well-defined depth plane:

1. **Layer 0 (Window Base Canvas):** `#F4F6F9` or `#F0F4F9` (Subtle cool slate-gray).
2. **Layer 1 (Main Structural Containers):** Pure White `#FFFFFF` with a 1px border (`#708FA8` for top-level frames, `#B0C8DF` for cards).
3. **Layer 2 (Inner Scrollable Workspace):** `#F8FAFD` (Aero Ice Mist).
4. **Layer 3 (Headers, Strips & Footers):**
   - Sub-Header Bar: `#FFFFFF` with `#D0DFEE` bottom border (Height 34).
   - KPI / Metric Strips: `#F6F9FD` with `#D0DFEE` bottom border.
   - Command Footers: `#F0F4F8` with `#D0DFEE` top border (Height 42-46).
   - StatusBar: `#EAF0F6` with `#BACFE4` top border (Height 24).

---

## 2. Border & Geometry Rules (The 1px Law)

1. **All dividing lines must be exactly 1px solid:**
   - Table gridlines, card borders, tab borders, header dividers.
   - Never use 0px borderless flat containers.
   - Never use 2px or 3px heavy outlines.
2. **Device Pixel Snapping:**
   - Always declare `SnapsToDevicePixels="True"` and `UseLayoutRounding="True"` on borders, lines, and shapes.
3. **Corner Radii:**
   - Windows, Frames, Tables: `0px` (Square, crisp).
   - Cards, GroupBoxes, Dialogs: `2px` to `4px` (Subtle softening).
   - Stepper Buttons, Badges, Chips: `2px` to `3px`.
   - Never use 12px, 16px, or 24px modern pill shapes (except circular LED dots and Pill Switches).

---

## 3. Aero Lighting & Gradient Physics

Classic Aero conveys physical depth through vertical and radial cylindrical lighting:

### 3.1 Linear Surfaces (Buttons, Progress Bars, Toolbars)
- Vector: `StartPoint="0,0" EndPoint="0,1"` (Strictly Top-to-Bottom).
- **Top Offset 0.00:** Specular Highlight (Light reflection).
- **Mid Offset 0.50:** Soft transition.
- **Base Offset 0.80 - 1.00:** Deeper shaded rim (tactile substance).
- *Rule:* Never use diagonal or horizontal linear gradients across status bars.

### 3.2 Radial Surfaces (Donuts, Dials, LED Spheres)
- Absolute Center Mapping.
- Outer edge gets the highlight reflection, inner hole/core gets the shadow.

---

## 4. Typography Hierarchy (Segoe UI)

| Role | Font Family | Size | Weight | Color | Usage |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Main Window / Section Title** | Segoe UI | 12pt - 13pt | SemiBold | `#1E395B` | Page titles, primary cards |
| **Sub-Header Subtitle** | Segoe UI | 11pt | Regular | `#6B88A4` | Explanations, metadata |
| **Table Header** | Segoe UI | 11pt | SemiBold | `#1E395B` | DataGrid columns |
| **Table Data / Input** | Segoe UI | 11pt | Regular | `#222222` | Cell values, TextBox text |
| **Dialog Headline** | Segoe UI | 12.5pt - 13pt | SemiBold | `#003399` | TaskDialog primary prompt |
| **KPI Big Number** | Segoe UI | 22pt - 24pt | Bold | `#1E395B` | Primary telemetry statistics |
| **Timestamp / Secondary** | Segoe UI | 9.5pt - 10pt | Regular | `#6C839C` | Dates, minor labels |

---

## 5. Status & Semantic Colors (Aero Enterprise)

When introducing new badges, alert chips, charts, or indicators:
- **Normal / Success / Online:** Emerald Green (`#24B812` core, `#9AEF4E` highlight, `#1A9308` shadow).
- **Active / Primary / In-Progress:** Royal Blue (`#2072BE` core, `#6ECBFD` highlight, `#095996` shadow).
- **Warning / Degraded / Caution:** Amber Gold (`#F09B00` core, `#FFE162` highlight, `#C97B00` shadow).
- **Critical / Fault / Danger:** Crimson Red (`#D61818` core, `#FF8484` highlight, `#A30E0E` shadow).
- **Standby / Maintenance / Inactive:** Slate Gray (`#889BB0` core, `#CBD4DE` highlight, `#657A90` shadow).
- **Offline / Disabled:** Charcoal Carbon (`#444444` core, `#777777` highlight).

---

## 6. How to Synthesize Any Specific New Control

### Case A: A Log Viewer / Console Window
- Frame: 1px `#708FA8` border.
- Sub-header: Standard 34px bar with "System Logs & Diagnostic Output", level filter combo, and Clear/Export buttons.
- Content: `TextBox` or `ListView` with `FontFamily="Consolas, Segoe UI"` `FontSize="11"`, Background `#FFFFFF`, alternating row background `#F8FAFD`.
- Status indicators: 3D LED dots next to error/warning lines.

### Case B: A Kanban / Workflow Board
- Background: `#F8FAFD`.
- Columns: Cards with `#FFFFFF` background, 1px `#B0C8DF` border, `CornerRadius="3"`.
- Column Header: `#F0F4F8` with `#D0DFEE` bottom border and small badge count.
- Items: Draggable white cards with `#DFE7F0` border and subtle Aero hover glow (`#A0C5EB`).

### Case C: An Interactive Topology Node / Diagram
- Node shape: Rounded rectangle (`CornerRadius="4"`) with `#FFFFFF` fill and 1px `#708FA8` border.
- Header: Subtle linear gradient (`#EAF2FA` to `#FFFFFF`) with 16x16 device icon and `#1E395B` title.
- Port badges: Mini 3D LED indicators (Green for Link Up, Gray for Link Down).
- Connectors: 1px `#8B9EB5` solid lines with centered status pills.
