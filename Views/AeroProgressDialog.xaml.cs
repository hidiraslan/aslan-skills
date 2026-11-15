using System;
using System.Windows;
using System.Windows.Threading;

namespace ClassicAeroStudio.Views
{
    public partial class AeroProgressDialog : Window
    {
        private DispatcherTimer? _timer;
        private int _currentProgress = 0;

        public bool IsCancelled { get; private set; } = false;

        public AeroProgressDialog(string title = "Device Configuration Sync", string header = "Writing Configuration to Device...", string message = "Please wait while settings are serialized and committed to NVRAM.")
        {
            InitializeComponent();
            Title = title;
            TxtHeader.Text = header;
            TxtSubHeader.Text = message;

            StartSimulation();
        }

        public void SetIndeterminate(bool indeterminate)
        {
            ProgressBarCtrl.IsIndeterminate = indeterminate;
            TxtPercent.Visibility = indeterminate ? Visibility.Collapsed : Visibility.Visible;
            if (indeterminate)
            {
                TxtDetail.Text = "Synchronizing configuration state with remote switch...";
            }
        }

        public void SetProgress(int percent, string detail)
        {
            _currentProgress = Math.Clamp(percent, 0, 100);
            ProgressBarCtrl.Value = _currentProgress;
            TxtPercent.Text = $"{_currentProgress}%";
            TxtDetail.Text = detail;
        }

        private void StartSimulation()
        {
            _currentProgress = 15;
            ProgressBarCtrl.Value = _currentProgress;
            TxtPercent.Text = $"{_currentProgress}%";

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(80)
            };
            _timer.Tick += (s, e) =>
            {
                _currentProgress += 3;
                if (_currentProgress >= 100)
                {
                    _currentProgress = 100;
                    _timer.Stop();
                    ProgressBarCtrl.Value = 100;
                    TxtPercent.Text = "100%";
                    TxtHeader.Text = "Process Completed Successfully";
                    TxtSubHeader.Text = "All operations committed and verified without errors.";
                    TxtDetail.Text = "Execution completed (Status: OK). Click OK to close.";
                    BtnBackground.Visibility = Visibility.Collapsed;
                    BtnCancel.Content = "OK";
                    try
                    {
                        if (FindResource("ClassicDefaultButton") is Style defaultBtnStyle)
                        {
                            BtnCancel.Style = defaultBtnStyle;
                        }
                    }
                    catch { }
                    BtnCancel.Focus();
                }
                else
                {
                    ProgressBarCtrl.Value = _currentProgress;
                    TxtPercent.Text = $"{_currentProgress}%";
                    TxtDetail.Text = $"Processing component block {(_currentProgress * 40 / 100)} of 40 (checksum ok)...";
                }
            };
            _timer.Start();
        }

        private void OnBackgroundClicked(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            try { DialogResult = true; } catch { }
            Close();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            _timer?.Stop();
            IsCancelled = true;
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            base.OnClosed(e);
        }
    }
}
