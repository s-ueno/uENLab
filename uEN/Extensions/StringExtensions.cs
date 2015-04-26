using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uEN.Extensions
{
    public static class StringExtensions
    {
        private static readonly Encoding ShiftJis = Encoding.GetEncoding("Shift_JIS");

        public static string ConvertShiftJis(this string s)
        {
            if (string.IsNullOrEmpty(s)) return s;

            var bytes = ShiftJis.GetBytes(s);
            return ShiftJis.GetString(bytes);
        }

        public static int GetShiftJisSize(this string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            return ShiftJis.GetBytes(s).Length;
        }

        public static byte[] GetShiftJisBytes(this string s)
        {
            return ShiftJis.GetBytes(s);
        }




        public static string Encrypt(this string plainText)
        {
            return uEN.Core.Crypto.EncryptStringAES(plainText, Password);
        }
        public static string Decrypt(this string cipherText)
        {
            return uEN.Core.Crypto.DecryptStringAES(cipherText, Password);
        }
        private static readonly string Password = BizUtils.AppSettings("CryptoKey", "abcdefg123");
    }
}
