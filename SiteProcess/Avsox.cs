using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace 老司机影片整理
{
    internal class Avsox : ISiteProcess
    {
        private HttpClient client;
        private static readonly Lazy<Avsox> lazy =
        new Lazy<Avsox>(() => new Avsox());

        public static Avsox Instance { get { return lazy.Value; } }

        private Avsox()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.122 Safari/537.36 Edg/80.0.361.62");
            client.DefaultRequestHeaders.Referrer = new Uri("https://avsox.host/cn");
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
            //HtmlWeb web = new HtmlWeb();
            //搜索番号
            //var doc = web.Load($"https://avsox.host/cn/search/{num}");
            var searchUrl = $"https://avsox.host/cn/search/{num}";
            var html = client.GetStringAsync(searchUrl).Result;
            if (string.IsNullOrEmpty(html))
            {
                //没抓取到html
                return null;
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var notFound = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div[1]/h4");
            if (notFound != null)
            {
                //页面放回404节点
                return null;
            }
            var movie = new MovieInfo();
            var body = doc.DocumentNode;
            var node = body.SelectSingleNode("/html/body/div[2]/div/div/div/a");
            if (node == null)
            {
                //没有找到详情链接
                return null;
            }
            //详情页链接
            var img = node.SelectSingleNode("div[1]/img");
            //封面
            movie.Cover = img.Attributes["src"].Value;
            //标题
            movie.Title = img.Attributes["title"].Value;
            //加载详情页
            var detailUrl = node.Attributes["href"].Value;
            client.DefaultRequestHeaders.Referrer = new Uri(searchUrl);
            html = client.GetStringAsync(detailUrl).Result;
            doc.LoadHtml(html);
            body = doc.DocumentNode;
            //背景图
            var imgNode = body.SelectSingleNode("/html/body/div[2]/div[1]/div[1]/a/img");
            if (imgNode == null)
            {
                return null;
            }
            movie.Backdrop = imgNode.Attributes["src"].Value;
            //没有图片
            if (!movie.Backdrop.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }
            var nodes = body.SelectNodes("/html/body/div[2]/div[1]/div[2]/p");
            if (nodes == null)
            {
                return null;
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                var text = nodes[i].InnerText;
                if (text.Contains("发行时间"))
                {
                    movie.Year = text.Replace("发行时间:", "").Replace(" ", "");
                }
                else if (text.Contains("长度"))
                {
                    movie.Length = text.Replace("长度:", "").Replace(" ", "");
                }
                else if (text.Contains("制作商"))
                {
                    movie.Studio = nodes[i + 1].InnerText;
                }
                else if (text.Contains("系列:"))
                {
                    movie.Series = nodes[i + 1].InnerText;
                }
                else if (text.Contains("类别:"))
                {
                    var genres = nodes[i + 1].SelectNodes("span");
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
            movie.WebSite = detailUrl;
            movie.Generate = "avsox";
            return movie;
        }
    }
}