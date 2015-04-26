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
    public partial class SimpleDataGridView : BizView
    {
        /// <summary>デフォルトコンストラクタ</summary>
        public SimpleDataGridView()
        {
            InitializeComponent();
        }

        protected override void BuildBinding()
        {
            var builder = CreateBindingBuilder<SimpleDataGridViewModel>();

            builder.Element(PART_grid)
                   .Binding(DataGrid.FrozenColumnCountProperty, x => x.FrozenColumnCount)
                   .Binding(DataGrid.SelectionModeProperty, x => x.SelectionMode)
                   .Binding(DataGrid.ItemsSourceProperty, x => x.GridSource);
        }

        protected override void OnViewModelMessageNotify(object sender, MessageNotificationEventArgs e)
        {
            if (e.Message == "GenerateColumns")
            {
                PART_grid.BeginInit();
                var factory = Repository.GetPriorityExport<IDataGridColumnFactory>();
                var atts = (IEnumerable<DataGridColumnAnnotationAttribute>)e.UserState;
                foreach (var each in atts.OrderBy(x => x.Idntity))
                {
                    PART_grid.Columns.Add(factory.Create(each));
                }
                PART_grid.EndInit();
                PART_grid.UpdateLayout();
            }
            base.OnViewModelMessageNotify(sender, e);
        }
    }
}
