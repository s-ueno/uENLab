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


namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DataGridSampleView : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public DataGridSampleView()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<DataGridSampleViewModel>();
            builder.Element(GridHost)
                   .Binding(ContentPresenter.ContentProperty, x => x.SimpleGrid);
            builder.Element(SearchIdentityTextBox)
                   .Binding(TextBox.TextProperty, x => x.SearchIdentity)
                   .Binding(SymbolTextBoxProxy.ClickEvent, x => x.ClearAction);
            builder.Element(FindButton)
                   .Binding(Button.ClickEvent, x => x.FindAction);
        }
    }
}
