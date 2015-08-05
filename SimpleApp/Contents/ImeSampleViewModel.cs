using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uEN.UI;
using uEN.UI.AttachedProperties;
using uEN.Core;
namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    [TextInputObserver]
    [VisualElements(typeof(ImeSampleView))]
    public class ImeSampleViewModel : BizViewModel
    {
        public override string Description { get { return "IME Sample"; } }

        [TextInputPolicy(TextInputState.None)]
        public string NoControlText { get; set; }
        [TextInputPolicy(TextInputState.On)]
        public string OnText { get; set; }
        [TextInputPolicy(TextInputState.Off)]
        public string OffText { get; set; }
        [TextInputPolicy(TextInputState.Disable)]
        public string DisableText { get; set; }
        [TextInputPolicy(TextInputState.Hiragana)]
        public string HiraganaText { get; set; }

        [KatakanaAnnotation]
        [TextInputPolicy(TextInputState.Katakana)]
        public string Katakana { get; set; }

        [KatakanaHalfAnnotation]
        [TextInputPolicy(TextInputState.KatakanaHalf)]
        public string KatakanaHalf { get; set; }

        [TextInputPolicy(TextInputState.AlphaFull)]
        public string AlphaFull { get; set; }

        [AlphanumericAnnotation]
        [TextInputPolicy(TextInputState.Alphanumeric)]
        public string Alpha { get; set; }
    }
}
