using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace 老司机影片整理
{
    internal class Avsox : ISiteProcess
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

        /// <summary>
        /// 搜索资料
        /// </summary>
        /// <param name="num">番号</param>
        /// <returns></returns>
        private MovieInfo SearchInfoFromNum(string num)
        {
            HtmlWeb web = new HtmlWeb();
            //搜索番号
            var doc = web.Load($"https://avsox.host/cn/search/{num}");
            var notFound = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/h4");
            if (notFound != null)
            {
                //没找到
            }
            else
            {
                var movie = new MovieInfo();
                var body = doc.DocumentNode;
                var node = body.SelectSingleNode("/html/body/div[2]/div/div/div/a");
                if (node != null)
                {
                    //详情页链接
                    var url = node.Attributes["href"].Value;
                    var img = node.SelectSingleNode("div[1]/img");
                    //封面
                    movie.Cover = img.Attributes["src"].Value;
                    //标题
                    movie.Title = img.Attributes["title"].Value;
                    //加载详情页
                    doc = web.Load(url);
                    body = doc.DocumentNode;
                    //背景图
                    movie.Backdrop = body.SelectSingleNode("/html/body/div[2]/div[1]/div[1]/a/img").Attributes["src"].Value;
                    //没有图片
                    if (!movie.Backdrop.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return null;
                    }
                    movie.Year = body.SelectSingleNode("/html/body/div[2]/div[1]/div[2]/p[2]").InnerText.Replace("发行时间:", "");
                    movie.Length = body.SelectSingleNode("/html/body/div[2]/div[1]/div[2]/p[3]").InnerText.Replace("长度:", "");
                    movie.Studio = body.SelectSingleNode("/html/body/div[2]/div[1]/div[2]/p[5]").InnerText;
                    movie.Series = body.SelectSingleNode("/html/body/div[2]/div[1]/div[2]/p[7]").InnerText;
                    //类别
                    var nodes = body.SelectNodes("/html/body/div[2]/div[1]/div[2]/p[9]/span");
                    if (nodes != null)
                    {
                        List<string> list = new List<string>();
                        foreach (var sub in nodes)
                        {
                            list.Add(sub.InnerText);
                        }
                        movie.Genre = list;
                    }
                    nodes = body.SelectNodes("//*[@id=\"avatar-waterfall\"]/a");
                    if (nodes != null)
                    {
                        List<string> list = new List<string>();
                        foreach (var sub in nodes)
                        {
                            list.Add(sub.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", ""));
                        }
                        movie.Star = list;
                    }
                    movie.WebSite = url;
                    movie.Generate = "avsox";
                    return movie;
                }
            }
            return null;
        }
    }
}