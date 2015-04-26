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
    public partial class BrandColorView : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public BrandColorView()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<BrandColorViewModel>();
            builder.Element(ColorCollection)
                   .Binding(ItemsControl.ItemsSourceProperty, x => x.ColorCollection);
        }
    }
}
