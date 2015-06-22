using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace uEN.Core
{
    /// <summary>
    /// 半角英数
    /// </summary>
    public class AlphanumericAnnotationAttribute : RegexAnnotationAttribute
    {
        public AlphanumericAnnotationAttribute() : base(@"(^([ -~])+$)") { }
    }

    /// <summary>
    /// 半角カナ
    /// </summary>
    public class KatakanaHalfAnnotationAttribute : RegexAnnotationAttribute
    {
        public KatakanaHalfAnnotationAttribute() : base(@"(^([｡-ﾟ -~])+$)") { }
    }

    /// <summary>
    /// カタカナ
    /// </summary>
    public class KatakanaAnnotationAttribute : RegexAnnotationAttribute
    {
        public KatakanaAnnotationAttribute() : base(@"(^([\p{IsKatakana}ー]|\\s)+$)") { }
    }

    /// <summary>
    /// ひらがな
    /// </summary>
    public class HiraganaAnnotationAttribute : RegexAnnotationAttribute
    {
        public HiraganaAnnotationAttribute() : base(@"(^([\p{IsHiragana}ー]|\\s)+$)") { }
    }
}
