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
    /// <seealso cref="NarrowTextGenerator"/>
    internal class WideTextGenerator
    {
        public bool UsesHiragana { get; set; }
        public string Convert(string input)
        {
            if (input == null)
                return null;

            StringBuilder sb = new StringBuilder();
            var chars = input.ToCharArray();
            bool jumped;
            for (int i = 0; i < chars.Length; i++)
            {
                sb.Append(ConvertChar(chars, i, UsesHiragana, out jumped));
                if (jumped)
                    i++;
            }
            return sb.ToString();
        }

        private static char ConvertChar(char[] chars, int position, bool usesHiragana, out bool jumped)
        {
            jumped = false;
            switch (chars[position])
            {
                case ' ': return '　';
                case '!': return '！';
                case '"': return '”';
                case '#': return '＃';
                case '$': return '＄';
                case '%': return '％';
                case '&': return '＆';
                case '\'': return '’';
                case '(': return '（';
                case ')': return '）';
                case '*': return '＊';
                case '+': return '＋';
                case ',': return '，';
                case '-': return '－';
                case '.': return '．';
                case '/': return '／';
                case ':': return '：';
                case ';': return '；';
                case '<': return '＜';
                case '=': return '＝';
                case '>': return '＞';
                case '?': return '？';
                case '@': return '＠';

                case '0': return '０';
                case '1': return '１';
                case '2': return '２';
                case '3': return '３';
                case '4': return '４';
                case '5': return '５';
                case '6': return '６';
                case '7': return '７';
                case '8': return '８';
                case '9': return '９';
                case '[': return '［';
                case '\\': return '￥';
                case ']': return '］';
                case '^': return '＾';
                case '_': return '＿';
                case '`': return '‘';
                case 'A': return 'Ａ';
                case 'B': return 'Ｂ';
                case 'C': return 'Ｃ';
                case 'D': return 'Ｄ';
                case 'E': return 'Ｅ';
                case 'F': return 'Ｆ';
                case 'G': return 'Ｇ';
                case 'H': return 'Ｈ';
                case 'I': return 'Ｉ';
                case 'J': return 'Ｊ';
                case 'K': return 'Ｋ';
                case 'L': return 'Ｌ';
                case 'M': return 'Ｍ';
                case 'N': return 'Ｎ';
                case 'O': return 'Ｏ';
                case 'P': return 'Ｐ';
                case 'Q': return 'Ｑ';
                case 'R': return 'Ｒ';
                case 'S': return 'Ｓ';
                case 'T': return 'Ｔ';
                case 'U': return 'Ｕ';
                case 'V': return 'Ｖ';
                case 'W': return 'Ｗ';
                case 'X': return 'Ｘ';
                case 'Y': return 'Ｙ';
                case 'Z': return 'Ｚ';
                case 'a': return 'ａ';
                case 'b': return 'ｂ';
                case 'c': return 'ｃ';
                case 'd': return 'ｄ';
                case 'e': return 'ｅ';
                case 'f': return 'ｆ';
                case 'g': return 'ｇ';
                case 'h': return 'ｈ';
                case 'i': return 'ｉ';
                case 'j': return 'ｊ';
                case 'k': return 'ｋ';
                case 'l': return 'ｌ';
                case 'm': return 'ｍ';
                case 'n': return 'ｎ';
                case 'o': return 'ｏ';
                case 'p': return 'ｐ';
                case 'q': return 'ｑ';
                case 'r': return 'ｒ';
                case 's': return 'ｓ';
                case 't': return 'ｔ';
                case 'u': return 'ｕ';
                case 'v': return 'ｖ';
                case 'w': return 'ｗ';
                case 'x': return 'ｘ';
                case 'y': return 'ｙ';
                case 'z': return 'ｚ';
                case '{': return '｛';
                case '|': return '｜';
                case '}': return '｝';
                case '~': return '～';
                case '｡': return '。';
                case '｢': return '「';
                case '｣': return '」';
                case '､': return '、';
                case '･': return '・';
                case 'ﾞ': return '゛';
                case 'ﾟ': return '゜';
                case 'ｧ': return usesHiragana ? 'ぁ' : 'ァ';
                case 'ｨ': return usesHiragana ? 'ぃ' : 'ィ';
                case 'ｩ': return usesHiragana ? 'ぅ' : 'ゥ';
                case 'ｪ': return usesHiragana ? 'ぇ' : 'ェ';
                case 'ｫ': return usesHiragana ? 'ぉ' : 'ォ';
                case 'ｬ': return usesHiragana ? 'ゃ' : 'ャ';
                case 'ｭ': return usesHiragana ? 'ゅ' : 'ュ';
                case 'ｮ': return usesHiragana ? 'ょ' : 'ョ';
                case 'ｯ': return usesHiragana ? 'っ' : 'ッ';
                case 'ｰ': return 'ー';
                case 'ｱ': return usesHiragana ? 'あ' : 'ア';
                case 'ｲ': return usesHiragana ? 'い' : 'イ';
                case 'ｳ': return usesHiragana ? 'う' : 'ウ';
                case 'ｴ': return usesHiragana ? 'え' : 'エ';
                case 'ｵ': return usesHiragana ? 'お' : 'オ';
                case 'ﾅ': return usesHiragana ? 'な' : 'ナ';
                case 'ﾆ': return usesHiragana ? 'に' : 'ニ';
                case 'ﾇ': return usesHiragana ? 'ぬ' : 'ヌ';
                case 'ﾈ': return usesHiragana ? 'ね' : 'ネ';
                case 'ﾉ': return usesHiragana ? 'の' : 'ノ';
                case 'ﾏ': return usesHiragana ? 'ま' : 'マ';
                case 'ﾐ': return usesHiragana ? 'み' : 'ミ';
                case 'ﾑ': return usesHiragana ? 'む' : 'ム';
                case 'ﾒ': return usesHiragana ? 'め' : 'メ';
                case 'ﾓ': return usesHiragana ? 'も' : 'モ';
                case 'ﾔ': return usesHiragana ? 'や' : 'ヤ';
                case 'ﾕ': return usesHiragana ? 'ゆ' : 'ユ';
                case 'ﾖ': return usesHiragana ? 'よ' : 'ヨ';
                case 'ﾗ': return usesHiragana ? 'ら' : 'ラ';
                case 'ﾘ': return usesHiragana ? 'り' : 'リ';
                case 'ﾙ': return usesHiragana ? 'る' : 'ル';
                case 'ﾚ': return usesHiragana ? 'れ' : 'レ';
                case 'ﾛ': return usesHiragana ? 'ろ' : 'ロ';
                case 'ﾜ': return usesHiragana ? 'わ' : 'ワ';
                case 'ｦ': return usesHiragana ? 'を' : 'ヲ';
                case 'ﾝ': return usesHiragana ? 'ん' : 'ン';
                case 'ｶ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'が' : 'ガ') : (usesHiragana ? 'か' : 'カ');
                case 'ｷ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ぎ' : 'ギ') : (usesHiragana ? 'き' : 'キ');
                case 'ｸ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ぐ' : 'グ') : (usesHiragana ? 'く' : 'ク');
                case 'ｹ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'げ' : 'ゲ') : (usesHiragana ? 'け' : 'ケ');
                case 'ｺ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ご' : 'ゴ') : (usesHiragana ? 'こ' : 'コ');
                case 'ｻ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ざ' : 'ザ') : (usesHiragana ? 'さ' : 'サ');
                case 'ｼ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'じ' : 'ジ') : (usesHiragana ? 'し' : 'シ');
                case 'ｽ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ず' : 'ズ') : (usesHiragana ? 'す' : 'ス');
                case 'ｾ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ぜ' : 'ゼ') : (usesHiragana ? 'せ' : 'セ');
                case 'ｿ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ぞ' : 'ゾ') : (usesHiragana ? 'そ' : 'ソ');
                case 'ﾀ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'だ' : 'ダ') : (usesHiragana ? 'た' : 'タ');
                case 'ﾁ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ぢ' : 'ヂ') : (usesHiragana ? 'ち' : 'チ');
                case 'ﾂ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'づ' : 'ヅ') : (usesHiragana ? 'つ' : 'ツ');
                case 'ﾃ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'で' : 'デ') : (usesHiragana ? 'て' : 'テ');
                case 'ﾄ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ど' : 'ド') : (usesHiragana ? 'と' : 'ト');
                case 'ﾊ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ば' : 'バ') : CheckConsonantB(chars, position, out jumped) ? (usesHiragana ? 'ぱ' : 'パ') : (usesHiragana ? 'は' : 'ハ');
                case 'ﾋ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'び' : 'ビ') : CheckConsonantB(chars, position, out jumped) ? (usesHiragana ? 'ぴ' : 'ピ') : (usesHiragana ? 'ひ' : 'ヒ');
                case 'ﾌ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ぶ' : 'ブ') : CheckConsonantB(chars, position, out jumped) ? (usesHiragana ? 'ぷ' : 'プ') : (usesHiragana ? 'ふ' : 'フ');
                case 'ﾍ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'べ' : 'ベ') : CheckConsonantB(chars, position, out jumped) ? (usesHiragana ? 'ぺ' : 'ペ') : (usesHiragana ? 'へ' : 'ヘ');
                case 'ﾎ': return CheckConsonantA(chars, position, out jumped) ? (usesHiragana ? 'ぼ' : 'ボ') : CheckConsonantB(chars, position, out jumped) ? (usesHiragana ? 'ぽ' : 'ポ') : (usesHiragana ? 'ほ' : 'ホ');
                default: return chars[position];
            }
        }
        private static bool CheckConsonantA(char[] chars, int position, out bool jumped)
        {
            if (chars.Length - 1 <= position)
            {
                jumped = false;
                return jumped;
            }

            jumped = chars[position + 1] == 'ﾞ';
            return jumped;
        }
        private static bool CheckConsonantB(char[] chars, int position, out bool jumped)
        {
            if (chars.Length - 1 <= position)
            {
                jumped = false;
                return jumped;
            }
            jumped = chars[position + 1] == 'ﾟ';
            return jumped;
        }
        public void SetArguments(object[] arguments)
        {
            if (arguments == null || arguments.Length != 1 || (!(arguments[0] is bool)))
                return;
            this.UsesHiragana = (bool)arguments[0];
        }
    }
}
