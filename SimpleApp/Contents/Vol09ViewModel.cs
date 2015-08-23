using SimpleApp.Properties;
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
    [VisualElements(typeof(Vol09View))]
    public class Vol09ViewModel : BizViewModel
    {
        public override string Description { get { return Resources.MessageBox; } }

        public void OkAction()
        {
            this.ShowOk(Resources.DesignOfTheMessage,
                Resources.MessageSample_01,
                OkActionInternal);
        }
        private void OkActionInternal()
        {
            StatusMessage = "click 'OK'.";
        }



        public void YesNoAction()
        {
            this.ShowYesNo(Resources.DesignOfTheMessage,
                Resources.MessageSample_02,
                YesAction,
                () => StatusMessage = "click 'No'.");
        }
        private void YesAction()
        {
            StatusMessage = "click 'Yes'";
        }



        public void YesNoCancelAction()
        {
            this.ShowYesNoCancel(Resources.DesignOfTheMessage,
                Resources.MessageSample_03,
                () => StatusMessage = "click 'YES'",
                () => StatusMessage = "click NO",
                null);
        }





        public void CustomAction()
        {
            this.ShowMessage(Resources.DesignOfTheMessage,
                Resources.MessageSample_04,
                new MessageDialogHelper.Command("Yes, it is", () => { }, true),
                new MessageDialogHelper.Command("No, I will not.", () => { }));
        }
    }
}
