using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uEN.UI;

namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(Vol10_1View))]
    public class Vol10_1ViewModel : BizViewModel
    {
        public override string Description { get { return "遷移元サンプル"; } }

        public void GoForwardAction()
        {
            var nextViewModel = new Vol10_2ViewModel();
            Navigator.GoForward(nextViewModel);
        }

        public void GoBackAction()
        {
            Navigator.GoBack();
        }
    }
}
