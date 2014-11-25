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
    public partial class Vol10_1View : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public Vol10_1View()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<Vol10_1ViewModel>();

            builder.Element(GoForwardButton)
                   .Binding(Button.ClickEvent, x => x.GoForwardAction);

            builder.Element(GoBackButton)
                   .Binding(Button.ClickEvent, x => x.GoBackAction);
        }
    }
}
