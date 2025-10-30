using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace i3mr0_MusicRP {
    /// <summary>
    /// Theme management system for i3mr0 MusicRP
    /// </summary>
    public class ThemeManager {
        public enum Theme {
            Default,
            Dark,
            Light,
            Neon,
            Minimal,
            Custom
        }

        private static Theme currentTheme = Theme.Default;
        private static Dictionary<Theme, ThemeColors> themes = new Dictionary<Theme, ThemeColors>();

        static ThemeManager() {
            InitializeThemes();
        }

        private static void InitializeThemes() {
            // Default theme
            themes[Theme.Default] = new ThemeColors {
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                Foreground = new SolidColorBrush(Color.FromRgb(33, 33, 33)),
                Accent = new SolidColorBrush(Color.FromRgb(0, 120, 215)),
                Border = new SolidColorBrush(Color.FromRgb(200, 200, 200)),
                ButtonBackground = new SolidColorBrush(Color.FromRgb(0, 120, 215)),
                ButtonForeground = new SolidColorBrush(Colors.White)
            };

            // Dark theme
            themes[Theme.Dark] = new ThemeColors {
                Background = new SolidColorBrush(Color.FromRgb(32, 32, 32)),
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Accent = new SolidColorBrush(Color.FromRgb(0, 162, 232)),
                Border = new SolidColorBrush(Color.FromRgb(64, 64, 64)),
                ButtonBackground = new SolidColorBrush(Color.FromRgb(0, 162, 232)),
                ButtonForeground = new SolidColorBrush(Colors.White)
            };

            // Light theme
            themes[Theme.Light] = new ThemeColors {
                Background = new SolidColorBrush(Colors.White),
                Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                Accent = new SolidColorBrush(Color.FromRgb(0, 100, 200)),
                Border = new SolidColorBrush(Color.FromRgb(220, 220, 220)),
                ButtonBackground = new SolidColorBrush(Color.FromRgb(0, 100, 200)),
                ButtonForeground = new SolidColorBrush(Colors.White)
            };

            // Neon theme
            themes[Theme.Neon] = new ThemeColors {
                Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0)),
                Accent = new SolidColorBrush(Color.FromRgb(255, 0, 255)),
                Border = new SolidColorBrush(Color.FromRgb(0, 255, 255)),
                ButtonBackground = new SolidColorBrush(Color.FromRgb(255, 0, 255)),
                ButtonForeground = new SolidColorBrush(Colors.Black)
            };

            // Minimal theme
            themes[Theme.Minimal] = new ThemeColors {
                Background = new SolidColorBrush(Color.FromRgb(250, 250, 250)),
                Foreground = new SolidColorBrush(Color.FromRgb(50, 50, 50)),
                Accent = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                Border = new SolidColorBrush(Color.FromRgb(180, 180, 180)),
                ButtonBackground = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                ButtonForeground = new SolidColorBrush(Colors.White)
            };
        }

        public static void ApplyTheme(Theme theme, Window window) {
            currentTheme = theme;
            var colors = themes[theme];

            if (window != null) {
                window.Background = colors.Background;
                ApplyThemeToElement(window, colors);
            }
        }

        private static void ApplyThemeToElement(DependencyObject element, ThemeColors colors) {
            if (element == null) return;

            // Apply to TextBlocks
            if (element is TextBlock textBlock) {
                textBlock.Foreground = colors.Foreground;
            }

            // Apply to Buttons
            if (element is Button button) {
                button.Background = colors.ButtonBackground;
                button.Foreground = colors.ButtonForeground;
                button.BorderBrush = colors.Border;
            }

            // Apply to TextBoxes
            if (element is TextBox textBox) {
                textBox.Background = colors.Background;
                textBox.Foreground = colors.Foreground;
                textBox.BorderBrush = colors.Border;
            }

            // Apply to CheckBoxes
            if (element is CheckBox checkBox) {
                checkBox.Foreground = colors.Foreground;
            }

            // Apply to ComboBoxes
            if (element is ComboBox comboBox) {
                comboBox.Background = colors.Background;
                comboBox.Foreground = colors.Foreground;
                comboBox.BorderBrush = colors.Border;
            }

            // Apply to Borders
            if (element is Border border) {
                border.BorderBrush = colors.Border;
            }

            // Recursively apply to children
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++) {
                var child = VisualTreeHelper.GetChild(element, i);
                ApplyThemeToElement(child, colors);
            }
        }

        public static Theme GetCurrentTheme() {
            return currentTheme;
        }

        public static ThemeColors GetCurrentThemeColors() {
            return themes[currentTheme];
        }

        public static void SetCustomTheme(ThemeColors customColors) {
            themes[Theme.Custom] = customColors;
        }
    }

    public class ThemeColors {
        public Brush Background { get; set; } = new SolidColorBrush(Colors.White);
        public Brush Foreground { get; set; } = new SolidColorBrush(Colors.Black);
        public Brush Accent { get; set; } = new SolidColorBrush(Colors.Blue);
        public Brush Border { get; set; } = new SolidColorBrush(Colors.Gray);
        public Brush ButtonBackground { get; set; } = new SolidColorBrush(Colors.Blue);
        public Brush ButtonForeground { get; set; } = new SolidColorBrush(Colors.White);
    }
}
