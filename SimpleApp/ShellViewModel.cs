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
            if (Initialized) return;

            var list = new List<BizViewModel>();
            list.Add(new Vol04ViewModel());
            list.Add(new Vol05ViewModel());
            list.Add(new Vol09ViewModel());
            list.Add(new Vol10ViewModel());
            list.Add(new RadioButtonViewModel());

            
            ViewModels = new ListCollectionView(list);
            ViewModels.MoveCurrentToLast();

            UserName = "User Name";
            Section = "Section";
            StatusMessage = "準備完了";
        }

        public ListCollectionView ViewModels { get; set; }


    }
}
