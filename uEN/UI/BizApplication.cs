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

namespace uEN.UI
{
    public class BizApplication : Application
    {
        public bool IsShuttingDown { get; private set; }
        protected override void OnExit(ExitEventArgs e)
        {
            this.IsShuttingDown = true;
            base.OnExit(e);
        }

        public ThemeManager ThemeManager
        {
            get { return Singleton<ThemeManager>.Value; }
        }


        public void Start(BizViewModel mainViewModel, AppStyle style = AppStyle.Modern, AppTheme theme = AppTheme.Light)
        {
            if (IsShuttingDown) return;

            ShutdownMode = System.Windows.ShutdownMode.OnMainWindowClose;

            ThemeManager.Style = style;
            if (!ThemeManager.Theme.HasValue)
                ThemeManager.Theme = theme;


            var window = new Window();
            window.Content = mainViewModel;
            window.ContentTemplateSelector = new ViewDataTemplateSelector();

            mainViewModel.IsWindowContent = true;

            Initialize(window, mainViewModel);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.DispatcherUnhandledException += BizApplication_DispatcherUnhandledException;

            Run(window);
        }
        protected virtual void Initialize(Window mainWindow, BizViewModel mainViewModel)
        {

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            try
            {
                OnUnhandledExceptionRaw(sender, ex);
            }
            catch
            {
            }
        }
        void BizApplication_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                OnUnhandledExceptionRaw(sender, e.Exception);
            }
            catch
            {
            }
            finally
            {
                e.Handled = true;
            }
        }
        void OnUnhandledExceptionRaw(object sender, Exception ex)
        {
            if (engaged) return;

            engaged = true;
            try
            {
                OnUnhandledException(sender, ex);
            }
            finally
            {
                engaged = false;
            }
        }
        static volatile bool engaged = false;

        protected virtual void OnUnhandledException(object sender, Exception ex)
        {
            var exPolicy = Repository.GetPriorityExport<uEN.UI.DataBinding.IExceptionPolicy>();
            if (exPolicy != null)
                exPolicy.Do(sender, ex);
        }
    }
}
