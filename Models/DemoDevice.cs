using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassicAeroStudio.Models
{
    public class DemoDevice : INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public bool IsOnline { get; set; } = true;
        public string StatusCode { get; set; } = "Online"; // "Online" (Green), "Warning" (Yellow), "Critical" (Red), "Maintenance" (Blue), "Inactive" (Gray), "Disabled" (Black)
        public string StatusTooltip { get; set; } = "Online";
        public string ModelName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string IPv4 { get; set; } = string.Empty;
        public string IPv6 { get; set; } = string.Empty;
        public string DeviceTime { get; set; } = string.Empty;
        public string Interface { get; set; } = string.Empty;
        public string FwVersion { get; set; } = string.Empty;
        public string HwVersion { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string IpMode { get; set; } = string.Empty;
        public string ProtocolVersion { get; set; } = string.Empty;
        public string DdpInformation { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ObservableCollection<DemoDevice> GetSampleDevices()
        {
            return new ObservableCollection<DemoDevice>
            {
                new DemoDevice
                {
                    IsSelected = true,
                    IsOnline = true,
                    StatusCode = "Online",
                    StatusTooltip = "Online (Yeşil) - Bağlantı aktif ve normal",
                    ModelName = "DGS-1210-20/C1",
                    ProductType = "Switch",
                    DeviceName = "DGS-1210-20",
                    MacAddress = "1C:5F:2B:73:47:B0",
                    IPv4 = "", // In screenshot 1 IPv4 is blank or not assigned on Port 1
                    IPv6 = "",
                    DeviceTime = "20171216131731",
                    Interface = "Port 1",
                    FwVersion = "4.10.B035",
                    HwVersion = "C1",
                    SerialNumber = "",
                    IpMode = "Static IP",
                    ProtocolVersion = "DDP V2",
                    DdpInformation = "0.24"
                },
                new DemoDevice
                {
                    IsSelected = false,
                    IsOnline = true,
                    StatusCode = "Warning",
                    StatusTooltip = "Warning (Sarı) - Yüksek CPU yükü / Paket kaybı uyarısı",
                    ModelName = "DGS-1210-28P/ME",
                    ProductType = "PoE Switch",
                    DeviceName = "DGS-1210-28P-Core",
                    MacAddress = "1C:5F:2B:88:12:3A",
                    IPv4 = "192.168.1.10",
                    IPv6 = "-",
                    DeviceTime = "20171216131731",
                    Interface = "Port 2",
                    FwVersion = "4.21.B012",
                    HwVersion = "A1",
                    SerialNumber = "QB7E1G500144",
                    IpMode = "Static IP",
                    ProtocolVersion = "DDP V2",
                    DdpInformation = "0.24"
                },
                new DemoDevice
                {
                    IsSelected = false,
                    IsOnline = false,
                    StatusCode = "Critical",
                    StatusTooltip = "Critical (Kırmızı) - Cihaz çevrimdışı / Yanıt vermiyor",
                    ModelName = "DGS-1510-28X",
                    ProductType = "Stackable Switch",
                    DeviceName = "DGS-1510-Dist",
                    MacAddress = "28:10:7B:44:91:EE",
                    IPv4 = "192.168.1.2",
                    IPv6 = "-",
                    DeviceTime = "20171216131730",
                    Interface = "Port 4",
                    FwVersion = "1.30.010",
                    HwVersion = "B1",
                    SerialNumber = "Q11A2J800051",
                    IpMode = "Static IP",
                    ProtocolVersion = "DDP V2",
                    DdpInformation = "0.26"
                },
                new DemoDevice
                {
                    IsSelected = false,
                    IsOnline = true,
                    StatusCode = "Maintenance",
                    StatusTooltip = "Maintenance (Mavi) - Bakım / Yazılım güncelleme modunda",
                    ModelName = "DAP-2660",
                    ProductType = "Access Point",
                    DeviceName = "DAP-2660-Floor2",
                    MacAddress = "70:62:B8:19:AC:40",
                    IPv4 = "192.168.1.55",
                    IPv6 = "-",
                    DeviceTime = "20171216131731",
                    Interface = "Port 8",
                    FwVersion = "2.01.R008",
                    HwVersion = "A1",
                    SerialNumber = "PV2A1C300492",
                    IpMode = "DHCP",
                    ProtocolVersion = "DDP V1",
                    DdpInformation = "0.19"
                },
                new DemoDevice
                {
                    IsSelected = false,
                    IsOnline = false,
                    StatusCode = "Inactive",
                    StatusTooltip = "Inactive (Gri) - Bekleme / Pasif cihaz konfigürasyonu",
                    ModelName = "DXS-3600-32S",
                    ProductType = "10G Aggregation",
                    DeviceName = "DXS-3600-CoreB",
                    MacAddress = "00:1E:58:A2:33:10",
                    IPv4 = "192.168.1.150",
                    IPv6 = "fe80::1",
                    DeviceTime = "20171216131720",
                    Interface = "Port 10",
                    FwVersion = "3.00.015",
                    HwVersion = "A2",
                    SerialNumber = "D36A1K000088",
                    IpMode = "Static IP",
                    ProtocolVersion = "DDP V2",
                    DdpInformation = "0.24"
                },
                new DemoDevice
                {
                    IsSelected = false,
                    IsOnline = false,
                    StatusCode = "Disabled",
                    StatusTooltip = "Disabled (Siyah) - Yönetici tarafından devre dışı bırakıldı",
                    ModelName = "DES-3200-28F",
                    ProductType = "Metro Switch",
                    DeviceName = "DES-3200-Access",
                    MacAddress = "B0:48:7A:11:80:CC",
                    IPv4 = "192.168.1.200",
                    IPv6 = "-",
                    DeviceTime = "20171216131718",
                    Interface = "Port 12",
                    FwVersion = "1.84.B006",
                    HwVersion = "B1",
                    SerialNumber = "M32F1H000912",
                    IpMode = "Static IP",
                    ProtocolVersion = "DDP V1",
                    DdpInformation = "0.18"
                }
            };
        }
    }

    public class WizardDeviceItem
    {
        public string IsSupported { get; set; } = "Yes";
        public string ModelName { get; set; } = "DGS-1210-20/C1";
        public string MacAddress { get; set; } = "1C:5F:2B:73:47:B0";
        public string IpAddress { get; set; } = "";
        public string SerialNumber { get; set; } = "";
    }
}
