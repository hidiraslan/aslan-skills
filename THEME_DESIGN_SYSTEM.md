# Classic Windows 7 / Aero Enterprise Desktop UI Design System

Bu doküman, projede oluşturulan Windows 7 / Aero dönemi klasik kurumsal masaüstü yazılımı tasarım sisteminin (D-Link Network Assistant, NetSetMan ve Windows Yönetim Araçları stili) mimarisini, renk paletini, bileşen kataloğunu ve **gelecekteki kendi C# WPF projelerinizde sıfırdan nasıl kullanabileceğinizi** adım adım açıklamaktadır.

---

## 1. Hızlı Başlangıç: Yeni Bir Projeye Entegrasyon

Bu tasarım sistemi **tamamen modüler ve tak-çalıştır (plug & play)** olarak tasarlanmıştır. Gelecekte oluşturacağınız herhangi bir .NET 8, .NET 9 veya .NET 10 WPF projesinde temayı kullanmak için yalnızca şu iki adımı uygulamanız yeterlidir:

### Adım 1: Klasörleri Projenize Kopyalayın
Aşağıdaki iki klasörü yeni projenizin ana dizinine kopyalayın:
- `Themes/` (Tüm XAML stil sözlükleri)
- `Assets/` (Vektörel çizim ikonları - `ClassicIcons.xaml`)

### Adım 2: `App.xaml` İçerisine Master Temayı Dahil Edin
Yeni projenizin `App.xaml` dosyasına tek satırda master temayı ekleyin:

```xaml
<Application x:Class="MyApplication.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <!-- Tek satırda tüm Windows 7 Enterprise temasını yükler -->
                <ResourceDictionary Source="pack://application:,,,/Themes/ClassicWindowsTheme.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

> **Önemli Avantaj:** `Themes/ClassicWindowsTheme.xaml` dosyası, tüm standart WPF kontrolleri (`Button`, `TextBox`, `ComboBox`, `CheckBox`, `RadioButton`, `DataGrid`, `TreeView`, `GroupBox`, `ScrollBar`, `ContextMenu`, `MenuItem`, `StatusBar`, `ProgressBar`, `Slider`) için **varsayılan (implicit) stiller** tanımlar. Dolayısıyla her butonun veya metin kutusunun içine ayrı ayrı `Style="{StaticResource ...}"` yazmak zorunda kalmazsınız; standart bir `<Button Content="Tamam"/>` eklediğinizde otomatik olarak Windows 7 stiline bürünür!

---

## 2. Windows ClearType & Keskin Yazı Ayarları

WPF varsayılan olarak yazı tiplerini baskı mantığıyla (Ideal mode) subpixel olarak çizer; bu durum Windows masaüstü programlarında yazıların hafif buzlu/bulanık görünmesine yol açabilir. Yazıların Windows 7 yerel Win32 programları gibi **jilet gibi net ve piksel hizalı** görünmesi için her `<Window>` kök etiketine şu özellikleri ekleyin:

```xaml
<Window ...
        FontFamily="Segoe UI, Tahoma, sans-serif"
        FontSize="11"
        SnapsToDevicePixels="True"
        UseLayoutRounding="True"
        TextOptions.TextFormattingMode="Display"
        TextOptions.TextRenderingMode="ClearType"
        TextOptions.TextHintingMode="Fixed"
        RenderOptions.ClearTypeHint="Enabled">
```

---

## 3. Renk Paleti ve Fırça Belirteçleri (Color Tokens)

| Token İsmi | Hex / Renk | Kullanım Alanı |
| :--- | :--- | :--- |
| `ClassicWindowBackground` | `#EDF2F8` / `#F0F4F9` | Pencerelerin ana arka planı ve dialog dolguları |
| `ClassicBorderBrush` | `#A0B4D0` / `#C2D1E4` | GroupBox, DataGrid ve kart dış çerçeve çizgileri (1px) |
| `ClassicTextPrimary` | `#1E395B` | Başlıklar, sekmeler ve koyu mavi kurumsal metinler |
| `ClassicTextDark` | `#000000` | Grid satırları, girdi kutuları ve veri metinleri |
| `ClassicTextMuted` | `#556677` / `#6B7F96` | Pasif metinler, açıklamalar ve kısayol tuşları |
| `ClassicSelectionBlue` | `#1A84E3` -> `#0B6EC6` | Windows 7 kraliyet mavisi seçili durum degradeleri (Seçili satır, aktif liste elemanı) |
| `ClassicButtonHover` | `#EAF6FD` -> `#BEE6FD` | Buton ve sekmelerin üzerine gelindiğinde oluşan açık mavi Aero ışıltısı |
| `IconStatusOnline` | `#4AD818` / `#189602` | Parlak bombeli yeşil durum LED göstergesi |

