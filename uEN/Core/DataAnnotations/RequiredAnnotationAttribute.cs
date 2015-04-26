using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace uEN.Core
{
    /// <summary>
    /// 必須入力を表します
    /// </summary>
    public class RequiredAnnotationAttribute : RequiredAttribute 
    {
        public RequiredAnnotationAttribute(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
