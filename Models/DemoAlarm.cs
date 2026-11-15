using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClassicAeroStudio.Models
{
    public enum AlarmSeverity
    {
        Critical,
        Warning,
        Information
    }

    public class DemoAlarm : INotifyPropertyChanged
    {
        private bool _isSelected;
        private bool _isAcknowledged;
        private string _statusText = "Unacknowledged";

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public string AlarmId { get; set; } = string.Empty;
        public AlarmSeverity Severity { get; set; } = AlarmSeverity.Warning;
        public string SeverityText => Severity.ToString();
        public string SeverityColorBrush => Severity switch
        {
            AlarmSeverity.Critical => "#CC1111",
            AlarmSeverity.Warning => "#D9822B",
            _ => "#1070CA"
        };
        public string SeverityIconKey => Severity switch
        {
            AlarmSeverity.Critical => "IconAlarmCritical",
            AlarmSeverity.Warning => "IconAlarmWarning",
            _ => "IconAlarmInfo"
        };

        public string DeviceName { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsAcknowledged
        {
            get => _isAcknowledged;
            set
            {
                _isAcknowledged = value;
                _statusText = value ? "Acknowledged" : "Unacknowledged";
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusText));
                OnPropertyChanged(nameof(StatusColorBrush));
            }
        }

        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(); }
        }

        public string StatusColorBrush => IsAcknowledged ? "#2A7A2A" : "#8A3B00";

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static ObservableCollection<DemoAlarm> GetSampleAlarms()
        {
            return new ObservableCollection<DemoAlarm>
            {
                new DemoAlarm
                {
                    IsSelected = false,
                    AlarmId = "ALM-1042",
                    Severity = AlarmSeverity.Critical,
                    DeviceName = "DGS-1510-Dist",
                    IpAddress = "192.168.1.2",
                    MacAddress = "28:10:7B:44:91:EE",
                    Category = "Power / Hardware",
                    Timestamp = "2026-09-28 14:48:12",
                    Description = "Power Supply 2 (RPS): Redundant power input lost / AC cable disconnected.",
                    IsAcknowledged = false
                },
                new DemoAlarm
                {
                    IsSelected = false,
                    AlarmId = "ALM-1039",
                    Severity = AlarmSeverity.Warning,
                    DeviceName = "DGS-1210-28P-Core",
                    IpAddress = "192.168.1.10",
                    MacAddress = "1C:5F:2B:88:12:3A",
                    Category = "Network Interface",
                    Timestamp = "2026-09-28 14:32:05",
                    Description = "Port 1/0/24: Link DOWN (1000BASE-T link flap detected within 30s threshold).",
                    IsAcknowledged = false
                },
                new DemoAlarm
                {
                    IsSelected = false,
                    AlarmId = "ALM-1025",
                    Severity = AlarmSeverity.Warning,
                    DeviceName = "DAP-2660-Floor2",
                    IpAddress = "192.168.1.55",
                    MacAddress = "70:62:B8:19:AC:40",
                    Category = "Wireless / RF",
                    Timestamp = "2026-09-28 13:14:22",
                    Description = "Radio 5GHz: High channel interference ratio (>78%) on Channel 36.",
                    IsAcknowledged = true
                },
                new DemoAlarm
                {
                    IsSelected = false,
                    AlarmId = "ALM-0984",
                    Severity = AlarmSeverity.Information,
                    DeviceName = "DGS-1210-20",
                    IpAddress = "192.168.1.1",
                    MacAddress = "1C:5F:2B:73:47:B0",
                    Category = "System / DDP",
                    Timestamp = "2026-09-28 12:05:00",
                    Description = "DDP discovery broadcast replied successfully. Firmware checksum verified (v4.10.B035).",
                    IsAcknowledged = true
                }
            };
        }
    }
}
