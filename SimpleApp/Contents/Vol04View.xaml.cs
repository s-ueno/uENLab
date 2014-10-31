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

namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Vol04View : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public Vol04View()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<Vol04ViewModel>();

            builder.Element(SampleTextBox)
                   .Binding(TextBox.TextProperty, x => x.SampleText);

            builder.Element(SampleButton)
                   .Binding(Button.ClickEvent, x => x.SampleAction);

        }
    }
}
