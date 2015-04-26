using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace uEN.Core
{
    public class RegexAnnotationAttribute : RegularExpressionAttribute 
    {
        public RegexAnnotationAttribute(string expression)
            : base(expression)
        {

        }

    }
}
