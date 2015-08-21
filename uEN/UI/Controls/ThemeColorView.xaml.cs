using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uEN.UI;

namespace uEN.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ThemeColorView : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public ThemeColorView()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<ThemeColorViewModel>();

            builder.Element(WhiteThemeRadioButton)
                   .Binding(RadioButton.IsCheckedProperty, x => x.UseWhiteTheme)
                   .Binding(RadioButton.CheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.UncheckedEvent, x => x.CheckedAction);

            builder.Element(BlackThemeRadioButton)
                   .Binding(RadioButton.IsCheckedProperty, x => x.UseBlackTheme)
                   .Binding(RadioButton.CheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.UncheckedEvent, x => x.CheckedAction);

            builder.Element(GlassBuleThemeRadioButton)
                   .Binding(RadioButton.IsCheckedProperty, x => x.UseGlassBuleTheme)
                   .Binding(RadioButton.CheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.UncheckedEvent, x => x.CheckedAction);

            builder.Element(GlassYellowThemeRadioButton)
                   .Binding(RadioButton.IsCheckedProperty, x => x.UseGlassYellowTheme)
                   .Binding(RadioButton.CheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.UncheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.VisibilityProperty, x => x.AllowGlassYellow)
                   .Convert(BooleanToVisibility);

            builder.Element(GlassRedThemeRadioButton)
                   .Binding(RadioButton.IsCheckedProperty, x => x.UseGlassRedTheme)
                   .Binding(RadioButton.CheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.UncheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.VisibilityProperty, x => x.AllowGlassRed)
                   .Convert(BooleanToVisibility);

            builder.Element(GlassGreenThemeRadioButton)
                   .Binding(RadioButton.IsCheckedProperty, x => x.UseGlassGreenTheme)
                   .Binding(RadioButton.CheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.UncheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.VisibilityProperty, x => x.AllowGlassGreen)
                   .Convert(BooleanToVisibility);

            builder.Element(GlassBrandThemeRadioButton)
                   .Binding(RadioButton.IsCheckedProperty, x => x.UseGlassBrandTheme)
                   .Binding(RadioButton.CheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.UncheckedEvent, x => x.CheckedAction)
                   .Binding(RadioButton.VisibilityProperty, x => x.AllowGlassBrand)
                   .Convert(BooleanToVisibility);
        }

        private object BooleanToVisibility(object arg1, Type arg2, object arg3, System.Globalization.CultureInfo arg4)
        {
            var ret = arg1 as bool?;
            if (!ret.HasValue || !ret.Value) return Visibility.Collapsed;
            return Visibility.Visible;
        }
    }
}
