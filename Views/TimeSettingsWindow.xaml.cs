using System.Collections.Generic;
using System.Windows;
using ClassicAeroStudio.Models;

namespace ClassicAeroStudio.Views
{
    public partial class TimeSettingsWindow : Window
    {
        public TimeSettingsWindow()
        {
            InitializeComponent();
            
            // Set password dots for visual demonstration
            PwdBox.Password = "password123";

            // Static row matching screenshot 2:
            WizardDataGrid.ItemsSource = new List<WizardDeviceItem>
            {
                new WizardDeviceItem
                {
                    IsSupported = "Yes",
                    ModelName = "DGS-1210-20/C1",
                    MacAddress = "1C:5F:2B:73:47:B0",
                    IpAddress = "192.168.1.20",
                    SerialNumber = "QB7E1G500012"
                }
            };

            // Start on Step 1
            ShowStep(1);
        }

        public void ShowStep(int step)
        {
            if (step == 1)
            {
                WizardHeaderControl.SetStep(1);
                Step1Panel.Visibility = Visibility.Visible;
                Step2Panel.Visibility = Visibility.Collapsed;
                AuthGroupBox.Visibility = Visibility.Collapsed;

                BtnNext.Visibility = Visibility.Visible;
                BtnPrevious.Visibility = Visibility.Collapsed;
                BtnSubmit.Visibility = Visibility.Collapsed;

                TxtStepHint.Text = "Step 1 of 2: Configure System Clock & NTP Source";
            }
            else if (step == 2)
            {
                WizardHeaderControl.SetStep(2);
                Step1Panel.Visibility = Visibility.Collapsed;
                Step2Panel.Visibility = Visibility.Visible;
                AuthGroupBox.Visibility = Visibility.Visible;

                BtnNext.Visibility = Visibility.Collapsed;
                BtnPrevious.Visibility = Visibility.Visible;
                BtnSubmit.Visibility = Visibility.Visible;

                TxtStepHint.Text = "Step 2 of 2: Confirm Configuration & Authenticate";
            }
        }

        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            ShowStep(2);
        }

        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            ShowStep(1);
        }

        private void OnTimeModeChanged(object sender, RoutedEventArgs e)
        {
            if (TxtManualDate == null || TxtManualTime == null) return;
            bool isManual = RadioManual?.IsChecked == true;
            TxtManualDate.IsEnabled = isManual;
            TxtManualTime.IsEnabled = isManual;
        }

        private void OnSubmitClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
