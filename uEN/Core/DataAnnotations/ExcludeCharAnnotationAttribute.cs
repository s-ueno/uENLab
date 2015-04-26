using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace uEN.Core
{
    /// <summary>
    /// 禁則文字を表します
    /// </summary>
    public class ExcludeCharAnnotationAttribute : ValidationAttribute
    {
        public ExcludeCharAnnotationAttribute(string characters)
        {
            Characters = characters;
        }
        public string Characters { get; private set; }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var s = Convert.ToString(value);
            foreach (var each in Characters)
            {
                if (s.Contains(each))
                {

                    return false;
                }
            }
            return true;
        }


    }
}
