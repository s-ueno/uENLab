using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uEN.Utils;

namespace uEN.UI
{
    public class BizApplication : Application
    {
        public void Start(BizViewModel mainViewModel)
        {
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
