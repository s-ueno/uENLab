using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uEN.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="WideTextGenerator"/>
    internal class NarrowTextGenerator
    {
        public string Convert(string input)
        {
            if (input == null)
                return null;

            StringBuilder sb = new StringBuilder();
            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                sb.Append(ConvertChar(chars, i));
            return sb.ToString();
        }
        private static string ConvertChar(char[] chars, int position)
        {
            switch (chars[position])
            {
                case '　':
                case ' ': return " ";
                case '！':
                case '!': return "!";
                case '”':
                case '"': return "\"";
                case '＃':
                case '#': return "#";
                case '＄':
                case '$': return "$";
                case '％':
                case '%': return "%";
                case '＆':
                case '&': return "&";
                case '’':
                case '\'': return "'";
                case '（':
                case '(': return "(";
                case '）':
                case ')': return ")";
                case '＊':
                case '*': return "*";
                case '＋':
                case '+': return "+";
                case '，':
                case ',': return ",";
                case '－':
                case '-': return "-";
                case '．':
                case '.': return ".";
                case '／':
                case '/': return "/";
                case '：':
                case ':': return ":";
                case '；':
                case ';': return ";";
                case '＜':
                case '<': return "<";
                case '＝':
                case '=': return "=";
                case '＞':
                case '>': return ">";
                case '？':
                case '?': return "?";
                case '＠':
                case '@': return "@";
                case '０':
                case '0': return "0";
                case '１':
                case '1': return "1";
                case '２':
                case '2': return "2";
                case '３':
                case '3': return "3";
                case '４':
                case '4': return "4";
                case '５':
                case '5': return "5";
                case '６':
                case '6': return "6";
                case '７':
                case '7': return "7";
                case '８':
                case '8': return "8";
                case '９':
                case '9': return "9";
                case '［':
                case '[': return "[";
                case '￥':
                case '\\': return "\\";
                case '］':
                case ']': return "]";
                case '＾':
                case '^': return "^";
                case '＿':
                case '_': return "_";
                case '‘':
                case '`': return "`";
                case 'Ａ':
                case 'A': return "A";
                case 'Ｂ':
                case 'B': return "B";
                case 'Ｃ':
                case 'C': return "C";
                case 'Ｄ':
                case 'D': return "D";
                case 'Ｅ':
                case 'E': return "E";
                case 'Ｆ':
                case 'F': return "F";
                case 'Ｇ':
                case 'G': return "G";
                case 'Ｈ':
                case 'H': return "H";
                case 'Ｉ':
                case 'I': return "I";
                case 'Ｊ':
                case 'J': return "J";
                case 'Ｋ':
                case 'K': return "K";
                case 'Ｌ':
                case 'L': return "L";
                case 'Ｍ':
                case 'M': return "M";
                case 'Ｎ':
                case 'N': return "N";
                case 'Ｏ':
                case 'O': return "O";
                case 'Ｐ':
                case 'P': return "P";
                case 'Ｑ':
                case 'Q': return "Q";
                case 'Ｒ':
                case 'R': return "R";
                case 'Ｓ':
                case 'S': return "S";
                case 'Ｔ':
                case 'T': return "T";
                case 'Ｕ':
                case 'U': return "U";
                case 'Ｖ':
                case 'V': return "V";
                case 'Ｗ':
                case 'W': return "W";
                case 'Ｘ':
                case 'X': return "X";
                case 'Ｙ':
                case 'Y': return "Y";
                case 'Ｚ':
                case 'Z': return "Z";
                case 'ａ':
                case 'a': return "a";
                case 'ｂ':
                case 'b': return "b";
                case 'ｃ':
                case 'c': return "c";
                case 'ｄ':
                case 'd': return "d";
                case 'ｅ':
                case 'e': return "e";
                case 'f':
                case 'ｆ': return "f";
                case 'ｇ':
                case 'g': return "g";
                case 'ｈ':
                case 'h': return "h";
                case 'ｉ':
                case 'i': return "i";
                case 'ｊ':
                case 'j': return "j";
                case 'ｋ':
                case 'k': return "k";
                case 'ｌ':
                case 'l': return "l";
                case 'ｍ':
                case 'm': return "m";
                case 'ｎ':
                case 'n': return "n";
                case 'ｏ':
                case 'o': return "o";
                case 'ｐ':
                case 'p': return "p";
                case 'ｑ':
                case 'q': return "q";
                case 'ｒ':
                case 'r': return "r";
                case 'ｓ':
                case 's': return "s";
                case 'ｔ':
                case 't': return "t";
                case 'ｕ':
                case 'u': return "u";
                case 'ｖ':
                case 'v': return "v";
                case 'ｗ':
                case 'w': return "w";
                case 'ｘ':
                case 'x': return "x";
                case 'ｙ':
                case 'y': return "y";
                case 'ｚ':
                case 'z': return "z";
                case '｛':
                case '{': return "{";
                case '｜':
                case '|': return "|";
                case '｝':
                case '}': return "}";
                case '～':
                case '~': return "~";
                case '。':
                case '｡': return "｡";
                case '「':
                case '｢': return "｢";
                case '」':
                case '｣': return "｣";
                case '、':
                case '､': return "､";
                case '・':
                case '･': return "･";
                case '゛':
                case 'ﾞ': return "ﾞ";
                case '゜':
                case 'ﾟ': return "ﾟ";
                case 'ヴ': return "ｩﾞ";
                case 'ぁ':
                case 'ァ':
                case 'ｧ': return "ｧ";
                case 'ぃ':
                case 'ィ':
                case 'ｨ': return "ｨ";
                case 'ぅ':
                case 'ゥ':
                case 'ｩ': return "ｩ";
                case 'ぇ':
                case 'ェ':
                case 'ｪ': return "ｪ";
                case 'ぉ':
                case 'ォ':
                case 'ｫ': return "ｫ";
                case 'ゃ':
                case 'ャ':
                case 'ｬ': return "ｬ";
                case 'ゅ':
                case 'ュ':
                case 'ｭ': return "ｭ";
                case 'ょ':
                case 'ョ':
                case 'ｮ': return "ｮ";
                case 'っ':
                case 'ッ':
                case 'ｯ': return "ｯ";
                case 'ー':
                case 'ｰ': return "ｰ";
                case 'あ':
                case 'ア':
                case 'ｱ': return "ｱ";
                case 'い':
                case 'イ':
                case 'ｲ': return "ｲ";
                case 'う':
                case 'ウ':
                case 'ｳ': return "ｳ";
                case 'え':
                case 'エ':
                case 'ｴ': return "ｴ";
                case 'お':
                case 'オ':
                case 'ｵ': return "ｵ";
                case 'な':
                case 'ナ':
                case 'ﾅ': return "ﾅ";
                case 'に':
                case 'ニ':
                case 'ﾆ': return "ﾆ";
                case 'ぬ':
                case 'ヌ':
                case 'ﾇ': return "ﾇ";
                case 'ね':
                case 'ネ':
                case 'ﾈ': return "ﾈ";
                case 'の':
                case 'ノ':
                case 'ﾉ': return "ﾉ";
                case 'ま':
                case 'マ':
                case 'ﾏ': return "ﾏ";
                case 'み':
                case 'ミ':
                case 'ﾐ': return "ﾐ";
                case 'む':
                case 'ム':
                case 'ﾑ': return "ﾑ";
                case 'め':
                case 'メ':
                case 'ﾒ': return "ﾒ";
                case 'も':
                case 'モ':
                case 'ﾓ': return "ﾓ";
                case 'や':
                case 'ヤ':
                case 'ﾔ': return "ﾔ";
                case 'ゆ':
                case 'ユ':
                case 'ﾕ': return "ﾕ";
                case 'よ':
                case 'ヨ':
                case 'ﾖ': return "ﾖ";
                case 'ら':
                case 'ラ':
                case 'ﾗ': return "ﾗ";
                case 'り':
                case 'リ':
                case 'ﾘ': return "ﾘ";
                case 'る':
                case 'ル':
                case 'ﾙ': return "ﾙ";
                case 'れ':
                case 'レ':
                case 'ﾚ': return "ﾚ";
                case 'ろ':
                case 'ロ':
                case 'ﾛ': return "ﾛ";
                case 'わ':
                case 'ワ':
                case 'ﾜ': return "ﾜ";
                case 'を':
                case 'ヲ':
                case 'ｦ': return "ｦ";
                case 'ん':
                case 'ン':
                case 'ﾝ': return "ﾝ";
                case 'か':
                case 'カ':
                case 'ｶ': return "ｶ";
                case 'き':
                case 'キ':
                case 'ｷ': return "ｷ";
                case 'く':
                case 'ク':
                case 'ｸ': return "ｸ";
                case 'け':
                case 'ケ':
                case 'ｹ': return "ｹ";
                case 'こ':
                case 'コ':
                case 'ｺ': return "ｺ";
                case 'さ':
                case 'サ':
                case 'ｻ': return "ｻ";
                case 'し':
                case 'シ':
                case 'ｼ': return "ｼ";
                case 'す':
                case 'ス':
                case 'ｽ': return "ｽ";
                case 'せ':
                case 'セ':
                case 'ｾ': return "ｾ";
                case 'そ':
                case 'ソ':
                case 'ｿ': return "ｿ";
                case 'た':
                case 'タ':
                case 'ﾀ': return "ﾀ";
                case 'ち':
                case 'チ':
                case 'ﾁ': return "ﾁ";
                case 'つ':
                case 'ツ':
                case 'ﾂ': return "ﾂ";
                case 'て':
                case 'テ':
                case 'ﾃ': return "ﾃ";
                case 'と':
                case 'ト':
                case 'ﾄ': return "ﾄ";
                case 'は':
                case 'ハ':
                case 'ﾊ': return "ﾊ";
                case 'ひ':
                case 'ヒ':
                case 'ﾋ': return "ﾋ";
                case 'ふ':
                case 'フ':
                case 'ﾌ': return "ﾌ";
                case 'へ':
                case 'ヘ':
                case 'ﾍ': return "ﾍ";
                case 'ほ':
                case 'ホ':
                case 'ﾎ': return "ﾎ";
                case 'が':
                case 'ガ': return "ｶﾞ";
                case 'ぎ':
                case 'ギ': return "ｷﾞ";
                case 'ぐ':
                case 'グ': return "ｸﾞ";
                case 'げ':
                case 'ゲ': return "ｹﾞ";
                case 'ご':
                case 'ゴ': return "ｺﾞ";
                case 'ざ':
                case 'ザ': return "ｻﾞ";
                case 'じ':
                case 'ジ': return "ｼﾞ";
                case 'ず':
                case 'ズ': return "ｽﾞ";
                case 'ぜ':
                case 'ゼ': return "ｾﾞ";
                case 'ぞ':
                case 'ゾ': return "ｿﾞ";
                case 'だ':
                case 'ダ': return "ﾀﾞ";
                case 'ぢ':
                case 'ヂ': return "ﾁﾞ";
                case 'づ':
                case 'ヅ': return "ﾂﾞ";
                case 'で':
                case 'デ': return "ﾃﾞ";
                case 'ど':
                case 'ド': return "ﾄﾞ";
                case 'ば':
                case 'バ': return "ﾊﾞ";
                case 'び':
                case 'ビ': return "ﾋﾞ";
                case 'ぶ':
                case 'ブ': return "ﾌﾞ";
                case 'べ':
                case 'ベ': return "ﾍﾞ";
                case 'ぼ':
                case 'ボ': return "ﾎﾞ";
                case 'ぱ':
                case 'パ': return "ﾊﾟ";
                case 'ぴ':
                case 'ピ': return "ﾋﾟ";
                case 'ぷ':
                case 'プ': return "ﾌﾟ";
                case 'ぺ':
                case 'ペ': return "ﾍﾟ";
                case 'ぽ':
                case 'ポ': return "ﾎﾟ";
                default: return string.Empty;
            }
        }
    }
}
