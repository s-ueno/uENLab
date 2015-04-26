using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace uEN.Core
{
    /// <summary>
    /// 許可文字を表します
    /// </summary>
    public class IncludeCharAnnotationAttribute : ValidationAttribute
    {
        public IncludeCharAnnotationAttribute(string characters)
        {
            Characters = characters;
        }
        public string Characters { get; private set; }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var s = Convert.ToString(value);

            foreach (var each in s)
            {
                if (!Characters.Contains(each))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
