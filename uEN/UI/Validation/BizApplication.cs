using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using uEN.Core;
using uEN.Utils;

namespace uEN.UI
{
    public class BizApplication : Application
    {
        public ThemeManager ThemeManager
        {
            get { return Singleton<ThemeManager>.Value; }
        }


        public void Start(BizViewModel mainViewModel, AppStyle style = AppStyle.Modern, AppTheme theme = AppTheme.Light)
        {
            ThemeManager.Style = style;
            ThemeManager.Theme = theme;

            var window = new Window();
            window.Content = mainViewModel;
            window.ContentTemplateSelector = new ViewDataTemplateSelector();



            Initialize(window, mainViewModel);
            Run(window);
        }

        protected virtual void Initialize(Window mainWindow, BizViewModel mainViewModel)
        {

        }
    }
}
