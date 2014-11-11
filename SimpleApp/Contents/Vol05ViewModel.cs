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
    [VisualElements(typeof(Vol05View))]
    public class Vol05ViewModel : BizViewModel
    {
        public override string Description { get { return "Vol 05"; } }


        internal void SampleAction()
        {
            var view = this.View as Vol05View;



        }
    }
}
