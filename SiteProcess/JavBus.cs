using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace 老司机影片整理
{
    internal class JavBus: ISiteProcess
    {
        private static readonly Lazy<JavBus> lazy =
        new Lazy<JavBus>(() => new JavBus());

        public static JavBus Instance { get { return lazy.Value; } }

        private JavBus()
        {

        }

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
            //尝试直接打开番号
            HtmlWeb web = new HtmlWeb();
            var detailUrl = $"https://www.dmmbus.cloud/{num}";
            var type = NumberTools.IsUncensored(num);
            string searchUrl = $"https://www.dmmbus.cloud/{(type ? "uncensored/" : "")}search/{num}&type=1";
            var doc = web.Load(detailUrl);
            var notFound = doc.DocumentNode.SelectSingleNode("/html/body/div[4]/div/div/h4");
            //404
            if (notFound != null)
            {
                detailUrl = null;
                //没找到, 搜索一下
                doc = web.Load(searchUrl);
                var nodes = doc.DocumentNode.SelectNodes("//*[@id=\"waterfall\"]/div[@id=\"waterfall\"]/div");
                if (nodes == null)
                {
                    //没搜索到
                    return null;
                }
                foreach (var item in nodes)
                {
                    if (item.SelectSingleNode("a/div[2]/span/date[1]").InnerText.ToLower().Contains(num.ToLower()))
                    {
                        detailUrl = item.SelectSingleNode("a").Attributes["href"].Value;
                        doc = web.Load(detailUrl);
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(detailUrl))
            {
                var movie = new MovieInfo();
                var body = doc.DocumentNode;
                var node = body.SelectSingleNode("/html/body/div[5]/div[1]/div[1]/a/img");
                movie.Backdrop = node.Attributes["src"].Value;
                movie.Title = node.Attributes["title"].Value;
                //获取所有P
                var nodes = body.SelectNodes("/html/body/div[5]/div[1]/div[2]/p");
                if (nodes != null)
                {
                    movie.Year = nodes[1].InnerText.Replace("發行日期:", "").Replace(" ", "");
                    movie.Length = nodes[2].InnerText.Replace("長度:", "").Replace(" ", "");
                    if (nodes[3].InnerText.Contains("導演:"))
                    {
                        movie.Direct = nodes[3].InnerText.Replace("導演:", "").Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                        if (nodes[4].InnerText.Contains("製作商:"))
                        {
                            movie.Studio = nodes[4].InnerText.Replace("製作商:", "").Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                            if (nodes[5].InnerText.Contains("發行商:"))
                            {
                                movie.Publisher = nodes[5].InnerText.Replace("發行商:", "").Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                                if (nodes[6].InnerText.Contains("類別:"))
                                {
                                    List<string> list = new List<string>();
                                    foreach (var sub in nodes[7].ChildNodes)
                                    {
                                        var name = sub.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");
                                        if (name != "")
                                        {
                                            list.Add(name);
                                        }
                                    }
                                    movie.Genre = list;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (nodes[3].InnerText.Contains("製作商:"))
                        {
                            movie.Studio = nodes[3].InnerText.Replace("製作商:", "").Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                            if (nodes[4].InnerText.Contains("發行商:"))
                            {
                                movie.Publisher = nodes[4].InnerText.Replace("發行商:", "").Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                                if (nodes[5].InnerText.Contains("類別:"))
                                {
                                    List<string> list = new List<string>();
                                    foreach (var sub in nodes[5].ChildNodes)
                                    {
                                        var name = sub.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");
                                        if (name != "")
                                        {
                                            list.Add(name);
                                        }
                                    }
                                    movie.Genre = list;
                                }
                            }
                            else
                            {
                                if (!nodes[4].InnerText.Contains("演員:"))
                                {
                                    movie.Series = nodes[4].InnerText.Replace("系列:", "").Replace(" ", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                                    if (nodes[5].InnerText.Contains("類別:"))
                                    {
                                        List<string> list = new List<string>();
                                        foreach (var sub in nodes[6].ChildNodes)
                                        {
                                            var name = sub.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");
                                            if (name != "")
                                            {
                                                list.Add(name);
                                            }
                                        }
                                        movie.Genre = list;
                                    }
                                }
                            }
                        }
                    }
                    if (!nodes[nodes.Count - 1].InnerText.Contains("演員:"))
                    {
                        List<string> list1 = new List<string>();
                        foreach (var sub in nodes[nodes.Count - 1].ChildNodes)
                        {
                            var name = sub.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(" ", "");
                            if (name != "")
                            {
                                list1.Add(name);
                            }
                        }
                        movie.Star = list1;
                    }
                }
                movie.WebSite = detailUrl;
                movie.Generate = "javbus";
                return movie;
            }
            return null;
        }
    }
}