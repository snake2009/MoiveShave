using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace 老司机影片整理
{
    internal class Avmoo : ISiteProcess
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
            var doc = web.Load($"https://avmask.com/cn/search/{num}");
            var notFound = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/h4");
            if (notFound != null)
            {
                //没找到
            }
            else
            {
                var movie = new MovieInfo();
                var body = doc.DocumentNode;
                var nodes = body.SelectNodes("/html/body/div[2]/div/div/div");
                HtmlNode node = null;
                foreach (var item in nodes)
                {
                    if (item.SelectSingleNode("a/div[2]/span/date[1]").InnerText.ToLower() == num.ToLower())
                    {
                        node = item;
                        break;
                    }
                }
                if (node!=null)
                {
                    //详情页链接
                    var url = node.SelectSingleNode("a").Attributes["href"].Value;
                    var img = node.SelectSingleNode("a/div/img");
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
                    nodes = body.SelectNodes("/html/body/div[2]/div[1]/div[2]/p");
                    movie.Year = nodes[1].InnerText.Replace("发行时间:", "").Replace(" ", "");
                    movie.Length = nodes[2].InnerText.Replace("长度:", "").Replace(" ", "");
                    if (nodes[3].InnerText.Contains("导演:"))
                    {
                        movie.Direct = nodes[3].InnerText.Replace("导演:", "").Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                        if (nodes[4].InnerText.Contains("制作商:"))
                        {
                            movie.Studio = nodes[5].InnerText;
                            if (nodes[6].InnerText.Contains("发行商:"))
                            {
                                movie.Publisher = nodes[7].InnerText;
                                if (nodes[8].InnerText.Contains("系列:"))
                                {
                                    movie.Series = nodes[9].InnerText;
                                }
                            }
                        }
                        else
                        {
                            if (nodes[4].InnerText.Contains("发行商:"))
                            {
                                movie.Publisher = nodes[5].InnerText;
                            }
                        }
                    }
                    else
                    {
                        if (nodes[3].InnerText.Contains("制作商:"))
                        {
                            movie.Studio = nodes[4].InnerText;
                            if (nodes[5].InnerText.Contains("发行商:"))
                            {
                                movie.Publisher = nodes[6].InnerText;
                                if (nodes[7].InnerText.Contains("系列:"))
                                {
                                    movie.Series = nodes[8].InnerText;
                                }
                            }
                        }
                        else
                        {
                            if (nodes[3].InnerText.Contains("发行商:"))
                            {
                                movie.Publisher = nodes[4].InnerText;
                            }
                        }
                        //movie.Series = nodes[4].InnerText;
                    }
                    //类别
                    nodes = nodes[nodes.Count - 1].SelectNodes("span/a");
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
                    movie.Generate = "avmoo";
                    return movie;
                }
            }
            return null;
        }
    }
}