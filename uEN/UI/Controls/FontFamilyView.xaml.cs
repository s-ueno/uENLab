using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uEN.UI;
using uEN.UI.AttachedProperties;
using uEN.Utils;

namespace uEN.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FontFamilyView : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public FontFamilyView()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<FontFamilyViewModel>();
            builder.Element(FontCollection)
                   .Binding(ListBox.ItemsSourceProperty, x => x.FontCollection);
                   
        }
       
    }

}
