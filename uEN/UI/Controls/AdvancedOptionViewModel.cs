﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using uEN.Core;
using uEN.UI;
using uEN.UI.AttachedProperties;
using uEN.UI.Validation;

namespace uEN.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(AdvancedOptionView))]
    public class AdvancedOptionViewModel : BizViewModel
    {
        public override string Description { get { return "高度な設定"; } }
        public bool UseAlternatingRowBackground
        {
            get { return Singleton<ThemeManager>.Value.UseAlternatingRowBackground; }
            set { Singleton<ThemeManager>.Value.UseAlternatingRowBackground = value; }
        }
        public void AlternatingRowBackgroundChanged()
        {
            Singleton<ThemeManager>.Value.AlternatingRowBackgroundChanged();
        }
    }
}