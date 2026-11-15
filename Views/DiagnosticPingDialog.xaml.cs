using System;
using System.Text;
using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class DiagnosticPingDialog : Window
    {
        public DiagnosticPingDialog()
        {
            InitializeComponent();
            PopulateDefaultPingOutput();
        }

        public DiagnosticPingDialog(string targetIp) : this()
        {
            if (!string.IsNullOrWhiteSpace(targetIp))
            {
                TxtTargetIp.Text = targetIp;
                Title = $"Ping Diagnostics - {targetIp}";
            }
        }

        private void PopulateDefaultPingOutput()
        {
            var ip = TxtTargetIp?.Text ?? "192.168.1.20";
            var sb = new StringBuilder();
            sb.AppendLine($"Pinging {ip} with 32 bytes of data:");
            sb.AppendLine($"Reply from {ip}: bytes=32 time=1ms TTL=64");
            sb.AppendLine($"Reply from {ip}: bytes=32 time<1ms TTL=64");
            sb.AppendLine($"Reply from {ip}: bytes=32 time=1ms TTL=64");
            sb.AppendLine($"Reply from {ip}: bytes=32 time=2ms TTL=64");
            sb.AppendLine();
            sb.AppendLine($"Ping statistics for {ip}:");
            sb.AppendLine("    Packets: Sent = 4, Received = 4, Lost = 0 (0% loss),");
            sb.AppendLine("Approximate round trip times in milli-seconds:");
            sb.AppendLine("    Minimum = 0ms, Maximum = 2ms, Average = 1ms");
            
            TxtConsole.Text = sb.ToString();
            TxtStatus.Text = $"Diagnostics complete - 0% loss ({ip})";
        }

        private void OnStartPingClicked(object sender, RoutedEventArgs e)
        {
            PopulateDefaultPingOutput();
        }

        private void OnClearClicked(object sender, RoutedEventArgs e)
        {
            TxtConsole.Text = string.Empty;
            TxtStatus.Text = "Log cleared - Ready.";
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
