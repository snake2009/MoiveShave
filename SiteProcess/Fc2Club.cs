using HtmlAgilityPack;
using System.Collections.Generic;

namespace 老司机影片整理
{
    internal class Fc2Club:ISiteProcess
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
            return SearchInfoFromNum(num);
        }

        private MovieInfo SearchInfoFromNum(string num)
        {
            //尝试直接打开番号
            HtmlWeb web = new HtmlWeb();
            var url = $"https://fc2club.com/html/{num.ToUpper()}.html";
            var doc = web.Load(url);
            var notFound = doc.DocumentNode.SelectSingleNode("/html/body/center[1]/h1");
            if (notFound != null)
            {
                //没找到
            }
            else
            {
                var movie = new MovieInfo();
                var body = doc.DocumentNode;
                //封面
                movie.Cover = "";//留空为需要裁剪的
                movie.Backdrop = "https://fc2club.com" + body.SelectSingleNode("/html/body/div[2]/div/div[1]/div[3]/ul[1]/li[1]/img").Attributes["src"].Value;
                //标题
                movie.Title = body.SelectSingleNode("/html/body/div[2]/div/div[1]/h3").InnerText;
                movie.Year = "";
                movie.Length = "";
                movie.Studio = body.SelectSingleNode("/html/body/div[2]/div/div[1]/h5[3]/a[1]").InnerText;
                movie.Series = "";
                movie.Star = new List<string>();
                var star = body.SelectSingleNode("/html/body/div[2]/div/div[1]/h5[5]/a").InnerText;
                movie.Star.Add(string.IsNullOrEmpty(star)?"未知": star);
                var nodes = body.SelectNodes("/html/body/div[2]/div/div[1]/h5[4]/a");
                if (nodes != null)
                {
                    List<string> list = new List<string>();
                    foreach (var sub in nodes)
                    {
                        if (sub.InnerText != "")
                        {
                            list.Add(sub.InnerText);
                        }
                    }
                    movie.Genre = list;
                }
                movie.WebSite = url;
                movie.Generate = "fc2club";
                return movie;
            }
            return null;
        }
    }
}