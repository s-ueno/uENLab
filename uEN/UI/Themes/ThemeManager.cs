﻿using System;
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

            AlternatingRowBackgroundChanged();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = null)
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
                var stringFontFamily = this.GetBackingStore("Font") as string;
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
                return new FontFamily("Meiryo");
            }
            set
            {
                SetFont(value);
                this.SetBackingStore(Convert.ToString(value), "Font");
                OnPropertyChanged();
            }
        }
        private void SetFont(FontFamily font)
        {
            if (!IsValid) return;
            Application.Current.Resources["AppFont"] = font;
        }

        #endregion

        #region FontSize

        public double FontSize
        {
            get
            {
                var storeItem = this.GetBackingStore("FontSize") as Double?;
                return storeItem.HasValue ? storeItem.Value : 13d;
            }
            set
            {
                SetFontSize(value);
                this.SetBackingStore(value, "FontSize");
                OnPropertyChanged();
            }
        }
        private void SetFontSize(double value)
        {
            if (!IsValid) return;
            Application.Current.Resources["AppFontSize"] = value;
        }

        #endregion

        #region BrandColor

        public Color? BrandColor
        {
            get
            {
                var stringBrandColor = this.GetBackingStore("BrandColor") as string;
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
                return ColorConverter.ConvertFromString("#00519A") as Color?;
            }
            set
            {
                SetBrandColor(value);
                this.SetBackingStore(Convert.ToString(value), "BrandColor");
                OnPropertyChanged();
            }
        }
        private void SetBrandColor(Color? color)
        {
            if (!IsValid) return;

            Application.Current.Resources["AppBrandColor"] = color;
            SetAppTheme(Theme);
        }

        #endregion

        #region AlternatingRowBackground

        public bool UseAlternatingRowBackground
        {
            get
            {
                return (this.GetBackingStore("UseAlternatingRowBackground") as bool?).GetValueOrDefault();
            }
            set
            {
                this.SetBackingStore(value, "UseAlternatingRowBackground");
            }
        }
        public void AlternatingRowBackgroundChanged()
        {
            if (UseAlternatingRowBackground)
            {
                var brand = BrandColor;
                var brush = new SolidColorBrush(brand.Value) { Opacity = 0.2 };
                AlternatingRowBackground = brush;
            }
            else
            {
                AlternatingRowBackground = Brushes.Transparent;
            }
        }
        public Brush AlternatingRowBackground
        {
            get
            {
                if (!IsValid) return Brushes.Transparent;
                return Application.Current.Resources["DataGrid.AlternatingRowBackgroundBrush"] as Brush;
            }
            set
            {
                if (!IsValid) return;
                Application.Current.Resources["DataGrid.AlternatingRowBackgroundBrush"] = value;
            }
        }

        #endregion

        #region AppStyle

        public AppStyle? Style
        {
            get { return this.GetBackingStore("Style") as AppStyle? ?? AppStyle.Modern; }
            set
            {
                SetAppStyle(value);
                this.SetBackingStore(value, "Style");
                OnPropertyChanged();
            }
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
            get { return this.GetBackingStore("Theme") as AppTheme?; }
            set
            {
                SetAppTheme(value);
                this.SetBackingStore(value, "Theme");
                OnPropertyChanged();
            }
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
