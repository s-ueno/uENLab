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
using uEN.UI.Validation;

namespace uEN.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(SimpleDataGridView))]
    public class SimpleDataGridViewModel : BizViewModel
    {
        public SimpleDataGridViewModel()
        {
            SelectionMode = DataGridSelectionMode.Single;
            FrozenColumnCount = 0;
        }

        public void EnsureItemsSource<T>(IEnumerable<T> source)
        {
            GridSource = new ListCollectionView(source.ToList());
            ColumnAnnotation = new List<DataGridColumnAnnotationAttribute>();
            foreach (var each in GridSource.ItemProperties)
            {
                var descriptor = each.Descriptor as PropertyDescriptor;
                if (descriptor != null)
                {
                    var att = descriptor.Attributes.OfType<DataGridColumnAnnotationAttribute>().FirstOrDefault();
                    if (att != null)
                    {
                        att.PropertyInfo = each;

                        ColumnAnnotation.Add(att);
                    }
                }
            }
            OnMessageNotify("GenerateColumns", ColumnAnnotation);
            UpdateTarget();
        }
        public override void LoadedView()
        {
            OnMessageNotify("GenerateColumns", ColumnAnnotation);
        }
        public int FrozenColumnCount { get; set; }
        public DataGridSelectionMode SelectionMode { get; set; }
        public ListCollectionView GridSource { get; set; }
        public List<DataGridColumnAnnotationAttribute> ColumnAnnotation { get; set; }
    }
}