---

## 4. Modüler Dosya Yapısı (Design Architecture)

```
Themes/
├── ClassicWindowsTheme.xaml    <-- Master tema sözlüğü (Tüm sözlükleri birleştirir + varsayılan stiller)
├── ButtonStyles.xaml           <-- Standart [OK], [Cancel], Ribbon ve Uygulama Pill butonu
├── ComboBoxStyles.xaml         <-- Windows 7 açılır liste (droplist) ve aşağı ok butonu
├── DataGridStyles.xaml         <-- 22px kompakt kurumsal grid, gradient başlıklar ve mavi seçili satır
├── GroupBoxStyles.xaml         <-- 4 kenarı kapalı, başlığı çizgiyi maskeleyen grup kutusu
├── MenuStyles.xaml             <-- Sol ikon kanallı Windows 7 ContextMenu ve MenuItem
├── RadioButtonStyles.xaml      <-- Klasik 13x13 yuvarlak mavi/yeşil merkezli radyo butonu
├── ScrollBarStyles.xaml        <-- 17px Aero gradient ok butonları, cam thumb ve tutamaç çizgileri
├── SliderProgressBarStyles.xaml <-- Yeşil gradient ilerleme çubuğu ve donanım kalibrasyon slider'ı
├── TabStyles.xaml              <-- Ribbon tabları, özellik sayfası (PropertySheet) tabları
├── TextBoxStyles.xaml          <-- Dikey ortalanmış TextBox, PasswordBox ve SearchBox
├── ToolbarStyles.xaml          <-- Ribbon üst bant arka planı ve grup ayırıcıları
└── TreeViewStyles.xaml         <-- Klasik port/arayüz hiyerarşisi ağaç görünümü

Assets/
└── Icons/
    └── ClassicIcons.xaml       <-- Tüm çözünürlüklerde pürüzsüz vektörel (DrawingBrush) simgeler
```

---

## 5. Hazır Bileşenler ve XAML Kullanım Örnekleri

### 5.1. Butonlar
```xaml
<!-- Standart Windows 7 Butonu -->
<Button Content="Restore Defaults" Width="110"/>

<!-- Varsayılan (Default - Mavi Çerçeveli) Buton -->
<Button Content="OK" Style="{StaticResource ClassicDefaultButton}" Width="75"/>

<!-- İptal / Standart Dialog Butonu -->
<Button Content="Cancel" Style="{StaticResource ClassicStandardButton}" Width="75"/>

<!-- Küçük Araç Çubuğu Butonu (İkonlu) -->
<Button Style="{StaticResource ClassicSmallToolButton}" Padding="8,1">
    <StackPanel Orientation="Horizontal">
        <Rectangle Width="13" Height="13" Fill="{DynamicResource IconRefresh}" Margin="0,0,4,0"/>
        <TextBlock Text="Refresh" VerticalAlignment="Center"/>
    </StackPanel>
</Button>
```

### 5.2. Grup Kutusu (GroupBox)
```xaml
<GroupBox Header="Management Protocols" Style="{StaticResource ClassicGroupBox}">
    <StackPanel Margin="6,4">
        <CheckBox Content="Enable D-Link Discovery Protocol (DDP v2)" IsChecked="True"/>
        <CheckBox Content="Enable IEEE 802.3az Energy Efficient Ethernet (EEE)" IsChecked="False"/>
    </StackPanel>
</GroupBox>
```

