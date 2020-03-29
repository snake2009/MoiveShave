using System;
using System.IO;
using System.Text.RegularExpressions;

namespace 老司机影片整理
{
    class NumberTools
    {
        /// <summary>
        /// 从文件名获取番号
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>番号</returns>
        internal static string Get(string filepath) 
        {
            var filename = Path.GetFileNameWithoutExtension(filepath);
            if (filename.ToLower().Contains("legsjapan"))
            {
                throw new Exception($"暂时不支持 {filepath}");
            }
            filename = filename.Replace("dioguitar", "").Replace("Carib-", "").Replace("Caribbeancom", "分隔").Replace("[", "分隔").Replace("]", "分隔");
            //优先处理 (FC2)(745325) 这种格式
            var re = Regex.Match(filename, @"\(FC2\)\([0-9]{6,8}\)", RegexOptions.IgnoreCase);
            if (re.Success)
            {
                return re.Groups[0].Value.Replace(")(", "-").Replace("(", "").Replace(")", "");
            }
            //优先处理 FC2PPV-1197848 和 FC2-PPV-1197848 这种格式
            re = Regex.Match(filename, @"(FC2[-]{0,1}PPV[-_]{1})([0-9]{6,8})", RegexOptions.IgnoreCase);
            if (re.Success)
            {
                return "fc2-" + re.Groups[2].Value;
            }
            //优先处理 [ThZu.Cc]heyzo_hd_1939_full 这种格式
            re = Regex.Match(filename, @"(heyzo[-_]{1}hd[-_]{1})([0-9]{4})", RegexOptions.IgnoreCase);
            if (re.Success)
            {
                return "heyzo-" + re.Groups[2].Value;
            }
            //优先处理 1pondo-021619_812 这种格式
            re = Regex.Match(filename, @"1pondo-([0-9]{6}[_-]{1}[0-9]{2,3})");
            if (re.Success)
            {
                return re.Groups[1].Value;
            }
            //优先处理 151225_1015_01 这种格式
            re = Regex.Match(filename, @"([0-9]{6}[_-]{1}[0-9]{3,4}[_-]{1}[0-2]{2})");
            if (re.Success)
            {
                return re.Groups[1].Value;
            }
            //优先处理 091009-YU ロリ物語『第１弾！動かない人形ゴスロリ』这种格式
            re = Regex.Match(filename, @"[0-9]{6}[_-]{1}[a-zA-Z]+");
            if (re.Success)
            {
                return re.Groups[0].Value;
            }
            //正则匹配番号 支持 [CWP-111] [AAA_111] [AAA|111] [AAA 111] [MKBD-S123] 四种格式，AAA支持1~8位长度允许夹杂数字（S2MBD-001），111支持2~8位数字
            var res = Regex.Matches(filename, @"([a-zA-Z0-9]{1,10})[-|_|\s]{0,3}[a-zA-Z]{0,1}([0-9]{2,8})(.*?)");
            if (res[0].Success)
            {
                //180464_3xplanet_Caribbeancom_013120-001
                if (filename.Contains("3xplanet"))
                {
                    return res[1].Groups[0].Value;
                }
                //[456k.me]ofje-046.mp4
                if (res[0].Groups[0].Value.Length <= 5)
                {
                    return res[1].Groups[0].Value;
                }
                return res[0].Groups[0].Value;
            }
            return "";
        }

        /// <summary>
        /// 判断番号是否无码
        /// </summary>
        /// <param name="num">番号</param>
        /// <returns>是否无码</returns>
        internal static bool IsUncensored(string num)
        {
            if (string.IsNullOrEmpty(num))
                return false;
            var name = num.ToLower();
            return name.Contains("sky")
                || name.Contains("red")
                || name.Contains("rhj")
                || name.Contains("sskj")
                || name.Contains("ccdv")
                || name.Contains("sskp")
                || name.Contains("sskx")
                || name.Contains("heyzo")
                || name.Contains("gachi")
                || name.Contains("laf")
                || name.Contains("cwp")
                || name.Contains("mcb")
                || name.Contains("s2m")
                || name.Contains("smd")
                || name.Contains("cwdv")
                || name.Contains("mcdv")
                || name.Contains("mcbd")
                || name.Contains("mcb3dbd")
                || name.Contains("mk3d2dbd")
                || name.Contains("jukujo")
                || name.Contains("drc")
                || name.Contains("candys")
                || name.Contains("dnk")
                || name.Contains("mtb")
                || name.Contains("fc2")
                || name.Contains("hamesamurai")
                || Regex.Match(name, @"^n\d{4}").Success
                || Regex.Match(name, @"bd[-_]{0,1}m\d{2}").Success
                || Regex.Match(name, @"xv\d{2}").Success
                || Regex.Match(name, @"mx[-_]{1}m\d{2}").Success
                || Regex.Match(name, @"(bt|ct|pt|fh|kg|hey|trg|trp|tw)[-_]{1}\d{2,4}").Success
                || Regex.Match(name, @"gachi[p]{0,1}[-_]\d{2,4}").Success // gachi-050 gachip-137
                || Regex.Match(name, @"\d{6}[-_]\d{2,3}").Success //030312_01 021816-099
                || Regex.Match(name, @"\d{6}[-_]\d{3}[-_]\d{2}").Success //021816-099
                || Regex.Match(name, @"\d{6}[-_][a-z_A-Z]+").Success //120316-SAKI_NOZOMI
                || name.Contains("dsam");
        }

    }
}
