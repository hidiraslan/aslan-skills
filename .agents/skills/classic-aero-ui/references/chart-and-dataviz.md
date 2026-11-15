# Windows Classic Aero UI - Data Visualization & Charts Reference

This document provides the mathematical formulas and production-ready XAML code for implementing enterprise data visualization widgets in WPF without relying on heavy third-party charting libraries.

---

## 1. Annular Donut Chart (Donut 1b - Aero Radial Gloss)

### Geometry Math
- Bounding Box: $110 \times 110$
- Center: $(C_x, C_y) = (55.0, 55.0)$
- Outer Radius: $R = 53.0$
- Inner Radius: $r = 37.0$

For any clockwise angle $\theta$ from 12 o'clock:
$$X = C_x + \text{radius} \cdot \sin(\theta)$$
$$Y = C_y - \text{radius} \cdot \cos(\theta)$$

### Transition Coordinates Table

| Segment Name | Percentage | Span ($\Delta\theta$) | End Angle ($\theta$) | Outer Coordinate $(X_o, Y_o)$ | Inner Coordinate $(X_i, Y_i)$ |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Start** | - | - | $0^\circ$ | $(55.0, 2.0)$ | $(55.0, 18.0)$ |
| **Normal (Green)** | 30% | $108^\circ$ | $108^\circ$ | $(105.4, 71.4)$ | $(90.2, 66.4)$ |
| **Transfer (Blue)** | 20% | $72^\circ$ | $180^\circ$ | $(55.0, 108.0)$ | $(55.0, 92.0)$ |
| **Warning (Amber)** | 15% | $54^\circ$ | $234^\circ$ | $(12.1, 86.2)$ | $(25.1, 76.7)$ |
| **Critical (Red)** | 10% | $36^\circ$ | $270^\circ$ | $(2.0, 55.0)$ | $(18.0, 55.0)$ |
| **Standby (Gray)** | 10% | $36^\circ$ | $306^\circ$ | $(12.1, 23.8)$ | $(25.1, 33.3)$ |
| **Boşluk (Track)** | 15% | $54^\circ$ | $360^\circ$ | $(55.0, 2.0)$ | $(55.0, 18.0)$ |

### Production XAML
```xml
<Grid Width="110" Height="110" HorizontalAlignment="Center" VerticalAlignment="Center">
    <!-- 1. Normal (Green) -->
    <Path StrokeThickness="0" SnapsToDevicePixels="True"
          Data="M 55.0,2.0 A 53,53 0 0,1 105.4,71.4 L 90.2,66.4 A 37,37 0 0,0 55.0,18.0 Z">
        <Path.Fill>
            <RadialGradientBrush Center="55,55" GradientOrigin="55,55" RadiusX="53" RadiusY="53" MappingMode="Absolute">
                <GradientStop Color="#9AEF4E" Offset="1.0"/>
                <GradientStop Color="#40D622" Offset="0.86"/>
                <GradientStop Color="#24B812" Offset="0.78"/>
                <GradientStop Color="#1A9308" Offset="0.69"/>
            </RadialGradientBrush>
        </Path.Fill>
    </Path>

    <!-- 2. Transfer (Blue) -->
    <Path StrokeThickness="0" SnapsToDevicePixels="True"
          Data="M 105.4,71.4 A 53,53 0 0,1 55.0,108.0 L 55.0,92.0 A 37,37 0 0,0 90.2,66.4 Z">
        <Path.Fill>
            <RadialGradientBrush Center="55,55" GradientOrigin="55,55" RadiusX="53" RadiusY="53" MappingMode="Absolute">
                <GradientStop Color="#6ECBFD" Offset="1.0"/>
                <GradientStop Color="#269DE6" Offset="0.86"/>
                <GradientStop Color="#107EC7" Offset="0.78"/>
                <GradientStop Color="#095996" Offset="0.69"/>
            </RadialGradientBrush>
        </Path.Fill>
    </Path>

    <!-- 3. Warning (Amber) -->
    <Path StrokeThickness="0" SnapsToDevicePixels="True"
          Data="M 55.0,108.0 A 53,53 0 0,1 12.1,86.2 L 25.1,76.7 A 37,37 0 0,0 55.0,92.0 Z">
        <Path.Fill>
            <RadialGradientBrush Center="55,55" GradientOrigin="55,55" RadiusX="53" RadiusY="53" MappingMode="Absolute">
                <GradientStop Color="#FFE162" Offset="1.0"/>
                <GradientStop Color="#FFB812" Offset="0.86"/>
                <GradientStop Color="#F09B00" Offset="0.78"/>
                <GradientStop Color="#C97B00" Offset="0.69"/>
            </RadialGradientBrush>
        </Path.Fill>
    </Path>

    <!-- 4. Critical (Red) -->
    <Path StrokeThickness="0" SnapsToDevicePixels="True"
          Data="M 12.1,86.2 A 53,53 0 0,1 2.0,55.0 L 18.0,55.0 A 37,37 0 0,0 25.1,76.7 Z">
        <Path.Fill>
            <RadialGradientBrush Center="55,55" GradientOrigin="55,55" RadiusX="53" RadiusY="53" MappingMode="Absolute">
                <GradientStop Color="#FF8484" Offset="1.0"/>
                <GradientStop Color="#F03333" Offset="0.86"/>
                <GradientStop Color="#D61818" Offset="0.78"/>
                <GradientStop Color="#A30E0E" Offset="0.69"/>
            </RadialGradientBrush>
        </Path.Fill>
    </Path>

    <!-- 5. Standby (Gray) -->
    <Path StrokeThickness="0" SnapsToDevicePixels="True"
          Data="M 2.0,55.0 A 53,53 0 0,1 12.1,23.8 L 25.1,33.3 A 37,37 0 0,0 18.0,55.0 Z">
        <Path.Fill>
            <RadialGradientBrush Center="55,55" GradientOrigin="55,55" RadiusX="53" RadiusY="53" MappingMode="Absolute">
                <GradientStop Color="#CBD4DE" Offset="1.0"/>
                <GradientStop Color="#9EAEBE" Offset="0.86"/>
                <GradientStop Color="#889BB0" Offset="0.78"/>
                <GradientStop Color="#657A90" Offset="0.69"/>
            </RadialGradientBrush>
        </Path.Fill>
    </Path>

    <!-- 6. Boşluk (Free Space Track) -->
    <Path StrokeThickness="0" SnapsToDevicePixels="True"
          Data="M 12.1,23.8 A 53,53 0 0,1 55.0,2.0 L 55.0,18.0 A 37,37 0 0,0 25.1,33.3 Z">
        <Path.Fill>
            <RadialGradientBrush Center="55,55" GradientOrigin="55,55" RadiusX="53" RadiusY="53" MappingMode="Absolute">
                <GradientStop Color="#DEE5EC" Offset="1.0"/>
                <GradientStop Color="#EFF4F8" Offset="0.85"/>
                <GradientStop Color="#F6FAFC" Offset="0.69"/>
            </RadialGradientBrush>
        </Path.Fill>
    </Path>

    <!-- 1px Radial Divider Lines -->
    <Line X1="55.0" Y1="2.0"   X2="55.0" Y2="18.0" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"/>
    <Line X1="105.4" Y1="71.4" X2="90.2" Y2="66.4" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"/>
    <Line X1="55.0" Y1="108.0" X2="55.0" Y2="92.0" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"/>
    <Line X1="12.1"  Y1="86.2" X2="25.1" Y2="76.7" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"/>
    <Line X1="2.0"   Y1="55.0" X2="18.0" Y2="55.0" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"/>
    <Line X1="12.1"  Y1="23.8" X2="25.1" Y2="33.3" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True"/>

    <!-- Outer & Inner 1px Enclosing Tracks -->
    <Ellipse Width="106" Height="106" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True" IsHitTestVisible="False"/>
    <Ellipse Width="74" Height="74" Stroke="#8B9EB5" StrokeThickness="1" SnapsToDevicePixels="True" IsHitTestVisible="False"/>
</Grid>
```

