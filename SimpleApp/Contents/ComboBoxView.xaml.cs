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
using uEN.Utils;

namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ComboBoxView : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public ComboBoxView()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<ComboBoxViewModel>();

            builder.Element(SampleComboBox)
                   .Binding(ComboBox.ItemsSourceProperty, x => x.SampleItemsSource);

            builder.Element(EditableComboBox)
                   .Binding(ComboBox.ItemsSourceProperty, x => x.EditableItemsSource);

            builder.Element(DisableComboBox)
                   .Binding(ComboBox.ItemsSourceProperty, x => x.DisableItemsSource);


        }
    }
}