### 5.3. Açılır Liste (ComboBox) & Metin Kutusu (TextBox)
```xaml
<!-- Windows 7 Droplist -->
<ComboBox SelectedIndex="0" Width="200">
    <ComboBoxItem Content="HTTP Web Management (Port 80)"/>
    <ComboBoxItem Content="HTTPS Secure Web (Port 443)"/>
</ComboBox>

<!-- Ortalı Metin Kutusu -->
<TextBox Text="192.168.1.1" Width="150"/>

<!-- Şifre Kutusu -->
<PasswordBox Width="150"/>
```

### 5.4. Sol Sidebar Navigasyonlu Seçenekler / Ayarlar Penceresi
Sol kenar çubuğu olan bir ayarlar penceresi oluşturmak için:
```xaml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="200"/> <!-- Sol Sidebar -->
        <ColumnDefinition Width="*"/>   <!-- Sağ İçerik -->
    </Grid.ColumnDefinitions>

    <!-- Sol Liste -->
    <ListBox x:Name="SidebarListBox"
             SelectedIndex="0"
             ItemContainerStyle="{StaticResource ClassicSidebarListBoxItem}">
        <ListBoxItem IsSelected="True">
            <StackPanel Orientation="Horizontal">
                <Rectangle Width="16" Height="16" Fill="{DynamicResource IconGeneral}" Margin="0,0,8,0"/>
                <TextBlock Text="General Settings" VerticalAlignment="Center"/>
            </StackPanel>
        </ListBoxItem>
        <ListBoxItem>
            <StackPanel Orientation="Horizontal">
                <Rectangle Width="16" Height="16" Fill="{DynamicResource IconDiscovery}" Margin="0,0,8,0"/>
                <TextBlock Text="Device Discovery" VerticalAlignment="Center"/>
            </StackPanel>
        </ListBoxItem>
    </ListBox>

    <!-- Sağ Sayfa İçeriği: Sidebar SelectedIndex'e bağlı TabControl -->
    <TabControl Grid.Column="1"
                SelectedIndex="{Binding SelectedIndex, ElementName=SidebarListBox}">
        <TabControl.ItemContainerStyle>
            <Style TargetType="TabItem"><Setter Property="Visibility" Value="Collapsed"/></Style>
        </TabControl.ItemContainerStyle>
        <TabItem>
            <!-- 1. Sayfa İçeriği -->
        </TabItem>
        <TabItem>
            <!-- 2. Sayfa İçeriği -->
        </TabItem>
    </TabControl>
</Grid>
```

### 5.5. Windows 7 Açılır Uygulama Menüsü (Application Menu)
```xaml
<Button Style="{StaticResource ClassicAppPillButton}">
    <Button.ContextMenu>
        <ContextMenu Style="{StaticResource ClassicContextMenu}">
            <MenuItem Header="New Network Scan" InputGestureText="Ctrl+N">
                <MenuItem.Icon>
                    <Rectangle Width="16" Height="16" Fill="{DynamicResource IconDiscovery}"/>
                </MenuItem.Icon>
            </MenuItem>
            <MenuItem Header="Save Device List" InputGestureText="Ctrl+S">
                <MenuItem.Icon>
                    <Rectangle Width="16" Height="16" Fill="{DynamicResource IconSave}"/>
                </MenuItem.Icon>
            </MenuItem>
            <Separator Style="{StaticResource ClassicMenuSeparator}"/>
            <MenuItem Header="Exit" InputGestureText="Alt+F4"/>
        </ContextMenu>
    </Button.ContextMenu>
</Button>
```

### 5.6. Renkli Aero Progress Barlar (Mavi, Sarı, Kırmızı, Yeşil)
Windows 7 cam yansımalı (Aero gloss) ilerleme çubukları farklı donanım durumları için renk seçeneklerine sahiptir:
```xaml
<!-- Standart Yeşil (Normal / İyi durum) -->
<ProgressBar Value="35" Style="{StaticResource ClassicProgressBar}"/>

<!-- Aero Mavi (CPU / Bilgi durumu) -->
<ProgressBar Value="28" Style="{StaticResource ClassicProgressBarBlue}"/>

<!-- Aero Sarı / Amber (Bellek / Uyarı durumu) -->
<ProgressBar Value="64" Style="{StaticResource ClassicProgressBarYellow}"/>

<!-- Aero Kırmızı (Disk / Kritik durum) -->
<ProgressBar Value="85" Style="{StaticResource ClassicProgressBarRed}"/>
```

