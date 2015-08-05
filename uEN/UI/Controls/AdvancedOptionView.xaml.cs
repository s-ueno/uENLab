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
using uEN;
using uEN.Core;
using uEN.UI;
using uEN.UI.AttachedProperties;


namespace uEN.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AdvancedOptionView : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public AdvancedOptionView()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<AdvancedOptionViewModel>();

            builder.Element(AlternatingRowBackgroundCheckBox)
                   .Binding(CheckBox.IsCheckedProperty, x => x.UseAlternatingRowBackground)
                   .Binding(CheckBox.CheckedEvent, x => x.AlternatingRowBackgroundChanged)
                   .Binding(CheckBox.UncheckedEvent, x => x.AlternatingRowBackgroundChanged);


            builder.Element(DataGridAsyncBindingCheckBox)
                   .Binding(CheckBox.IsCheckedProperty, x => x.UseDataGridAsyncBinding, BindingMode.TwoWay);

        }
    }
}
