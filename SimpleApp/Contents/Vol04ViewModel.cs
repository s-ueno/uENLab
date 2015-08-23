using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using uEN.UI;
using uEN.UI.Validation;

namespace SimpleApp.Contents
{
    /// <summary>
    /// 
    /// </summary>
    [VisualElements(typeof(Vol04View))]
    public class Vol04ViewModel : BizViewModel
    {
        public override string Description { get { return "Vol 04"; } }

        [RequiredRule("Please to text input.")]
        public string SampleText { get; set; }

        public void SampleAction()
        {
            ThrowValidationError();

            MessageBox.Show("OK!!");
        }
    }
}
