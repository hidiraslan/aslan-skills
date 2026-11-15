using System.Windows;
using System.Windows.Controls;

namespace ClassicAeroStudio.Views
{
    public partial class FilePickerDialog : Window
    {
        public string SelectedFilePath { get; set; } = "C:\\NetworkBackups\\Firmware_v4\\DGS-1210-28P_Firmware_v4.21.B012.bin";

        public FilePickerDialog()
        {
            InitializeComponent();
        }

        private void OnFileSelected(object sender, SelectionChangedEventArgs e)
        {
            if (FileListBox?.SelectedItem is ListBoxItem item && item.Content is StackPanel sp)
            {
                foreach (var child in sp.Children)
                {
                    if (child is TextBlock tb && !string.IsNullOrWhiteSpace(tb.Text) && !tb.Text.StartsWith("  ["))
                    {
                        if (FileNameBox != null)
                        {
                            FileNameBox.Text = tb.Text;
                            SelectedFilePath = "C:\\NetworkBackups\\Firmware_v4\\" + tb.Text;
                        }
                        break;
                    }
                }
            }
        }

        private void OnBreadcrumbClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                // visual feedback
            }
        }

        private void OnOpenClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
