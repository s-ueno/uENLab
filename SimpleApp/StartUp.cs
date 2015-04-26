using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uEN.UI;

namespace SimpleApp
{
    public static class Startup
    {
        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomainHost)]
        static void Main()
        {
            var app = new MyApplication();
            app.Start(new ShellViewModel());
        }

    }


    public class MyApplication : BizApplication
    {
        protected override void Initialize(System.Windows.Window mainWindow, BizViewModel mainViewModel)
        {
            mainWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
    }

}