---

## 2. Semi-Circular Dial / Hardware Capacity Gauge

A clean $180^\circ$ radial gauge with a pivoting red needle and separated bottom labels:

```xml
<Grid Width="160" Height="90" HorizontalAlignment="Center">
    <!-- Outer Inactive Arc Track -->
    <Path Stroke="#E2EAF2" StrokeThickness="12" StrokeStartLineCap="Round" StrokeEndLineCap="Round"
          Data="M 15,75 A 65,65 0 0,1 145,75"/>

    <!-- Active Value Arc (e.g. 68.4%) -->
    <Path Stroke="#269DE6" StrokeThickness="12" StrokeStartLineCap="Round"
          Data="M 15,75 A 65,65 0 0,1 120,30"/>

    <!-- Needle Pivot Base -->
    <Ellipse Width="12" Height="12" Fill="#B80E0E" HorizontalAlignment="Center" VerticalAlignment="Bottom" Margin="0,0,0,10"/>

    <!-- Rotating Indicator Needle -->
    <Line X1="80" Y1="75" X2="120" Y2="25" Stroke="#B80E0E" StrokeThickness="2.5" StrokeStartLineCap="Round" StrokeEndLineCap="Round"/>

    <!-- 0% and 100% Endpoint Labels -->
    <TextBlock Text="0%" FontFamily="Segoe UI" FontSize="9" Foreground="#6C839C" HorizontalAlignment="Left" VerticalAlignment="Bottom" Margin="8,0,0,-4"/>
    <TextBlock Text="100%" FontFamily="Segoe UI" FontSize="9" Foreground="#6C839C" HorizontalAlignment="Right" VerticalAlignment="Bottom" Margin="0,0,8,-4"/>
</Grid>
```

---

## 3. Milestone Pipeline (Centrally Aligned Timeline)

```xml
<Grid Height="40" Margin="10,0">
    <!-- Connecting Timeline Track (Vertically Centered at Y=20) -->
    <Line X1="20" Y1="20" X2="280" Y2="20" Stroke="#CBD8E6" StrokeThickness="2"/>

    <!-- 4 Milestones -->
    <!-- Milestone 1: Done -->
    <Ellipse Width="14" Height="14" Fill="#24B812" HorizontalAlignment="Left" Margin="13,0,0,0" VerticalAlignment="Center"/>
    <!-- Milestone 2: Done -->
    <Ellipse Width="14" Height="14" Fill="#24B812" HorizontalAlignment="Left" Margin="93,0,0,0" VerticalAlignment="Center"/>
    <!-- Milestone 3: In Progress (Pulse Glow) -->
    <Ellipse Width="16" Height="16" Fill="#107EC7" HorizontalAlignment="Left" Margin="172,0,0,0" VerticalAlignment="Center"/>
    <!-- Milestone 4: Pending -->
    <Ellipse Width="12" Height="12" Fill="#D0DFEE" HorizontalAlignment="Left" Margin="254,0,0,0" VerticalAlignment="Center"/>
</Grid>
```
