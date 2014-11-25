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
    [VisualElements(typeof(Vol10View))]
    public class Vol10ViewModel : BizViewModel
    {
        public override string Description { get { return "画面遷移"; } }

        

        public void StartAction()
        {
            var viewModel = new Vol10_1ViewModel();
            Navigator.GoForward(viewModel);
        }


       
    }
}
