using System.Collections.ObjectModel;
using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class MmcLayoutWindow : Window
    {
        public class MmcNodeItem
        {
            public string NodeName { get; set; } = string.Empty;
            public string IpAddress { get; set; } = string.Empty;
            public string Firmware { get; set; } = string.Empty;
            public string Uptime { get; set; } = string.Empty;
            public string Cpu { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }

        public ObservableCollection<MmcNodeItem> Nodes { get; set; }

        public MmcLayoutWindow()
        {
            InitializeComponent();

            Nodes = new ObservableCollection<MmcNodeItem>
            {
                new MmcNodeItem { NodeName = "Core-SW-01", IpAddress = "192.168.1.1", Firmware = "v4.1.0", Uptime = "45 days", Cpu = "12%", Role = "Active Supervisor" },
                new MmcNodeItem { NodeName = "Core-SW-02", IpAddress = "192.168.1.2", Firmware = "v4.1.0", Uptime = "45 days", Cpu = "4%", Role = "Hot Standby" },
                new MmcNodeItem { NodeName = "Dist-Aggregation", IpAddress = "192.168.1.10", Firmware = "v4.0.8", Uptime = "12 days", Cpu = "28%", Role = "VLAN Gateway" }
            };

            MmcDataGrid.ItemsSource = Nodes;
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
