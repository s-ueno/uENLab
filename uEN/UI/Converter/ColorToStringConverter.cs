using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace uEN.UI
{
    public class ColorToStringConverter : IValueConverter
    {
        public static IEnumerable<Color> ListColors()
        {
            foreach (var each in dic.Keys)
            {
                yield return each;
            }
        }

        static ColorToStringConverter()
        {
            //定義済みカラーより和色がキレイでした。
            /*
            var pis = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.Public);
            foreach (var p in pis)
            {
                try
                {
                    var c = (Color)p.GetValue(null, null);
                    dic[c] = p.Name;
                }
                catch
                {
                }
            }
            */
            dic[(Color)ColorConverter.ConvertFromString("#EF454A")] = "朱色 ";
            dic[(Color)ColorConverter.ConvertFromString("#94474B")] = "蘇芳 ";
            dic[(Color)ColorConverter.ConvertFromString("#E38089")] = "桃色 ";
            dic[(Color)ColorConverter.ConvertFromString("#DF828A")] = "紅梅色 ";
            dic[(Color)ColorConverter.ConvertFromString("#AD3140")] = "臙脂 ";
            dic[(Color)ColorConverter.ConvertFromString("#FF7F8F")] = "珊瑚色 ";
            dic[(Color)ColorConverter.ConvertFromString("#FBDADE")] = "桜色 ";
            dic[(Color)ColorConverter.ConvertFromString("#9E2236")] = "茜色 ";
            dic[(Color)ColorConverter.ConvertFromString("#E64B6B")] = "韓紅 ";
            dic[(Color)ColorConverter.ConvertFromString("#B81A3E")] = "紅赤 ";
            dic[(Color)ColorConverter.ConvertFromString("#D53E62")] = "薔薇色 ";
            dic[(Color)ColorConverter.ConvertFromString("#BE0032")] = "赤 ";
            dic[(Color)ColorConverter.ConvertFromString("#FA9CB8")] = "鴇色 ";
            dic[(Color)ColorConverter.ConvertFromString("#BE003F")] = "紅色 ";
            dic[(Color)ColorConverter.ConvertFromString("#CF4078")] = "躑躅色 ";
            dic[(Color)ColorConverter.ConvertFromString("#DA508F")] = "赤紫 ";
            dic[(Color)ColorConverter.ConvertFromString("#C94093")] = "牡丹色 ";
            dic[(Color)ColorConverter.ConvertFromString("#C573B2")] = "菖蒲色 ";
            dic[(Color)ColorConverter.ConvertFromString("#473946")] = "茄子紺 ";
            dic[(Color)ColorConverter.ConvertFromString("#422C41")] = "紫紺 ";
            dic[(Color)ColorConverter.ConvertFromString("#765276")] = "古代紫 ";
            dic[(Color)ColorConverter.ConvertFromString("#A757A8")] = "紫 ";
            dic[(Color)ColorConverter.ConvertFromString("#614876")] = "江戸紫 ";
            dic[(Color)ColorConverter.ConvertFromString("#665971")] = "鳩羽色 ";
            dic[(Color)ColorConverter.ConvertFromString("#744B98")] = "菖蒲色 ";
            dic[(Color)ColorConverter.ConvertFromString("#714C99")] = "菫色 ";
            dic[(Color)ColorConverter.ConvertFromString("#7445AA")] = "青紫 ";
            dic[(Color)ColorConverter.ConvertFromString("#9883C9")] = "藤紫 ";
            dic[(Color)ColorConverter.ConvertFromString("#A294C8")] = "藤色 ";
            dic[(Color)ColorConverter.ConvertFromString("#69639A")] = "藤納戸 ";
            dic[(Color)ColorConverter.ConvertFromString("#353573")] = "紺藍 ";
            dic[(Color)ColorConverter.ConvertFromString("#292934")] = "鉄紺 ";
            dic[(Color)ColorConverter.ConvertFromString("#4347A2")] = "桔梗色 ";
            dic[(Color)ColorConverter.ConvertFromString("#3A3C4F")] = "勝色 ";
            dic[(Color)ColorConverter.ConvertFromString("#384D98")] = "群青色 ";
            dic[(Color)ColorConverter.ConvertFromString("#435AA0")] = "杜若色 ";
            dic[(Color)ColorConverter.ConvertFromString("#343D55")] = "紺色 ";
            dic[(Color)ColorConverter.ConvertFromString("#3A4861")] = "紺青 ";
            dic[(Color)ColorConverter.ConvertFromString("#27477A")] = "瑠璃紺 ";
            dic[(Color)ColorConverter.ConvertFromString("#89ACD7")] = "勿忘草色 ";
            dic[(Color)ColorConverter.ConvertFromString("#72777D")] = "鉛色 ";
            dic[(Color)ColorConverter.ConvertFromString("#00519A")] = "瑠璃色 ";
            dic[(Color)ColorConverter.ConvertFromString("#223546")] = "濃藍 ";
            dic[(Color)ColorConverter.ConvertFromString("#2B618F")] = "縹色 ";
            dic[(Color)ColorConverter.ConvertFromString("#2B4B65")] = "藍色 ";
            dic[(Color)ColorConverter.ConvertFromString("#006AB6")] = "青 ";
            dic[(Color)ColorConverter.ConvertFromString("#89BDDE")] = "空色 ";
            dic[(Color)ColorConverter.ConvertFromString("#007BC3")] = "露草色 ";
            dic[(Color)ColorConverter.ConvertFromString("#576D79")] = "藍鼠 ";
            dic[(Color)ColorConverter.ConvertFromString("#9DCCE0")] = "水色 ";
            dic[(Color)ColorConverter.ConvertFromString("#7EB1C1")] = "甕覗き ";
            dic[(Color)ColorConverter.ConvertFromString("#73B3C1")] = "白群 ";
            dic[(Color)ColorConverter.ConvertFromString("#00687C")] = "納戸色 ";
            dic[(Color)ColorConverter.ConvertFromString("#00859B")] = "浅葱色 ";
            dic[(Color)ColorConverter.ConvertFromString("#53A8B7")] = "新橋色 ";
            dic[(Color)ColorConverter.ConvertFromString("#6D969C")] = "水浅葱 ";
            dic[(Color)ColorConverter.ConvertFromString("#608A8E")] = "錆浅葱 ";
            dic[(Color)ColorConverter.ConvertFromString("#008E94")] = "青緑 ";
            dic[(Color)ColorConverter.ConvertFromString("#24433E")] = "鉄色 ";
            dic[(Color)ColorConverter.ConvertFromString("#6AA89D")] = "青竹色 ";
            dic[(Color)ColorConverter.ConvertFromString("#00A37E")] = "若竹色 ";
            dic[(Color)ColorConverter.ConvertFromString("#00533E")] = "萌葱色 ";
            dic[(Color)ColorConverter.ConvertFromString("#6DA895")] = "青磁色 ";
            dic[(Color)ColorConverter.ConvertFromString("#007B50")] = "常磐色 ";
            dic[(Color)ColorConverter.ConvertFromString("#005638")] = "深緑 ";
            dic[(Color)ColorConverter.ConvertFromString("#00B66E")] = "緑 ";
            dic[(Color)ColorConverter.ConvertFromString("#3C6754")] = "千歳緑 ";
            dic[(Color)ColorConverter.ConvertFromString("#4D8169")] = "緑青色 ";
            dic[(Color)ColorConverter.ConvertFromString("#BADBC7")] = "白緑 ";
            dic[(Color)ColorConverter.ConvertFromString("#6E7972")] = "利休鼠 ";
            dic[(Color)ColorConverter.ConvertFromString("#687E52")] = "松葉色 ";
            dic[(Color)ColorConverter.ConvertFromString("#A9C087")] = "若葉色 ";
            dic[(Color)ColorConverter.ConvertFromString("#737C3E")] = "草色 ";
            dic[(Color)ColorConverter.ConvertFromString("#97A61E")] = "萌黄 ";
            dic[(Color)ColorConverter.ConvertFromString("#AAB300")] = "若草色 ";
            dic[(Color)ColorConverter.ConvertFromString("#BBC000")] = "黄緑 ";
            dic[(Color)ColorConverter.ConvertFromString("#7C7A37")] = "苔色 ";
            dic[(Color)ColorConverter.ConvertFromString("#C2BD3D")] = "鶸色 ";
            dic[(Color)ColorConverter.ConvertFromString("#706C3E")] = "鶯色 ";
            dic[(Color)ColorConverter.ConvertFromString("#D6C949")] = "黄檗色 ";
            dic[(Color)ColorConverter.ConvertFromString("#C0BA7F")] = "抹茶色 ";
            dic[(Color)ColorConverter.ConvertFromString("#EDD60E")] = "中黄 ";
            dic[(Color)ColorConverter.ConvertFromString("#E3C700")] = "蒲公英色 ";
            dic[(Color)ColorConverter.ConvertFromString("#E3C700")] = "黄色 ";
            dic[(Color)ColorConverter.ConvertFromString("#EAD56B")] = "刈安色 ";
            dic[(Color)ColorConverter.ConvertFromString("#716B4A")] = "海松色 ";
            dic[(Color)ColorConverter.ConvertFromString("#6A5F37")] = "鶯茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#EDAE00")] = "鬱金色 ";
            dic[(Color)ColorConverter.ConvertFromString("#FFBB00")] = "向日葵色 ";
            dic[(Color)ColorConverter.ConvertFromString("#F8A900")] = "山吹色 ";
            dic[(Color)ColorConverter.ConvertFromString("#C8A65D")] = "芥子色 ";
            dic[(Color)ColorConverter.ConvertFromString("#B47700")] = "金茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#B8883B")] = "黄土色 ";
            dic[(Color)ColorConverter.ConvertFromString("#C5B69E")] = "砂色 ";
            dic[(Color)ColorConverter.ConvertFromString("#DED2BF")] = "象牙色 ";
            dic[(Color)ColorConverter.ConvertFromString("#EBE7E1")] = "胡粉色 ";
            dic[(Color)ColorConverter.ConvertFromString("#F4BD6B")] = "卵色 ";
            dic[(Color)ColorConverter.ConvertFromString("#EB8400")] = "蜜柑色 ";
            dic[(Color)ColorConverter.ConvertFromString("#6B3E08")] = "褐色 ";
            dic[(Color)ColorConverter.ConvertFromString("#9F6C31")] = "土色 ";
            dic[(Color)ColorConverter.ConvertFromString("#AA7A40")] = "琥珀色 ";
            dic[(Color)ColorConverter.ConvertFromString("#847461")] = "朽葉色 ";
            dic[(Color)ColorConverter.ConvertFromString("#5D5245")] = "煤竹色 ";
            dic[(Color)ColorConverter.ConvertFromString("#D4A168")] = "小麦色 ";
            dic[(Color)ColorConverter.ConvertFromString("#EAE0D5")] = "生成り色 ";
            dic[(Color)ColorConverter.ConvertFromString("#EF810F")] = "橙色 ";
            dic[(Color)ColorConverter.ConvertFromString("#D89F6D")] = "杏色 ";
            dic[(Color)ColorConverter.ConvertFromString("#FAA55C")] = "柑子色 ";
            dic[(Color)ColorConverter.ConvertFromString("#B1632A")] = "黄茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#6D4C33")] = "茶色 ";
            dic[(Color)ColorConverter.ConvertFromString("#F1BB93")] = "肌色 ";
            dic[(Color)ColorConverter.ConvertFromString("#B0764F")] = "駱駝色 ";
            dic[(Color)ColorConverter.ConvertFromString("#816551")] = "灰茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#564539")] = "焦茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#D86011")] = "黄赤 ";
            dic[(Color)ColorConverter.ConvertFromString("#998D86")] = "茶鼠 ";
            dic[(Color)ColorConverter.ConvertFromString("#B26235")] = "代赭 ";
            dic[(Color)ColorConverter.ConvertFromString("#704B38")] = "栗色 ";
            dic[(Color)ColorConverter.ConvertFromString("#3E312B")] = "黒茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#865C4B")] = "桧皮色 ";
            dic[(Color)ColorConverter.ConvertFromString("#B64826")] = "樺色 ";
            dic[(Color)ColorConverter.ConvertFromString("#DB5C35")] = "柿色 ";
            dic[(Color)ColorConverter.ConvertFromString("#EB6940")] = "黄丹 ";
            dic[(Color)ColorConverter.ConvertFromString("#914C35")] = "煉瓦色 ";
            dic[(Color)ColorConverter.ConvertFromString("#B5725C")] = "肉桂色 ";
            dic[(Color)ColorConverter.ConvertFromString("#624035")] = "錆色 ";
            dic[(Color)ColorConverter.ConvertFromString("#E65226")] = "赤橙 ";
            dic[(Color)ColorConverter.ConvertFromString("#8D3927")] = "赤錆色 ";
            dic[(Color)ColorConverter.ConvertFromString("#AD4E39")] = "赤茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#EA4E31")] = "金赤 ";
            dic[(Color)ColorConverter.ConvertFromString("#693C34")] = "海老茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#905D54")] = "小豆色 ";
            dic[(Color)ColorConverter.ConvertFromString("#863E33")] = "弁柄色 ";
            dic[(Color)ColorConverter.ConvertFromString("#6D3A33")] = "紅海老茶 ";
            dic[(Color)ColorConverter.ConvertFromString("#7A453D")] = "鳶色 ";
            dic[(Color)ColorConverter.ConvertFromString("#D1483E")] = "鉛丹色 ";
            dic[(Color)ColorConverter.ConvertFromString("#9E413F")] = "紅樺色 ";
            dic[(Color)ColorConverter.ConvertFromString("#EF4644")] = "紅緋 ";
            dic[(Color)ColorConverter.ConvertFromString("#F0F0F0")] = "白 ";
            dic[(Color)ColorConverter.ConvertFromString("#9C9C9C")] = "銀鼠 ";
            dic[(Color)ColorConverter.ConvertFromString("#838383")] = "鼠色 ";
            dic[(Color)ColorConverter.ConvertFromString("#767676")] = "灰色 ";
            dic[(Color)ColorConverter.ConvertFromString("#343434")] = "墨 ";
            dic[(Color)ColorConverter.ConvertFromString("#2A2A2A")] = "鉄黒 ";
            dic[(Color)ColorConverter.ConvertFromString("#2A2A2A")] = "黒 ";            
        }
        private static readonly Dictionary<Color, string> dic = new Dictionary<Color, string>();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var color = value as Color?;
            if (!color.HasValue)
                return null;

            if (dic.ContainsKey(color.Value))
            {
                return dic[color.Value];
            }
            return (value ?? string.Empty).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
