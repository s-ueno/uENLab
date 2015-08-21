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
using uEN.UI.AttachedProperties;

namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ImeSampleView : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public ImeSampleView()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<ImeSampleViewModel>();

            builder.Element(NoControlTextBox)
                   .Binding(TextBox.TextProperty, x => x.NoControlText);

            builder.Element(OnTextBox)
                   .Binding(TextBox.TextProperty, x => x.OnText);

            builder.Element(OffTextBox)
                   .Binding(TextBox.TextProperty, x => x.OffText);

            builder.Element(DisableTextBox)
                   .Binding(TextBox.TextProperty, x => x.DisableText);

            builder.Element(HiraganaTextBox)
                   .Binding(TextBox.TextProperty, x => x.HiraganaText);

            builder.Element(KatakanaTextBox)
                   .Binding(TextBox.TextProperty, x => x.Katakana);

            builder.Element(KatakanaHalfTextBox)
                   .Binding(TextBox.TextProperty, x => x.KatakanaHalf);

            builder.Element(AlphaFullTextBox)
                   .Binding(TextBox.TextProperty, x => x.AlphaFull);

            builder.Element(AlphaTextBox)
                   .Binding(TextBox.TextProperty, x => x.Alpha);

            builder.Element(NumericTextBox)
                   .Binding(TextBox.TextProperty, x => x.Numeric);
        }
    }
}
