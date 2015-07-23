using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using uEN.Core;
using uEN.UI;
using uEN.UI.AttachedProperties;
using uEN.UI.Controls;
using uEN.UI.Validation;

namespace uEN.UI
{
    public interface ISimpleGridVisualElements
    {
        DataGrid Grid { get; }
        event EventHandler Assigned;
    }
    public interface ISimpleGrid : IDisposable
    {
        int FrozenColumnCount { get; set; }
        DataGridSelectionMode SelectionMode { get; set; }
        ListCollectionView GridSource { get; set; }
        event EventHandler<System.EventArgs> DoubleClick;
        object Current { get; }
        List<DataGridColumnAnnotationAttribute> ColumnAnnotation { get; }
        void Refresh();
    }

    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(SimpleDataGridView))]
    public class SimpleDataGridViewModel
        : BizViewModel, ISimpleGrid, ISimpleGridVisualElements
    {
        public SimpleDataGridViewModel()
        {
            SelectionMode = DataGridSelectionMode.Single;
            FrozenColumnCount = 0;
        }

        public void EnsureItemsSource<T>(IEnumerable<T> source)
        {
            GridSource = new ListCollectionView(source.ToList());
            var columnAnnotation = new List<DataGridColumnAnnotationAttribute>();
            foreach (var each in GridSource.ItemProperties)
            {
                var descriptor = each.Descriptor as PropertyDescriptor;
                if (descriptor != null)
                {
                    var att = descriptor.Attributes.OfType<DataGridColumnAnnotationAttribute>().FirstOrDefault();
                    if (att != null)
                    {
                        att.PropertyInfo = each;

                        columnAnnotation.Add(att);
                    }
                }
            }
            ColumnAnnotation = columnAnnotation.OrderBy(x => x.Idntity).ToList();
        }
        public override void LoadedView()
        {
            OnMessageNotify("GenerateColumns", ColumnAnnotation);
        }
        public int FrozenColumnCount { get; set; }
        public DataGridSelectionMode SelectionMode { get; set; }
        public ListCollectionView GridSource { get; set; }
        public List<DataGridColumnAnnotationAttribute> ColumnAnnotation { get; set; }
        public event EventHandler<EventArgs> DoubleClick;
        public void SelectAction()
        {
            OnDoubleClick(new EventArgs());
        }
        protected void OnDoubleClick(EventArgs e)
        {
            if (DoubleClick != null)
                DoubleClick(this, e);
        }
        public object Current { get { return GridSource.CurrentItem; } }

        public void Refresh()
        {
            LoadedView();
        }

        #region ISimpleGridVisualElements

        public DataGrid Grid
        {
            get;
            internal set;
        }
        public event EventHandler Assigned;
        internal void OnAssigned()
        {
            if (Assigned != null)
                Assigned(this, new EventArgs());
        }

        #endregion

    }
}
