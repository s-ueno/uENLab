using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
            get { return GetFont(); }
            set
            {
                SetFont(value);
                OnPropertyChanged();
            }
        }
        private FontFamily GetFont()
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
            get { return GetFontSize(); }
            set
            {
                SetFontSize(value);
                OnPropertyChanged();
            }
        }

        private double GetFontSize()
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

        #region AppStyle

        public AppStyle? Style
        {
            get { return GetAppStyle(); }
            set
            {
                SetAppStyle(value);
                OnPropertyChanged();
            }
        }

        private AppStyle? GetAppStyle()
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
                        new ResourceDictionary() { Source = ModernStyle } ;
            Application.Current.Resources.MergedDictionaries.Add(dic);
        }

        #endregion

        #region AppThnme

        public AppTheme? Theme
        {
            get { return GetAppTheme(); }
            set
            {
                SetAppTheme(value);
                OnPropertyChanged();
            }
        }

        private AppTheme? GetAppTheme()
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
