﻿using System;
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
    [VisualElements(typeof(Vol10_2View))]
    public class Vol10_2ViewModel : BizViewModel
    {
        public override string Description { get { return Properties.Resources.SecondScreen; } }

        public void GoBackAction()
        {

            Navigator.GoBack();

        }
    }
}