### 5.7. Ribbon Genişletilmiş Kontrolleri
Genişletilmiş Ribbon araç çubuğu standart büyük butonların yanı sıra basılı kalabilen (toggle) ve dikey mini butonları destekler:
```xaml
<!-- Basılı Kalabilen Ribbon Toggle Butonu (Örn: Otomatik Yenileme) -->
<ToggleButton Style="{StaticResource ClassicRibbonToggleButton}" ToolTip="Otomatik Tarama">
    <StackPanel HorizontalAlignment="Center">
        <Rectangle Width="24" Height="24" Fill="{DynamicResource IconRefresh}"/>
        <TextBlock Text="Auto&#x0a;Refresh" TextAlignment="Center"/>
    </StackPanel>
</ToggleButton>

<!-- Dikey Sıralı Mini Butonlar (Kompakt Aksiyon Grubu) -->
<StackPanel VerticalAlignment="Center">
    <Button Style="{StaticResource ClassicRibbonMiniButton}">
        <StackPanel Orientation="Horizontal">
            <Rectangle Width="12" Height="12" Fill="{DynamicResource IconStatusOnline}" Margin="0,0,4,0"/>
            <TextBlock Text="Select All"/>
        </StackPanel>
    </Button>
    <Button Style="{StaticResource ClassicRibbonMiniButton}">
        <StackPanel Orientation="Horizontal">
            <Rectangle Width="12" Height="12" Fill="{DynamicResource IconReset}" Margin="0,0,4,0"/>
            <TextBlock Text="Clear Selection"/>
        </StackPanel>
    </Button>
</StackPanel>
```

### 5.8. Klasik Windows 7 Diyalog Ekranları
1. **Windows 7 Task Dialog (Onay Ekranı):**
   Üstü beyaz bilgi paneli (32px simge + koyu mavi başlık + detay metni + "Bir daha gösterme" onay kutusu), altı klasik gri komut çubuğu (`Views/ConfirmDialog.xaml`).
2. **Ağ Tanılama Aracı (ICMP Ping Dialog):**
   Hedef IP, paket sayısı, buffer boyutu ve timeout parametreleri ile altta monospace konsol çıktısı veren Windows yardımcı program penceresi (`Views/DiagnosticPingDialog.xaml`).

---

## 6. Projede Yer Alan Örnek Pencereler ve Diyaloglar

1. **`MainWindow.xaml`**: Ana yönetim konsolu (Common & Advanced Ribbon sekmeleri, ToggleButton, dikey mini butonlar, açılır uygulama menüsü, 16 sütunlu kompakt DataGrid, arama kutusu ve durum çubuğu).
2. **`SettingsWindow.xaml`**: Sol sidebar navigasyonlu çok sayfalı kurumsal ayarlar penceresi (6 kategori, GroupBox'lar, Checkbox'lar, Combobox'lar).
3. **`DeviceSettingsWindow.xaml`**: 4 sekmeli özellik sayfası (Port arayüz TreeView, Mavi/Sarı/Kırmızı/Yeşil CPU/RAM/Flash/Buffer ilerleme çubukları, PoE güç slider'ı, ağ parametreleri).
4. **`TimeSettingsWindow.xaml`**: Dinamik 2 adımlı chevron sihirbazı (1. Adımda Next > ile 2. Adıma geçiş, 2. Adımda < Previous'a dönüşme, özet tablosu ve kimlik doğrulama).
5. **`DiagnosticPingDialog.xaml`**: Monospace konsol çıktılı kurumsal ICMP Ping tanı aracı penceresi.
6. **`ConfirmDialog.xaml`**: Windows Vista/7 TaskDialog stilinde yeniden başlatma onay penceresi.
7. **`AboutWindow.xaml`**: Windows 7 stili "Hakkında" modal bilgi iletişim kutusu.

