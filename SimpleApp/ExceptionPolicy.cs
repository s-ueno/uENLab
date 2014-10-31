using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uEN;
using uEN.UI.Binding;

namespace SimpleApp
{
    [Export(typeof(IExceptionPolicy))]
    public class ExceptionPolicy : IExceptionPolicy
    {
        public void Do(Exception ex)
        {
            var appException = ex as BizApplicationException;
            if (appException != null)
            {
                MessageBox.Show(ex.Message, "サンプル", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            throw ex;
        }
    }
}
