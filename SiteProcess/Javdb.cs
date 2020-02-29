using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace 老司机影片整理
{
    internal class Javdb : ISiteProcess
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
            var doc = web.Load($"https://javdb.com/search?q={num}&f=all");
            var movie = new MovieInfo();
            var body = doc.DocumentNode;
            var nodes = body.SelectNodes("/html/body/section/div/div[4]/div/div");
            if (nodes != null)
            {
                HtmlNode node = null;
                foreach (var item in nodes)
                {
                    if (item.SelectSingleNode("a/div[2]").InnerText.ToLower() == num.ToLower())
                    {
                        node = item;
                        break;
                    }
                }
                if (node != null)
                {
                    //详情页链接
                    var url = "https://javdb.com" + node.SelectSingleNode("a").Attributes["href"].Value;
                    //封面 太小 放弃
                    //var img = node.SelectSingleNode("a/div/img");
                    //movie.Cover = img.Attributes["src"].Value;
                    //标题
                    movie.Title = node.SelectSingleNode("a").Attributes["title"].Value;
                    //加载详情页
                    doc = web.Load(url);
                    body = doc.DocumentNode;
                    //背景图
                    movie.Backdrop = body.SelectSingleNode("/html/body/section/div/div[3]/div[1]/a/img").Attributes["src"].Value;
                    //没有图片
                    if (!movie.Backdrop.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return null;
                    }
                    nodes = body.SelectNodes("/html/body/section/div/div[3]/div[2]/nav/div");
                    movie.Year = nodes[1].SelectSingleNode("span[2]").InnerText.Replace(" ", "");
                    movie.Length = nodes[2].SelectSingleNode("span[2]").InnerText.Replace(" ", "");

                    foreach (var item in nodes)
                    {
                        if (item.ChildNodes[1].InnerText.Contains("導演"))
                        {
                            movie.Direct = item.ChildNodes[3].InnerText.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                        }
                        if (item.ChildNodes[1].InnerText.Contains("片商"))
                        {
                            movie.Studio = item.ChildNodes[3].InnerText.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                        }
                        if (item.ChildNodes[1].InnerText.Contains("發行"))
                        {
                            movie.Publisher = item.ChildNodes[3].InnerText.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                        }
                        if (item.ChildNodes[1].InnerText.Contains("系列"))
                        {
                            movie.Series = item.ChildNodes[3].InnerText.Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                        }
                        if (item.ChildNodes[1].InnerText.Contains("类别"))
                        {
                            var genres = item.ChildNodes[3].SelectNodes("a");
                            if (genres != null)
                            {
                                List<string> list = new List<string>();
                                foreach (var sub in genres)
                                {
                                    list.Add(sub.InnerText);
                                }
                                movie.Genre = list;
                            }
                        }
                        if (item.ChildNodes[1].InnerText.Contains("演員"))
                        {
                            var stars = item.ChildNodes[3].SelectNodes("a");
                            if (stars != null)
                            {
                                List<string> list = new List<string>();
                                foreach (var sub in stars)
                                {
                                    list.Add(sub.InnerText);
                                }
                                movie.Star = list;
                            }
                        }
                    }
                    movie.WebSite = url;
                    movie.Generate = "javdb";
                    return movie;
                }
            }
            return null;
        }
    }
}