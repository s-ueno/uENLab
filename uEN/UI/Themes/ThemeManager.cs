using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using uEN.Core;

namespace uEN.UI
{
    public enum AppStyle
    {
        Modern,
        Flat,
    }

    public enum AppTheme
    {
        Light,
        Dark,
    }



    public class ThemeManager : INotifyPropertyChanged
    {
        public ThemeManager()
        {
            SetAppStyle(Style);
            SetAppTheme(Theme);

            SetFont(Font);
            SetFontSize(FontSize);
            SetBrandColor(BrandColor);
        }


        private LocalStorage<ThemeManager> Store = new LocalStorage<ThemeManager>();
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        static readonly Uri DarkTheme = new Uri(@"pack://application:,,,/uEN;component/UI/Themes/DarkTheme.xaml");
        static readonly Uri LightTheme = new Uri(@"pack://application:,,,/uEN;component/UI/Themes/LightTheme.xaml");

        static readonly Uri FlatStyle = new Uri(@"pack://application:,,,/uEN;component/UI/Themes/FlatStyle.xaml");
        static readonly Uri ModernStyle = new Uri(@"pack://application:,,,/uEN;component/UI/Themes/ModernStyle.xaml");

        public bool IsValid { get { return Application.Current != null; } }

        #region Font

        public FontFamily Font
        {
            get
            {
                var stringFontFamily = Store.Load() as string;
                if (!string.IsNullOrWhiteSpace(stringFontFamily))
                {
                    FontFamily buff = null;
                    try
                    {
                        buff = new FontFamily(stringFontFamily);
                    }
                    catch
                    {
                    }
                    if (buff != null)
                    {
                        SetFont(buff);
                        return buff;
                    }
                }
                return GetDefaultFont();
            }
            set
            {
                SetFont(value);
                Store.Save(value.ToString());
                OnPropertyChanged();
            }
        }
        private FontFamily GetDefaultFont()
        {
            FontFamily font = null;
            if (IsValid)
                font = Application.Current.TryFindResource(KeyFont) as FontFamily;
            if (font == null)
                font = new FontFamily("Meiryo");
            return font;
        }
        private void SetFont(FontFamily font)
        {
            if (!IsValid) return;
            Application.Current.Resources[KeyFont] = font;
        }

        public const string KeyFont = "AppFont";

        #endregion

        #region FontSize

        public double FontSize
        {
            get
            {
                var storeItem = Store.Load() as Double?;
                return storeItem.HasValue ? storeItem.Value : GetDefaultFontSize();
            }
            set
            {
                SetFontSize(value);
                Store.Save(value);
                OnPropertyChanged();
            }
        }

        private double GetDefaultFontSize()
        {
            double? size = null;
            if (IsValid)
                size = Application.Current.TryFindResource(KeyFontSize) as double?;
            if (!size.HasValue)
                size = 13;
            return size.Value;
        }

        private void SetFontSize(double value)
        {
            if (!IsValid) return;
            Application.Current.Resources[KeyFontSize] = value;
        }

        public const string KeyFontSize = "AppFontSize";

        #endregion

        #region BrandColor

        public Color? BrandColor
        {
            get
            {
                var stringBrandColor = Store.Load() as string;
                if (!string.IsNullOrWhiteSpace(stringBrandColor))
                {
                    Color? buff = null;
                    try
                    {
                        buff = ColorConverter.ConvertFromString(stringBrandColor) as Color?;
                    }
                    catch
                    {
                    }
                    if (buff.HasValue)
                    {
                        SetBrandColor(buff.Value);
                        return buff.Value;
                    }
                }
                return GetDefaultBrandColor();
            }
            set
            {
                SetBrandColor(value);
                Store.Save(Convert.ToString(value));
                OnPropertyChanged();
            }
        }
        private Color? GetDefaultBrandColor()
        {
            Color? color = null;
            if (IsValid)
                color = Application.Current.TryFindResource(KeyAppBrandColor) as Color?;
            if (color == null)
                color = ColorConverter.ConvertFromString("#00519A") as Color?;
            return color;
        }
        private void SetBrandColor(Color? color)
        {
            if (!IsValid) return;

            Application.Current.Resources[KeyAppBrandColor] = color;
            SetAppTheme(GetDefaultAppTheme());
        }

        public const string KeyAppBrandColor = "AppBrandColor";

        #endregion


        #region AppStyle

        public AppStyle? Style
        {
            get { return Store.Load() as AppStyle? ?? GetDefaultAppStyle(); }
            set
            {
                SetAppStyle(value);
                Store.Save(value);
                OnPropertyChanged();
            }
        }

        private AppStyle? GetDefaultAppStyle()
        {
            if (!IsValid)
                return null;
            var modern = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == ModernStyle);
            var flat = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == FlatStyle);
            return modern != null ? AppStyle.Modern :
                   flat != null ? AppStyle.Flat : (AppStyle?)null;
        }

        private void SetAppStyle(AppStyle? value)
        {
            if (!IsValid || !value.HasValue)
                return;

            var modern = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == ModernStyle);
            var flat = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == FlatStyle);

            if (modern != null)
                Application.Current.Resources.MergedDictionaries.Remove(modern);
            if (flat != null)
                Application.Current.Resources.MergedDictionaries.Remove(flat);

            var dic = value.Value == AppStyle.Flat ?
                        new ResourceDictionary() { Source = FlatStyle } :
                        new ResourceDictionary() { Source = ModernStyle };
            Application.Current.Resources.MergedDictionaries.Add(dic);
        }

        #endregion

        #region AppThnme

        public AppTheme? Theme
        {
            get { return Store.Load() as AppTheme? ?? GetDefaultAppTheme(); }
            set
            {
                SetAppTheme(value);
                Store.Save(value);
                OnPropertyChanged();
            }
        }

        private AppTheme? GetDefaultAppTheme()
        {
            if (!IsValid)
                return null;
            var light = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == LightTheme);
            var dark = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == DarkTheme);
            return light != null ? AppTheme.Light :
                   dark != null ? AppTheme.Dark : (AppTheme?)null;
        }

        private void SetAppTheme(AppTheme? value)
        {
            if (!IsValid || !value.HasValue)
                return;

            var light = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == LightTheme);
            var dark = Application.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source == DarkTheme);

            if (light != null)
                Application.Current.Resources.MergedDictionaries.Remove(light);
            if (dark != null)
                Application.Current.Resources.MergedDictionaries.Remove(dark);

            var dic = value.Value == AppTheme.Dark ?
                        new ResourceDictionary() { Source = DarkTheme } :
                        new ResourceDictionary() { Source = LightTheme };
            Application.Current.Resources.MergedDictionaries.Add(dic);
        }

        #endregion




    }
}
