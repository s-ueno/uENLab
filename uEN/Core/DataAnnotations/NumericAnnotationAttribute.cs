using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace uEN.Core
{
    /// <summary>
    /// 数値を表します
    /// </summary>
    public class NumericAnnotationAttribute : ValidationAttribute
    {
        /// <summary>
        /// 数値を表す付加情報です。
        /// </summary>
        /// <param name="precision">有効桁数</param>
        public NumericAnnotationAttribute(int precision)
            : this(precision, 0)
        {
        }
        /// <summary>
        /// 数値を表す付加情報です。
        /// </summary>
        /// <param name="precision">有効桁数</param>
        /// <param name="scale">少数桁数</param>
        public NumericAnnotationAttribute(int precision, int scale)
            : this(precision, scale, true)
        {
        }
        /// <summary>
        /// 数値を表す付加情報です。
        /// </summary>
        /// <param name="precision">有効桁数</param>
        /// <param name="scale">少数桁数</param>
        /// <param name="unsigned">true:正の値のみ。false：負値が入ることも考慮。</param>
        public NumericAnnotationAttribute(int precision, int scale, bool unsigned)
        {
            if (precision < scale)
                throw new InvalidOperationException("precision<scale.　precisionは少数桁を含めた総桁数を設定する必要があります。");
            Precision = precision;
            Scale = scale;
            Unsigned = unsigned;
        }
        public int Precision { get; private set; }
        public int Scale { get; private set; }
        public bool Unsigned { get; private set; }
        public override bool IsValid(object value)
        {
            var s = Convert.ToString(value);
            if (string.IsNullOrEmpty(s)) return true; //Nullableに対して、必須入力チェックではない。

            if (!Unsigned && s == "-") return true;

            decimal dec;
            var ret = decimal.TryParse(Convert.ToString(value), out dec);
            if (!ret) return false;

            if (Unsigned && dec < 0) return false;

            //http://stackoverflow.com/questions/763942/calculate-system-decimal-precision-and-scale
            uint[] bits = (uint[])(object)decimal.GetBits(dec);
            decimal mantissa = (bits[2] * 4294967296m * 4294967296m) +
                               (bits[1] * 4294967296m) +
                                bits[0];
            var scale = bits[3] >> 16 & 0x00FF;
            uint precision = 0;
            if (dec != 0m)
            {
                for (decimal tmp = mantissa; tmp >= 1; tmp /= 10)
                {
                    precision++;
                }
            }
            else
            {
                precision = scale + 1;
            }
            return precision <= Precision && scale <= Scale;
        }

    }
}
