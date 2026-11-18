# Windows Classic Aero UI - Vektör İkon Tasarım ve Üretim Rehberi (Icon Creation Guide)

Bu kılavuz, `classic-aero-ui` tasarım sistemine yeni ikonlar eklerken veya mevcut ikonları güncellerken **%100 stil birliği ve görsel kusursuzluk** sağlamak için uyulması zorunlu olan mimari kuralları ve şablonları içerir.

---

## 1. Altın Kurallar (Golden Rules of Aero Iconography)

1. **Format:** Tüm ikonlar `DrawingBrush` olarak `Assets/Icons/ClassicIcons.xaml` içinde tanımlanmalıdır. Asla harici PNG, JPG, BMP veya font ikon (FontAwesome, Segoe MDL2, Material) kullanılmaz.
2. **Tuval ve Koordinat Sistemi (ViewBox & Canvas):**
   - Referans çizim alanı `16x16` (küçük detaylar) veya `24x24` (büyük detaylar) koordinat sisteminde çizilir.
   - Tüm koordinatlar `0` ile `16` (veya `24`) sınırları içinde kalmalıdır.
3. **Tek Parça Döküm Kuralı (Unified Geometry):**
   - Nesneler (örn. anahtarın halkası ve sapı, kilit gövdesi ve kancası) parçalı dikdörtgenler veya bağımsız daireler şeklinde yan yana konulmaz.
   - Birbiriyle temas eden ana parçalar `GeometryCombineMode="Union"` ile matematiksel olarak tek dış silüette birleştirilir. Aksi halde parçalar arasında yapay ayrım çizgileri oluşur.
4. **Çizgi ve Kontur Standardı (0.8px Law):**
   - Ana dış kontur daima `Thickness="0.8"` (veya `0.7` - `0.9`) olmalıdır. Asla 2px ve üzeri kalın/kaba kenarlıklar kullanılmaz.
   - Kontur rengi, dolgu renginin koyu/derin tonu olmalıdır (örn. altın için `#5E3E00`, çelik/mavi için `#15304D`, yeşil için `#1E4D09`).
5. **Silindirik Işık ve Yansıma (Aero Lighting):**
   - Üstten alta silindirik gradyan (`StartPoint="0,0" EndPoint="0,1"`).
   - Üst kenarda 1px yarım saydam beyaz yansıma çizgisi (`Pen Brush="#80FFFFFF" Thickness="0.7"`).
