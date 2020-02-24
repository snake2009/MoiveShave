using HtmlAgilityPack;
using System.Collections.Generic;

namespace 老司机影片整理
{
    internal class JavLibrary : ISiteProcess
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="num">番号</param>
        /// <param name="cfg">配置</param>
        /// <returns></returns>
        public MovieInfo GetInfoAsync(string num)
        {
            return SearchInfoFromNumAsync(num);
        }

        private MovieInfo SearchInfoFromNumAsync(string num)
        {
            //尝试直接打开番号
            HtmlWeb web = new HtmlWeb();
            var url = $"http://www.javlibrary.com/cn/vl_searchbyid.php?keyword={num}";
            var doc = web.Load(url);
            var notFound = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/p/em");
            if (notFound != null)
            {
                //没找到
            }
            else
            {
                var movie = new MovieInfo();
                var body = doc.DocumentNode;
                movie.Number = body.SelectSingleNode("//*[@id=\"video_id\"]/table/tbody/tr/td[2]").InnerText;
                movie.Backdrop = body.SelectSingleNode("//*[@id=\"video_jacket_img\"]").Attributes["src"].Value;
                movie.Title = body.SelectSingleNode("/html/body/div[3]/div[2]/div[1]/h3").InnerText.Replace($"{movie.Number} ", "");
                movie.Year = body.SelectSingleNode("//*[@id=\"video_date\"]/table/tbody/tr/td[2]").InnerText;
                movie.Length = body.SelectSingleNode("//*[@id=\"video_length\"]/table/tbody/tr/td[2]").InnerText;
                movie.Direct = body.SelectSingleNode("//*[@id=\"video_director\"]/table/tbody/tr/td[2]/span/a").InnerText;
                movie.Studio = body.SelectSingleNode("//*[@id=\"video_maker\"]/table/tbody/tr/td[2]/span/a").InnerText;
                movie.Publisher = body.SelectSingleNode("//*[@id=\"video_label\"]/table/tbody/tr/td[2]/span/a").InnerText;
                var nodes = body.SelectNodes("//*[@id=\"video_genres\"]/table/tbody/tr/td[2]/span");
                List<string> list = new List<string>();
                foreach (var sub in nodes)
                {
                    var name = sub.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");
                    if (name != "")
                    {
                        list.Add(name);
                    }
                }
                movie.Genre = list;

                nodes = body.SelectNodes("//*[@id=\"video_genres\"]/table/tbody/tr/td[2]/span");
                list = new List<string>();
                foreach (var sub in nodes)
                {
                    var name = sub.SelectSingleNode("span/a").InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");
                    if (name != "")
                    {
                        list.Add(name);
                    }
                }
                movie.Star = list;
                movie.WebSite = url;
                movie.Generate = "javlibrary";
                return movie;
            }
            return null;
        }
    }
}