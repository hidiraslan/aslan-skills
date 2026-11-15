using System.Windows;

namespace ClassicAeroStudio.Views
{
    public partial class StudioLayoutWindow : Window
    {
        public StudioLayoutWindow()
        {
            InitializeComponent();
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
