using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using uEN.Core;
using uEN.UI;

namespace uEN.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(FontSizeView))]
    public class FontSizeViewModel : BizViewModel
    {
        public override string Description { get { return "フォント サイズ"; } }
        public override void ApplyView()
        {
            var list = new List<double>();
            for (double i = 7; i <= 50; i += 0.5)
            {
                list.Add(i);
            }
            FontSizeCollection = new ListCollectionView(list);
            FontSizeCollection.MoveCurrentTo(Singleton<ThemeManager>.Value.FontSize);
            FontSizeCollection.CurrentChanged -= FontSizeCollection_CurrentChanged;                
            FontSizeCollection.CurrentChanged += FontSizeCollection_CurrentChanged;                
        }
        public ListCollectionView FontSizeCollection { get; set; }

        void FontSizeCollection_CurrentChanged(object sender, EventArgs e)
        {
            var size = FontSizeCollection.CurrentItem as double?;
            if (size.HasValue)
            {
                Singleton<ThemeManager>.Value.FontSize = size.Value;
            }
        }
    }
}
