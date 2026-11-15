using System;
using System.Windows;
using System.Windows.Media;

namespace ClassicAeroStudio.Views
{
    public enum AeroMessageBoxButton
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel,
        RetryCancel
    }

    public enum AeroMessageBoxIcon
    {
        Information,
        Success,
        Warning,
        Error,
        Question
    }

    public enum AeroMessageBoxResult
    {
        Ok,
        Cancel,
        Yes,
        No,
        Retry
    }

    public partial class AeroMessageBox : Window
    {
        public AeroMessageBoxResult Result { get; private set; } = AeroMessageBoxResult.Cancel;

        public AeroMessageBox()
        {
            InitializeComponent();
        }

        public static AeroMessageBoxResult Show(
            Window? owner,
            string message,
            string title = "Network Assistant",
            string? header = null,
            AeroMessageBoxButton button = AeroMessageBoxButton.Ok,
            AeroMessageBoxIcon icon = AeroMessageBoxIcon.Information)
        {
            var msgBox = new AeroMessageBox
            {
                Title = title
            };

            if (owner != null && owner.IsVisible)
            {
                msgBox.Owner = owner;
            }
            else if (Application.Current?.MainWindow != null && Application.Current.MainWindow.IsVisible)
            {
                msgBox.Owner = Application.Current.MainWindow;
            }

            // Setup Header / Main Instruction & Content
            if (string.IsNullOrEmpty(header))
            {
                msgBox.TxtMainInstruction.Text = title;
                msgBox.TxtContent.Text = message;
            }
            else
            {
                msgBox.TxtMainInstruction.Text = header;
                msgBox.TxtContent.Text = message;
            }

            // Setup Icon
            string iconKey = icon switch
            {
                AeroMessageBoxIcon.Success => "IconAlarmSuccess",
                AeroMessageBoxIcon.Warning => "IconAlarmWarning",
                AeroMessageBoxIcon.Error => "IconAlarmCritical",
                AeroMessageBoxIcon.Question => "IconAlarmQuestion",
                _ => "IconAlarmInfo"
            };

            if (msgBox.TryFindResource(iconKey) is DrawingBrush brush)
            {
                msgBox.IconRect.Fill = brush;
            }

            // Setup Buttons
            msgBox.ConfigureButtons(button);

            msgBox.ShowDialog();
            return msgBox.Result;
        }

        public static AeroMessageBoxResult Show(string message, string title = "Network Assistant")
        {
            return Show(null, message, title, null, AeroMessageBoxButton.Ok, AeroMessageBoxIcon.Information);
        }

        public static AeroMessageBoxResult ShowInfo(Window? owner, string message, string title = "Information", string? header = null)
        {
            return Show(owner, message, title, header, AeroMessageBoxButton.Ok, AeroMessageBoxIcon.Information);
        }

        public static AeroMessageBoxResult ShowSuccess(Window? owner, string message, string title = "Operation Complete", string? header = null)
        {
            return Show(owner, message, title, header, AeroMessageBoxButton.Ok, AeroMessageBoxIcon.Success);
        }

        public static AeroMessageBoxResult ShowWarning(Window? owner, string message, string title = "Warning", string? header = null)
        {
            return Show(owner, message, title, header, AeroMessageBoxButton.Ok, AeroMessageBoxIcon.Warning);
        }

        public static AeroMessageBoxResult ShowError(Window? owner, string message, string title = "Error", string? header = null)
        {
            return Show(owner, message, title, header, AeroMessageBoxButton.Ok, AeroMessageBoxIcon.Error);
        }

        public static bool ShowConfirm(Window? owner, string message, string title = "Confirm Action", string? header = null)
        {
            var res = Show(owner, message, title, header, AeroMessageBoxButton.YesNo, AeroMessageBoxIcon.Question);
            return res == AeroMessageBoxResult.Yes;
        }

        private void ConfigureButtons(AeroMessageBoxButton button)
        {
            Btn1.Visibility = Visibility.Visible;
            Btn2.Visibility = Visibility.Collapsed;
            Btn3.Visibility = Visibility.Collapsed;

            switch (button)
            {
                case AeroMessageBoxButton.Ok:
                    Btn1.Content = "OK";
                    Btn1.IsDefault = true;
                    Btn1.IsCancel = true;
                    Btn1.Margin = new Thickness(0);
                    break;

                case AeroMessageBoxButton.OkCancel:
                    Btn1.Content = "OK";
                    Btn1.IsDefault = true;
                    Btn1.Margin = new Thickness(0, 0, 8, 0);

                    Btn2.Content = "Cancel";
                    Btn2.IsCancel = true;
                    Btn2.Margin = new Thickness(0);
                    Btn2.Visibility = Visibility.Visible;
                    break;

                case AeroMessageBoxButton.YesNo:
                    Btn1.Content = "Yes";
                    Btn1.IsDefault = true;
                    Btn1.Margin = new Thickness(0, 0, 8, 0);

                    Btn2.Content = "No";
                    Btn2.IsCancel = true;
                    Btn2.Margin = new Thickness(0);
                    Btn2.Visibility = Visibility.Visible;
                    break;

                case AeroMessageBoxButton.YesNoCancel:
                    Btn1.Content = "Yes";
                    Btn1.IsDefault = true;
                    Btn1.Margin = new Thickness(0, 0, 8, 0);

                    Btn2.Content = "No";
                    Btn2.Margin = new Thickness(0, 0, 8, 0);
                    Btn2.Visibility = Visibility.Visible;

                    Btn3.Content = "Cancel";
                    Btn3.IsCancel = true;
                    Btn3.Margin = new Thickness(0);
                    Btn3.Visibility = Visibility.Visible;
                    break;

                case AeroMessageBoxButton.RetryCancel:
                    Btn1.Content = "Retry";
                    Btn1.IsDefault = true;
                    Btn1.Margin = new Thickness(0, 0, 8, 0);

                    Btn2.Content = "Cancel";
                    Btn2.IsCancel = true;
                    Btn2.Margin = new Thickness(0);
                    Btn2.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void OnBtn1Clicked(object sender, RoutedEventArgs e)
        {
            if (Btn1.Content?.ToString() == "Yes")
                Result = AeroMessageBoxResult.Yes;
            else if (Btn1.Content?.ToString() == "Retry")
                Result = AeroMessageBoxResult.Retry;
            else
                Result = AeroMessageBoxResult.Ok;

            DialogResult = true;
            Close();
        }

        private void OnBtn2Clicked(object sender, RoutedEventArgs e)
        {
            if (Btn2.Content?.ToString() == "No")
                Result = AeroMessageBoxResult.No;
            else
                Result = AeroMessageBoxResult.Cancel;

            DialogResult = false;
            Close();
        }

        private void OnBtn3Clicked(object sender, RoutedEventArgs e)
        {
            Result = AeroMessageBoxResult.Cancel;
            DialogResult = false;
            Close();
        }
    }
}
