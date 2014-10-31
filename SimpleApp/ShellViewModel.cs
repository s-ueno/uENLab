using SimpleApp.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using uEN.UI;
using uEN.UI.Validation;

namespace SimpleApp
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(ShellView))]
    public class ShellViewModel : BizViewModel
    {
        public override void ApplyView()
        {
            var list = new List<BizViewModel>();
            list.Add(new Vol04ViewModel());
            ViewModels = new ListCollectionView(list);   
        }

        public ListCollectionView ViewModels { get; set; }


    }
}
