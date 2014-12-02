using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using uEN.Core;
using uEN.UI;

namespace uEN.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(ThemeColorView))]
    public class ThemeColorViewModel : BizViewModel
    {
        public override string Description { get { return "Window カラー"; } }

        private static Color LightColor = Colors.Snow;
        private static Color DarkColor = (Color)ColorConverter.ConvertFromString("#1d1d1d");

        public override void ApplyView()
        {
            ColorCollection = new ListCollectionView(new[] 
            {
                LightColor,
                DarkColor
            }.ToList());

            if (Singleton<ThemeManager>.Value.Theme == AppTheme.Dark)
            {
                ColorCollection.MoveCurrentTo(DarkColor);
            }
            if (Singleton<ThemeManager>.Value.Theme == AppTheme.Light)
            {
                ColorCollection.MoveCurrentTo(LightColor);
            }

            ColorCollection.CurrentChanged -= ColorCollection_CurrentChanged;
            ColorCollection.CurrentChanged += ColorCollection_CurrentChanged;
        }
        public ListCollectionView ColorCollection { get; set; }

        void ColorCollection_CurrentChanged(object sender, EventArgs e)
        {
            var theme = ColorCollection.CurrentItem as Color?;
            if (!theme.HasValue)
                return;

            if (theme.Value == DarkColor)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.Dark;
            }
            if (theme.Value == LightColor)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.Light;
            }
        }
    }
}
