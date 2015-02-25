using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using uEN.UI;

namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(ComboBoxView))]
    public class ComboBoxViewModel : BizViewModel
    {
        public override string Description { get { return "ComboBox Sample"; } }

        public ListCollectionView SampleItemsSource { get; set; }
        public ListCollectionView EditableItemsSource { get; set; }
        public ListCollectionView DisableItemsSource { get; set; }


        public override void ApplyView()
        {
            var items = new[] { "Australia (English)", "Brasil (Português)", "Canada (English)", "日本 (日本語)" };

            SampleItemsSource = new ListCollectionView(items);
            SampleItemsSource.MoveCurrentToFirst();


            EditableItemsSource = new ListCollectionView(items);
            EditableItemsSource.MoveCurrentToNext();

            DisableItemsSource = new ListCollectionView(items);
            DisableItemsSource.MoveCurrentToLast();

        }


    }
}
