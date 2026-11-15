using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace ClassicAeroStudio.Services
{
    public enum AppTheme
    {
        ClassicAero,
        DarkAero
    }

    public static class ThemeManager
    {
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private static AppTheme _currentTheme = AppTheme.ClassicAero;
        public static AppTheme CurrentTheme => _currentTheme;

        public static event Action<AppTheme>? ThemeChanged;

        public static void SetWindowDarkMode(IntPtr hwnd, bool isDark)
        {
            if (hwnd == IntPtr.Zero) return;
            try
            {
                int darkMode = isDark ? 1 : 0;
                int hr = DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));
                if (hr != 0)
                {
                    DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref darkMode, sizeof(int));
                }
            }
            catch
            {
                // Ignore if dwmapi is not available on legacy OS
            }
        }

        public static void ApplyWindowTheme(Window window, bool isDark)
        {
            if (window == null) return;
            var helper = new WindowInteropHelper(window);
            if (helper.Handle != IntPtr.Zero)
            {
                SetWindowDarkMode(helper.Handle, isDark);
            }
            else
            {
                window.SourceInitialized += (s, e) =>
                {
                    var h = new WindowInteropHelper(window).Handle;
                    SetWindowDarkMode(h, isDark);
                };
            }
        }

        public static void ApplyTheme(AppTheme theme)
        {
            _currentTheme = theme;
            var app = Application.Current;
            if (app == null) return;

            string themeSource = theme == AppTheme.DarkAero
                ? "pack://application:,,,/Themes/DarkAeroTheme.xaml"
                : "pack://application:,,,/Themes/ClassicWindowsTheme.xaml";

            var newDict = new ResourceDictionary { Source = new Uri(themeSource, UriKind.Absolute) };

            var merged = app.Resources.MergedDictionaries;
            bool replaced = false;
            for (int i = 0; i < merged.Count; i++)
            {
                var dict = merged[i];
                if (dict.Source != null &&
                    (dict.Source.OriginalString.Contains("ClassicWindowsTheme.xaml") ||
                     dict.Source.OriginalString.Contains("DarkAeroTheme.xaml")))
                {
                    merged.RemoveAt(i);
                    merged.Insert(i, newDict);
                    replaced = true;
                    break;
                }
            }

            if (!replaced)
            {
                merged.Add(newDict);
            }

            bool isDark = (theme == AppTheme.DarkAero);
            foreach (Window window in app.Windows)
            {
                ApplyWindowTheme(window, isDark);
            }

            ThemeChanged?.Invoke(theme);
        }
    }
}
