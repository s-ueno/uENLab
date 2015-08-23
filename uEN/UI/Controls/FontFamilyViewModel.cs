using System;
using System.Collections.Generic;
using System.Linq;
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
    [VisualElements(typeof(FontFamilyView))]
    public class FontFamilyViewModel : BizViewModel
    {
        public override string Description { get { return Properties.Resources.FontFamily; } }
        public override void ApplyView()
        {
            var fonts = Fonts.SystemFontFamilies.OrderBy(x => x.ToString()).ToList();
            FontCollection = new ListCollectionView(fonts);
            FontCollection.MoveCurrentTo(Singleton<ThemeManager>.Value.Font);
            FontCollection.CurrentChanged += (sender, e) =>
            {
                Singleton<ThemeManager>.Value.Font = FontCollection.CurrentItem as FontFamily;
            };
        }
        public ListCollectionView FontCollection { get; set; }

    }
}
