using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using uEN.Extensions;

namespace uEN.Core
{
    //.net 4.5
    public class MaxLengthAnnotationAttribute : ValidationAttribute
    {
        public MaxLengthAnnotationAttribute(int length, bool isShiftJisByteSize = false)
        {
            Length = length;
            IsShiftJisByteSize = isShiftJisByteSize;
        }
        public int Length { get; private set; }
        public bool IsShiftJisByteSize { get; private set; }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var s = Convert.ToString(value);

            if (!IsShiftJisByteSize)
            {
                if (Length < s.Length)
                {
                    return false;
                }
            }
            else
            {
                var size = s.GetShiftJisSize();
                if (Length < size)
                {
                    return false;
                }
            }
            return true;
        }


    }
}
