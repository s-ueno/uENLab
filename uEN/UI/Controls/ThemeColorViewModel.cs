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
    [VisualElements(typeof(ThemeColorView))]
    public class ThemeColorViewModel : BizViewModel
    {
        public override string Description { get { return "Window カラー"; } }
        public bool UseWhiteTheme { get; set; }
        public bool UseBlackTheme { get; set; }
        public bool UseGlassBuleTheme { get; set; }



        public bool UseGlassYellowTheme { get; set; }
        public bool AllowGlassYellow
        {
            get { return allowGlassYellow; }
        }
        static readonly bool allowGlassYellow = BizUtils.AppSettings("Theme.AllowGlassYellow", false);

        public bool UseGlassRedTheme { get; set; }
        public bool AllowGlassRed
        {
            get { return allowGlassRed; }
        }
        static readonly bool allowGlassRed = BizUtils.AppSettings("Theme.AllowGlassRed", false);


        public bool UseGlassGreenTheme { get; set; }
        public bool AllowGlassGreen
        {
            get { return allowGlassGreen; }
        }
        static readonly bool allowGlassGreen = BizUtils.AppSettings("Theme.AllowGlassGreen", false);


        public bool UseGlassBrandTheme { get; set; }
        public bool AllowGlassBrand
        {
            get { return allowGlassBrand; }
        }
        static readonly bool allowGlassBrand = BizUtils.AppSettings("Theme.AllowGlassBrand", true);

        public override void ApplyView()
        {
            var themeManager = Singleton<ThemeManager>.Value;
            if (themeManager.Theme == AppTheme.Light)
            {
                UseWhiteTheme = true;
            }
            if (themeManager.Theme == AppTheme.Dark)
            {
                UseBlackTheme = true;
            }
            if (themeManager.Theme == AppTheme.GlassBlue)
            {
                UseGlassBuleTheme = true;
            }
            if (themeManager.Theme == AppTheme.GlassYellow)
            {
                UseGlassYellowTheme = true;
            }
            if (themeManager.Theme == AppTheme.GlassRed)
            {
                UseGlassRedTheme = true;
            }
            if (themeManager.Theme == AppTheme.GlassGreen)
            {
                UseGlassGreenTheme = true;
            }
            if (themeManager.Theme == AppTheme.GlassBrand)
            {
                UseGlassBrandTheme = true;
            }
        }
        public void CheckedAction()
        {
            SetTheme();
        }
        public void UnCheckedAction()
        {
            SetTheme();
        }
        protected void SetTheme()
        {
            if (UseWhiteTheme)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.Light;
            }
            if (UseBlackTheme)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.Dark;
            }
            if (UseGlassBuleTheme)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.GlassBlue;
            }
            if (UseGlassYellowTheme)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.GlassYellow;
            }
            if (UseGlassRedTheme)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.GlassRed;
            }
            if (UseGlassGreenTheme)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.GlassGreen;
            }
            if (UseGlassBrandTheme)
            {
                Singleton<ThemeManager>.Value.Theme = AppTheme.GlassBrand;
            }
        }
    }
}
