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
    internal class ColorToStringConverter : IValueConverter
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
            //定義済みカラーより和色を気に入りました。
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
            dic[(Color)ColorConverter.ConvertFromString("#EF454A")] = "朱色 (しゅいろ)\n#EF454A";
            dic[(Color)ColorConverter.ConvertFromString("#94474B")] = "蘇芳 (すおう)\n#94474B";
            dic[(Color)ColorConverter.ConvertFromString("#E38089")] = "桃色 (ももいろ)\n#E38089";
            dic[(Color)ColorConverter.ConvertFromString("#DF828A")] = "紅梅色 (こうばいいろ)\n#DF828A";
            dic[(Color)ColorConverter.ConvertFromString("#AD3140")] = "臙脂 (えんじ)\n#AD3140";
            dic[(Color)ColorConverter.ConvertFromString("#FF7F8F")] = "珊瑚色 (さんごいろ)\n#FF7F8F";
            dic[(Color)ColorConverter.ConvertFromString("#FBDADE")] = "桜色 (さくらいろ)\n#FBDADE";
            dic[(Color)ColorConverter.ConvertFromString("#9E2236")] = "茜色 (あかねいろ)\n#9E2236";
            dic[(Color)ColorConverter.ConvertFromString("#E64B6B")] = "韓紅 (からくれない)\n#E64B6B";
            dic[(Color)ColorConverter.ConvertFromString("#B81A3E")] = "紅赤 (べにあか)\n#B81A3E";
            dic[(Color)ColorConverter.ConvertFromString("#D53E62")] = "薔薇色 (ばらいろ)\n#D53E62";
            dic[(Color)ColorConverter.ConvertFromString("#BE0032")] = "赤 (あか)\n#BE0032";
            dic[(Color)ColorConverter.ConvertFromString("#FA9CB8")] = "鴇色 (ときいろ)\n#FA9CB8";
            dic[(Color)ColorConverter.ConvertFromString("#BE003F")] = "紅色 (べにいろ)\n#BE003F";
            dic[(Color)ColorConverter.ConvertFromString("#CF4078")] = "躑躅色 (つつじいろ)\n#CF4078";
            dic[(Color)ColorConverter.ConvertFromString("#DA508F")] = "赤紫 (あかむらさき)\n#DA508F";
            dic[(Color)ColorConverter.ConvertFromString("#C94093")] = "牡丹色 (ぼたんいろ)\n#C94093";
            dic[(Color)ColorConverter.ConvertFromString("#C573B2")] = "菖蒲色 (あやめいろ)\n#C573B2";
            dic[(Color)ColorConverter.ConvertFromString("#473946")] = "茄子紺 (なすこん)\n#473946";
            dic[(Color)ColorConverter.ConvertFromString("#422C41")] = "紫紺 (しこん)\n#422C41";
            dic[(Color)ColorConverter.ConvertFromString("#765276")] = "古代紫 (こだいむらさき)\n#765276";
            dic[(Color)ColorConverter.ConvertFromString("#A757A8")] = "紫 (むらさき)\n#A757A8";
            dic[(Color)ColorConverter.ConvertFromString("#614876")] = "江戸紫 (えどむらさき)\n#614876";
            dic[(Color)ColorConverter.ConvertFromString("#665971")] = "鳩羽色 (はとばいろ)\n#665971";
            dic[(Color)ColorConverter.ConvertFromString("#744B98")] = "菖蒲色 (しょうぶいろ)\n#744B98";
            dic[(Color)ColorConverter.ConvertFromString("#714C99")] = "菫色 (すみれいろ)\n#714C99";
            dic[(Color)ColorConverter.ConvertFromString("#7445AA")] = "青紫 (あおむらさき)\n#7445AA";
            dic[(Color)ColorConverter.ConvertFromString("#9883C9")] = "藤紫 (ふじむらさき)\n#9883C9";
            dic[(Color)ColorConverter.ConvertFromString("#A294C8")] = "藤色 (ふじいろ)\n#A294C8";
            dic[(Color)ColorConverter.ConvertFromString("#69639A")] = "藤納戸 (ふじなんど)\n#69639A";
            dic[(Color)ColorConverter.ConvertFromString("#353573")] = "紺藍 (こんあい)\n#353573";
            dic[(Color)ColorConverter.ConvertFromString("#292934")] = "鉄紺 (てつこん)\n#292934";
            dic[(Color)ColorConverter.ConvertFromString("#4347A2")] = "桔梗色 (ききょういろ)\n#4347A2";
            dic[(Color)ColorConverter.ConvertFromString("#3A3C4F")] = "勝色 (かちいろ)\n#3A3C4F";
            dic[(Color)ColorConverter.ConvertFromString("#384D98")] = "群青色 (ぐんじょういろ)\n#384D98";
            dic[(Color)ColorConverter.ConvertFromString("#435AA0")] = "杜若色 (かきつばたいろ)\n#435AA0";
            dic[(Color)ColorConverter.ConvertFromString("#343D55")] = "紺色 (こんいろ)\n#343D55";
            dic[(Color)ColorConverter.ConvertFromString("#3A4861")] = "紺青 (こんじょう)\n#3A4861";
            dic[(Color)ColorConverter.ConvertFromString("#27477A")] = "瑠璃紺 (るりこん)\n#27477A";
            dic[(Color)ColorConverter.ConvertFromString("#89ACD7")] = "勿忘草色 (わすれなぐさいろ)\n#89ACD7";
            dic[(Color)ColorConverter.ConvertFromString("#72777D")] = "鉛色 (なまりいろ)\n#72777D";
            dic[(Color)ColorConverter.ConvertFromString("#00519A")] = "瑠璃色 (るりいろ)\n#00519A";
            dic[(Color)ColorConverter.ConvertFromString("#223546")] = "濃藍 (こいあい)\n#223546";
            dic[(Color)ColorConverter.ConvertFromString("#2B618F")] = "縹色 (はなだいろ)\n#2B618F";
            dic[(Color)ColorConverter.ConvertFromString("#2B4B65")] = "藍色 (あいいろ)\n#2B4B65";
            dic[(Color)ColorConverter.ConvertFromString("#006AB6")] = "青 (あお)\n#006AB6";
            dic[(Color)ColorConverter.ConvertFromString("#89BDDE")] = "空色 (そらいろ)\n#89BDDE";
            dic[(Color)ColorConverter.ConvertFromString("#007BC3")] = "露草色 (つゆくさいろ)\n#007BC3";
            dic[(Color)ColorConverter.ConvertFromString("#576D79")] = "藍鼠 (あいねず)\n#576D79";
            dic[(Color)ColorConverter.ConvertFromString("#9DCCE0")] = "水色 (みずいろ)\n#9DCCE0";
            dic[(Color)ColorConverter.ConvertFromString("#7EB1C1")] = "甕覗き (かめのぞき)\n#7EB1C1";
            dic[(Color)ColorConverter.ConvertFromString("#73B3C1")] = "白群 (びゃくぐん)\n#73B3C1";
            dic[(Color)ColorConverter.ConvertFromString("#00687C")] = "納戸色 (なんどいろ)\n#00687C";
            dic[(Color)ColorConverter.ConvertFromString("#00859B")] = "浅葱色 (あさぎいろ)\n#00859B";
            dic[(Color)ColorConverter.ConvertFromString("#53A8B7")] = "新橋色 (しんばしいろ)\n#53A8B7";
            dic[(Color)ColorConverter.ConvertFromString("#6D969C")] = "水浅葱 (みずあさぎ)\n#6D969C";
            dic[(Color)ColorConverter.ConvertFromString("#608A8E")] = "錆浅葱 (さびあさぎ)\n#608A8E";
            dic[(Color)ColorConverter.ConvertFromString("#008E94")] = "青緑 (あおみどり)\n#008E94";
            dic[(Color)ColorConverter.ConvertFromString("#24433E")] = "鉄色 (てついろ)\n#24433E";
            dic[(Color)ColorConverter.ConvertFromString("#6AA89D")] = "青竹色 (あおたけいろ)\n#6AA89D";
            dic[(Color)ColorConverter.ConvertFromString("#00A37E")] = "若竹色 (わかたけいろ)\n#00A37E";
            dic[(Color)ColorConverter.ConvertFromString("#00533E")] = "萌葱色 (もえぎいろ)\n#00533E";
            dic[(Color)ColorConverter.ConvertFromString("#6DA895")] = "青磁色 (せいじいろ)\n#6DA895";
            dic[(Color)ColorConverter.ConvertFromString("#007B50")] = "常磐色 (ときわいろ)\n#007B50";
            dic[(Color)ColorConverter.ConvertFromString("#005638")] = "深緑 (ふかみどり)\n#005638";
            dic[(Color)ColorConverter.ConvertFromString("#00B66E")] = "緑 (みどり)\n#00B66E";
            dic[(Color)ColorConverter.ConvertFromString("#3C6754")] = "千歳緑 (ちとせみどり)\n#3C6754";
            dic[(Color)ColorConverter.ConvertFromString("#4D8169")] = "緑青色 (ろくしょういろ)\n#4D8169";
            dic[(Color)ColorConverter.ConvertFromString("#BADBC7")] = "白緑 (びゃくろく)\n#BADBC7";
            dic[(Color)ColorConverter.ConvertFromString("#6E7972")] = "利休鼠 (りきゅうねずみ)\n#6E7972";
            dic[(Color)ColorConverter.ConvertFromString("#687E52")] = "松葉色 (まつばいろ)\n#687E52";
            dic[(Color)ColorConverter.ConvertFromString("#A9C087")] = "若葉色 (わかばいろ)\n#A9C087";
            dic[(Color)ColorConverter.ConvertFromString("#737C3E")] = "草色 (くさいろ)\n#737C3E";
            dic[(Color)ColorConverter.ConvertFromString("#97A61E")] = "萌黄 (もえぎ)\n#97A61E";
            dic[(Color)ColorConverter.ConvertFromString("#AAB300")] = "若草色 (わかくさいろ)\n#AAB300";
            dic[(Color)ColorConverter.ConvertFromString("#BBC000")] = "黄緑 (きみどり)\n#BBC000";
            dic[(Color)ColorConverter.ConvertFromString("#7C7A37")] = "苔色 (こけいろ)\n#7C7A37";
            dic[(Color)ColorConverter.ConvertFromString("#C2BD3D")] = "鶸色 (ひわいろ)\n#C2BD3D";
            dic[(Color)ColorConverter.ConvertFromString("#706C3E")] = "鶯色 (うぐいすいろ)\n#706C3E";
            dic[(Color)ColorConverter.ConvertFromString("#D6C949")] = "黄檗色 (きはだいろ)\n#D6C949";
            dic[(Color)ColorConverter.ConvertFromString("#C0BA7F")] = "抹茶色 (まっちゃいろ)\n#C0BA7F";
            dic[(Color)ColorConverter.ConvertFromString("#EDD60E")] = "中黄 (ちゅうき)\n#EDD60E";
            dic[(Color)ColorConverter.ConvertFromString("#E3C700")] = "蒲公英色 (たんぽぽいろ)\n#E3C700";
            dic[(Color)ColorConverter.ConvertFromString("#E3C700")] = "黄色 (きいろ)\n#E3C700";
            dic[(Color)ColorConverter.ConvertFromString("#EAD56B")] = "刈安色 (かりやすいろ)\n#EAD56B";
            dic[(Color)ColorConverter.ConvertFromString("#716B4A")] = "海松色 (みるいろ)\n#716B4A";
            dic[(Color)ColorConverter.ConvertFromString("#6A5F37")] = "鶯茶 (うぐいすちゃ)\n#6A5F37";
            dic[(Color)ColorConverter.ConvertFromString("#EDAE00")] = "鬱金色 (うこんいろ)\n#EDAE00";
            dic[(Color)ColorConverter.ConvertFromString("#FFBB00")] = "向日葵色 (ひまわりいろ)\n#FFBB00";
            dic[(Color)ColorConverter.ConvertFromString("#F8A900")] = "山吹色 (やまぶきいろ)\n#F8A900";
            dic[(Color)ColorConverter.ConvertFromString("#C8A65D")] = "芥子色 (からしいろ)\n#C8A65D";
            dic[(Color)ColorConverter.ConvertFromString("#B47700")] = "金茶 (きんちゃ)\n#B47700";
            dic[(Color)ColorConverter.ConvertFromString("#B8883B")] = "黄土色 (おうどいろ)\n#B8883B";
            dic[(Color)ColorConverter.ConvertFromString("#C5B69E")] = "砂色 (すないろ)\n#C5B69E";
            dic[(Color)ColorConverter.ConvertFromString("#DED2BF")] = "象牙色 (ぞうげいろ)\n#DED2BF";
            dic[(Color)ColorConverter.ConvertFromString("#EBE7E1")] = "胡粉色 (ごふんいろ)\n#EBE7E1";
            dic[(Color)ColorConverter.ConvertFromString("#F4BD6B")] = "卵色 (たまごいろ)\n#F4BD6B";
            dic[(Color)ColorConverter.ConvertFromString("#EB8400")] = "蜜柑色 (みかんいろ)\n#EB8400";
            dic[(Color)ColorConverter.ConvertFromString("#6B3E08")] = "褐色 (かっしょく)\n#6B3E08";
            dic[(Color)ColorConverter.ConvertFromString("#9F6C31")] = "土色 (つちいろ)\n#9F6C31";
            dic[(Color)ColorConverter.ConvertFromString("#AA7A40")] = "琥珀色 (こはくいろ)\n#AA7A40";
            dic[(Color)ColorConverter.ConvertFromString("#847461")] = "朽葉色 (くちばいろ)\n#847461";
            dic[(Color)ColorConverter.ConvertFromString("#5D5245")] = "煤竹色 (すすたけいろ)\n#5D5245";
            dic[(Color)ColorConverter.ConvertFromString("#D4A168")] = "小麦色 (こむぎいろ)\n#D4A168";
            dic[(Color)ColorConverter.ConvertFromString("#EAE0D5")] = "生成り色 (きなりいろ)\n#EAE0D5";
            dic[(Color)ColorConverter.ConvertFromString("#EF810F")] = "橙色 (だいだいいろ)\n#EF810F";
            dic[(Color)ColorConverter.ConvertFromString("#D89F6D")] = "杏色 (あんずいろ)\n#D89F6D";
            dic[(Color)ColorConverter.ConvertFromString("#FAA55C")] = "柑子色 (こうじいろ)\n#FAA55C";
            dic[(Color)ColorConverter.ConvertFromString("#B1632A")] = "黄茶 (きちゃ)\n#B1632A";
            dic[(Color)ColorConverter.ConvertFromString("#6D4C33")] = "茶色 (ちゃいろ)\n#6D4C33";
            dic[(Color)ColorConverter.ConvertFromString("#F1BB93")] = "肌色 (はだいろ)\n#F1BB93";
            dic[(Color)ColorConverter.ConvertFromString("#B0764F")] = "駱駝色 (らくだいろ)\n#B0764F";
            dic[(Color)ColorConverter.ConvertFromString("#816551")] = "灰茶 (はいちゃ)\n#816551";
            dic[(Color)ColorConverter.ConvertFromString("#564539")] = "焦茶 (こげちゃ)\n#564539";
            dic[(Color)ColorConverter.ConvertFromString("#D86011")] = "黄赤 (きあか)\n#D86011";
            dic[(Color)ColorConverter.ConvertFromString("#998D86")] = "茶鼠 (ちゃねずみ)\n#998D86";
            dic[(Color)ColorConverter.ConvertFromString("#B26235")] = "代赭 (たいしゃ)\n#B26235";
            dic[(Color)ColorConverter.ConvertFromString("#704B38")] = "栗色 (くりいろ)\n#704B38";
            dic[(Color)ColorConverter.ConvertFromString("#3E312B")] = "黒茶 (くろちゃ)\n#3E312B";
            dic[(Color)ColorConverter.ConvertFromString("#865C4B")] = "桧皮色 (ひわだいろ)\n#865C4B";
            dic[(Color)ColorConverter.ConvertFromString("#B64826")] = "樺色 (かばいろ)\n#B64826";
            dic[(Color)ColorConverter.ConvertFromString("#DB5C35")] = "柿色 (かきいろ)\n#DB5C35";
            dic[(Color)ColorConverter.ConvertFromString("#EB6940")] = "黄丹 (おうに)\n#EB6940";
            dic[(Color)ColorConverter.ConvertFromString("#914C35")] = "煉瓦色 (れんがいろ)\n#914C35";
            dic[(Color)ColorConverter.ConvertFromString("#B5725C")] = "肉桂色 (にっけいいろ)\n#B5725C";
            dic[(Color)ColorConverter.ConvertFromString("#624035")] = "錆色 (さびいろ)\n#624035";
            dic[(Color)ColorConverter.ConvertFromString("#E65226")] = "赤橙 (あかだいだい)\n#E65226";
            dic[(Color)ColorConverter.ConvertFromString("#8D3927")] = "赤錆色 (あかさびいろ)\n#8D3927";
            dic[(Color)ColorConverter.ConvertFromString("#AD4E39")] = "赤茶 (あかちゃ)\n#AD4E39";
            dic[(Color)ColorConverter.ConvertFromString("#EA4E31")] = "金赤 (きんあか)\n#EA4E31";
            dic[(Color)ColorConverter.ConvertFromString("#693C34")] = "海老茶 (えびちゃ)\n#693C34";
            dic[(Color)ColorConverter.ConvertFromString("#905D54")] = "小豆色 (あずきいろ)\n#905D54";
            dic[(Color)ColorConverter.ConvertFromString("#863E33")] = "弁柄色 (べんがらいろ)\n#863E33";
            dic[(Color)ColorConverter.ConvertFromString("#6D3A33")] = "紅海老茶 (べにえびちゃ)\n#6D3A33";
            dic[(Color)ColorConverter.ConvertFromString("#7A453D")] = "鳶色 (とびいろ)\n#7A453D";
            dic[(Color)ColorConverter.ConvertFromString("#D1483E")] = "鉛丹色 (えんたんいろ)\n#D1483E";
            dic[(Color)ColorConverter.ConvertFromString("#9E413F")] = "紅樺色 (べにかばいろ)\n#9E413F";
            dic[(Color)ColorConverter.ConvertFromString("#EF4644")] = "紅緋 (べにひ)\n#EF4644";
            dic[(Color)ColorConverter.ConvertFromString("#F0F0F0")] = "白 (しろ)\n#F0F0F0";
            dic[(Color)ColorConverter.ConvertFromString("#9C9C9C")] = "銀鼠 (ぎんねず)\n#9C9C9C";
            dic[(Color)ColorConverter.ConvertFromString("#838383")] = "鼠色 (ねずみいろ)\n#838383";
            dic[(Color)ColorConverter.ConvertFromString("#767676")] = "灰色 (はいいろ)\n#767676";
            dic[(Color)ColorConverter.ConvertFromString("#343434")] = "墨 (すみ)\n#343434";
            //dic[(Color)ColorConverter.ConvertFromString("#2A2A2A")] = "鉄黒 (てつぐろ)\n#2A2A2A";
            dic[(Color)ColorConverter.ConvertFromString("#2A2A2A")] = "黒 (くろ)\n#2A2A2A";
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
            return color.Value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
