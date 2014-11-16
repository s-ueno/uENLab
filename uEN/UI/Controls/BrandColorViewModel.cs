using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using uEN.Core;
using uEN.UI;

namespace uEN.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(BrandColorView))]
    public class BrandColorViewModel : BizViewModel
    {
        public override string Description { get { return "ブランド カラー"; } }

        public override void ApplyView()
        {
            ColorCollection = new ListCollectionView(ColorToStringConverter.ListColors().ToList());
            ColorCollection.MoveCurrentTo(Singleton<ThemeManager>.Value.BrandColor);
            ColorCollection.CurrentChanged -= ColorCollection_CurrentChanged;
            ColorCollection.CurrentChanged += ColorCollection_CurrentChanged;
        }

        void ColorCollection_CurrentChanged(object sender, EventArgs e)
        {
            Singleton<ThemeManager>.Value.BrandColor = ColorCollection.CurrentItem as Color?;
        }
        public ListCollectionView ColorCollection { get; set; }

    }
}
