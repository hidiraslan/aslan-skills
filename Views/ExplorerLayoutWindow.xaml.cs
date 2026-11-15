using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class ExplorerLayoutWindow : Window
    {
        public class ArchiveFileItem
        {
            public string Name { get; set; } = string.Empty;
            public string Date { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Size { get; set; } = string.Empty;
            public string Status { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
        }

        public ObservableCollection<ArchiveFileItem> Files { get; set; }

        public ExplorerLayoutWindow()
        {
            InitializeComponent();

            Files = new ObservableCollection<ArchiveFileItem>
            {
                new ArchiveFileItem { Name = "Running-Config-Snapshot-20261002.bin", Date = "02.10.2026 10:15", Type = "Flash Binary", Size = "2.45 MB", Status = "Verified (SHA256)", Location = "NVMe-Flash://Bank-1/System" },
                new ArchiveFileItem { Name = "Core-Switch-VLAN-Topology.xml", Date = "01.10.2026 18:30", Type = "XML Schema", Size = "340 KB", Status = "Valid", Location = "NVMe-Flash://Bank-1/Config" },
                new ArchiveFileItem { Name = "Syslog-Diagnostic-Trace-Q3.log", Date = "30.09.2026 23:59", Type = "Log Archive", Size = "14.2 MB", Status = "Indexed", Location = "Local-LogStore://Partition-2" },
                new ArchiveFileItem { Name = "Firmware-EdgeSeries-v4.1.2.tar.gz", Date = "28.09.2026 14:20", Type = "Signed Image", Size = "48.1 MB", Status = "Digitally Signed", Location = "Storage-Cluster://Firmware/v4" },
                new ArchiveFileItem { Name = "SNMP-Trap-Database-MIBs.db", Date = "25.09.2026 09:12", Type = "SQLite Database", Size = "8.60 MB", Status = "Active", Location = "NVMe-Flash://Bank-2/MIBs" },
                new ArchiveFileItem { Name = "Emergency-Factory-Default.cfg", Date = "20.09.2026 11:00", Type = "Recovery Config", Size = "64 KB", Status = "Protected", Location = "ROM-Secure://Recovery" },
                new ArchiveFileItem { Name = "SpanningTree-Convergence-Logs.tar", Date = "18.09.2026 08:45", Type = "TAR Archive", Size = "5.12 MB", Status = "Archived", Location = "Local-LogStore://Archives" },
                new ArchiveFileItem { Name = "Radius-Auth-Certificates.pfx", Date = "15.09.2026 16:10", Type = "PKCS#12 Cert", Size = "12 KB", Status = "Installed", Location = "Security-Vault://Certs" },
                new ArchiveFileItem { Name = "BGP-Route-Reflection-Table.dump", Date = "12.09.2026 12:00", Type = "Memory Dump", Size = "32.8 MB", Status = "Analyzed", Location = "Diagnostics://Core-Dump" },
                new ArchiveFileItem { Name = "Core-Switch-02-Diff-Patch.patch", Date = "10.09.2026 17:35", Type = "Diff Patch", Size = "88 KB", Status = "Applied", Location = "NVMe-Flash://Bank-1/Patches" },
                new ArchiveFileItem { Name = "LLDP-Neighbor-Discovery.json", Date = "08.09.2026 09:50", Type = "JSON Map", Size = "192 KB", Status = "Synchronized", Location = "NVMe-Flash://Bank-1/Config" },
                new ArchiveFileItem { Name = "Bootloader-Recovery-ROM-v2.0.bin", Date = "01.09.2026 14:15", Type = "ROM Image", Size = "1.02 MB", Status = "Read-Only", Location = "ROM-Secure://Bootloader" },
                new ArchiveFileItem { Name = "Access-Control-List-Audit.csv", Date = "28.08.2026 11:22", Type = "CSV Report", Size = "410 KB", Status = "Audited", Location = "Audit-Trail://Reports" },
                new ArchiveFileItem { Name = "CrashDump-Kernel-20260824.dmp", Date = "24.08.2026 03:14", Type = "Crash Dump", Size = "64.5 MB", Status = "Flagged", Location = "Diagnostics://Core-Dump" },
                new ArchiveFileItem { Name = "OSPF-Neighbor-Adjacencies.xml", Date = "20.08.2026 19:40", Type = "XML Schema", Size = "115 KB", Status = "Synchronized", Location = "NVMe-Flash://Bank-1/Routing" },
                new ArchiveFileItem { Name = "TACACS-Accounting-Export.log", Date = "15.08.2026 08:00", Type = "Log Archive", Size = "22.4 MB", Status = "Rotated", Location = "Local-LogStore://Accounting" },
                new ArchiveFileItem { Name = "QoS-Policy-Map-Traffic-Class.cfg", Date = "10.08.2026 14:25", Type = "Policy Config", Size = "76 KB", Status = "Active", Location = "NVMe-Flash://Bank-1/Config" },
                new ArchiveFileItem { Name = "Hardware-Transceiver-DOM-Diagnostics.db", Date = "05.08.2026 16:50", Type = "SQLite Database", Size = "4.20 MB", Status = "Online", Location = "Diagnostics://SFP-Telemetry" }
            };

            // Set DataContext
            var dataGrid = this.FindName("ExplorerDataGrid") as System.Windows.Controls.DataGrid;
            if (dataGrid != null)
            {
                dataGrid.ItemsSource = Files;
            }
        }

        private void OnTreeViewRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            // Suppress horizontal shifting when a tree item is selected or clicked
            e.Handled = true;
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