6. **Açık Çizgilerde Pen Zorunluluğu:**
   - Ucu açık çizgiler (`M x,y L x,y` şeklinde kapanmayan yollar) kesinlikle `Brush` ile boyanamaz (WPF'te alan 0 olduğu için görünmez olurlar). Mutlaka `<GeometryDrawing.Pen>` kullanılmalıdır.

---

## 2. İkon Renk Paleti ve Aydınlatma Standartları

| Malzeme / Tür | Dış Kontur (Pen) | Gövde Gradyanı (LinearGradient 0,0 $\to$ 0,1) | Üst Yansıma (Specular) |
| :--- | :--- | :--- | :--- |
| **Aero Mavi (Sistem & UI)** | `#15304D` | `#3E6C99` $\to$ `#244C73` $\to$ `#163452` | `#60FFFFFF` |
| **Kristal Cam Mercek** | `#3C719C` | `#EAF4FC` $\to$ `#9CC5E8` $\to$ `#4B8EC4` | `#FFFFFF` (Hilal Yayı) |
| **Döküm Pirinç / Altın** | `#5E3E00` | `#FFF6B2` $\to$ `#EDB817` $\to$ `#7A5200` | `#A0FFFFFF` |
| **Krom / Fırçalanmış Metal**| `#55687A` | `#FFFFFF` $\to$ `#D0DCE6` $\to$ `#9AB0C4` | `#FFFFFF` |
| **Zümrüt Yeşil (Başarı/Çalışma)**| `#15540D` | `#A5EB52` $\to$ `#42C81E` $\to$ `#1C7D08` | `#80FFFFFF` |
| **Yakut Kırmızı (Kritik/Durdur)**| `#6B0A0A` | `#FF7A7A` $\to$ `#E02424` $\to$ `#8A0A0A` | `#80FFFFFF` |
| **Kehribar / Klasik Sarı Klasör**| `#9C6B00` | `#FFE885` $\to$ `#F3BA1D` $\to$ `#C28800` | `#60FFFFFF` |

---

## 3. Hazır Kod Şablonları (Templates)

### Şablon A: Standart 3D Aero Gövde ve Yansıma Şablonu
```xml
<!-- [No]. [İkon Adı] -->
<DrawingBrush x:Key="Icon[OrnekAdi]" Stretch="Uniform">
    <DrawingBrush.Drawing>
        <DrawingGroup>
            <!-- 1. Ana Gövde ve Koyu Kontur -->
            <GeometryDrawing>
                <GeometryDrawing.Brush>
                    <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
                        <GradientStop Color="#[AçıkTon]" Offset="0.0"/>
                        <GradientStop Color="#[OrtaTon]" Offset="0.5"/>
                        <GradientStop Color="#[KoyuTon]" Offset="1.0"/>
                    </LinearGradientBrush>
                </GeometryDrawing.Brush>
                <GeometryDrawing.Pen>
                    <Pen Brush="#[KoyuKonturRengi]" Thickness="0.8"/>
                </GeometryDrawing.Pen>
                <GeometryDrawing.Geometry>
                    <PathGeometry Figures="M ... Z"/>
                </GeometryDrawing.Geometry>
            </GeometryDrawing>

            <!-- 2. Üst Kenar Aero Cam Yansıması (Specular Highlight) -->
            <GeometryDrawing>
                <GeometryDrawing.Pen>
                    <Pen Brush="#80FFFFFF" Thickness="0.7" StartLineCap="Round" EndLineCap="Round"/>
                </GeometryDrawing.Pen>
                <GeometryDrawing.Geometry>
                    <PathGeometry Figures="M ... L ..."/>
                </GeometryDrawing.Geometry>
            </GeometryDrawing>
        </DrawingGroup>
    </DrawingBrush.Drawing>
</DrawingBrush>
```

### Şablon B: Çoklu Parçayı Birleştiren Döküm Geometrisi (Union Şablonu)
```xml
<!-- İki parçanın (daire + sap) tek parça halinde kaynaması -->
<CombinedGeometry GeometryCombineMode="Union">
    <CombinedGeometry.Geometry1>
        <EllipseGeometry Center="6.5, 7.5" RadiusX="4.5" RadiusY="4.5"/>
    </CombinedGeometry.Geometry1>
    <CombinedGeometry.Geometry2>
        <PathGeometry Figures="M 6.5,6.3 L 20.0,6.3 L 20.0,8.7 L 6.5,8.7 Z"/>
    </CombinedGeometry.Geometry2>
</CombinedGeometry>
```

---

## 4. Kullanım ve Çağırma Standartları
Oluşturulan her ikon XAML içinde doğrudan şu boyut standartlarıyla çağrılır:
```xml
<!-- 16x16: Tablo Satırı veya Menü -->
<Rectangle Width="16" Height="16" Fill="{DynamicResource Icon[OrnekAdi]}"/>

<!-- 24x24: Standart Buton veya Araç Çubuğu -->
<Rectangle Width="24" Height="24" Fill="{DynamicResource Icon[OrnekAdi]}"/>

<!-- 48x48: Yüksek Çözünürlüklü Dokunmatik / Karşılama Kartı -->
<Rectangle Width="48" Height="48" Fill="{DynamicResource Icon[OrnekAdi]}"/>
```

---

## 5. Uygulama Ana İkonu (Application Favicon & Logo) Standartları

1. **Rastgele Harflerden Kaçının:** Kullanıcı özel bir monogram/baş harf logosu istemedikçe asla "D", "A" gibi rastgele harf içeren statik ikonlar kullanılmaz.
2. **Program Türüne Göre Semantik Amblem:**
   - Uygulamanın alanına göre sembol seçilir: Veritabanı için kristal silindir/veri amblemi, Güvenlik için Aero kalkan/anahtar, Kodlama için terminal/parantez amblemi, Medya için ses dalgası/disk.
3. **Genel Kurumsal Standart:**
   - Özel bir alan belirtilmemişse veya genel masaüstü aracı ise nötr **Classic Aero Crystal Diamond** (3D Aero kristal cam pencere amblemi `IconAppLogo`) kullanılır.
4. **Vektör Üretim Şablonu:** Yeni uygulama ikonları dışta koyu 3D bevel, içte silindirik Aero cam gradyanı ve üstte hilal yansıması (specular arc) taşıyarak Windows 7 kristal cam derinliğini tam yansıtmalıdır.
