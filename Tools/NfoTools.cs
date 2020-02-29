using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace 老司机影片整理
{
    internal class NfoTools
    {
        internal static void Create(string path, MovieInfo data, Config config)
        {
            if (!config.CreateNfo)
            {
                //不创建就返回
                return;
            }
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
                doc.AppendChild(dec);

                XmlElement root = doc.CreateElement("movie");
                doc.AppendChild(root);

                XmlElement title = doc.CreateElement("title");
                title.InnerText = config.TitleTemplate.Replace("$番号", data.Number).Replace("$标题", data.Title).Replace("$年代", data.Year).Replace("$女优", data.Star == null?"": data.Star[0]);
                root.AppendChild(title);

                XmlElement year = doc.CreateElement("year");
                year.InnerText = string.IsNullOrEmpty(data.Year) ? "未知" : data.Year.Substring(0, 4);
                root.AppendChild(year);

                XmlElement poster = doc.CreateElement("poster");
                poster.InnerText = "Cover.png";
                root.AppendChild(poster);

                XmlElement thumb = doc.CreateElement("thumb");
                thumb.InnerText = "Cover.png";
                root.AppendChild(thumb);

                XmlElement fanart = doc.CreateElement("fanart");
                fanart.InnerText = "Backdrop.jpg";
                root.AppendChild(fanart);

                XmlElement premiered = doc.CreateElement("premiered");
                premiered.InnerText = data.Year;
                root.AppendChild(premiered);

                if (!string.IsNullOrEmpty(data.Length))
                {
                    XmlElement runtime = doc.CreateElement("runtime");
                    runtime.InnerText = data.Length.Replace("分钟", "").Replace("分鐘", "");
                    root.AppendChild(runtime);
                }
                if (data.Star == null)
                {
                    XmlElement actor = doc.CreateElement("actor");
                    root.AppendChild(actor);

                    XmlElement name = doc.CreateElement("name");
                    name.InnerText = "未知";
                    actor.AppendChild(name);

                    XmlElement type = doc.CreateElement("type");
                    type.InnerText = "Actor";
                    actor.AppendChild(type);
                }
                else
                {
                    foreach (var item in data.Star)
                    {
                        XmlElement actor = doc.CreateElement("actor");
                        root.AppendChild(actor);

                        XmlElement name = doc.CreateElement("name");
                        name.InnerText = item;
                        actor.AppendChild(name);

                        XmlElement type = doc.CreateElement("type");
                        type.InnerText = "Actor";
                        actor.AppendChild(type);
                    }
                }

                if (data.Genre != null)
                {
                    foreach (var item in data.Genre)
                    {
                        XmlElement genre = doc.CreateElement("genre");
                        genre.InnerText = item;
                        root.AppendChild(genre);

                        XmlElement tag = doc.CreateElement("tag");
                        tag.InnerText = item;
                        root.AppendChild(tag);
                    }
                }

                if (!string.IsNullOrEmpty(data.Series))
                {
                    XmlElement genre = doc.CreateElement("genre");
                    genre.InnerText = $"系列:{data.Series}";
                    root.AppendChild(genre);

                    XmlElement tag = doc.CreateElement("tag");
                    tag.InnerText = $"系列:{data.Series}";
                    root.AppendChild(tag);
                }

                if (!string.IsNullOrEmpty(data.Direct))
                {
                    XmlElement genre = doc.CreateElement("genre");
                    genre.InnerText = $"导演:{data.Direct}";
                    root.AppendChild(genre);

                    XmlElement tag = doc.CreateElement("tag");
                    tag.InnerText = $"导演:{data.Direct}";
                    root.AppendChild(tag);
                }

                if (!string.IsNullOrEmpty(data.Publisher))
                {
                    XmlElement genre = doc.CreateElement("genre");
                    genre.InnerText = $"发行商:{data.Publisher}";
                    root.AppendChild(genre);

                    XmlElement tag = doc.CreateElement("tag");
                    tag.InnerText = $"发行商:{data.Publisher}";
                    root.AppendChild(tag);
                }

                if (!string.IsNullOrEmpty(data.Studio))
                {
                    XmlElement studio = doc.CreateElement("studio");
                    studio.InnerText = data.Studio;
                    root.AppendChild(studio);

                    XmlElement maker = doc.CreateElement("maker");
                    maker.InnerText = data.Studio;
                    root.AppendChild(maker);

                    XmlElement genre = doc.CreateElement("genre");
                    genre.InnerText = $"制作商:{data.Studio}";
                    root.AppendChild(genre);

                    XmlElement tag = doc.CreateElement("tag");
                    tag.InnerText = $"制作商:{data.Studio}";
                    root.AppendChild(tag);
                }

                XmlElement num = doc.CreateElement("num");
                num.InnerText = data.Number;
                root.AppendChild(num);

                XmlElement cover = doc.CreateElement("cover");
                cover.InnerText = data.Backdrop;
                root.AppendChild(cover);

                XmlElement website = doc.CreateElement("website");
                website.InnerText = data.WebSite;
                root.AppendChild(website);

                doc.Save(path);
            }
            catch (Exception e)
            {
                Log.Save($"生成nfo出错 {e.Message}");
                throw new Exception($"生成nfo出错 {e.Message}");
            }
        }

        /// <summary>
        /// 加载视频同名nfo文件
        /// </summary>
        /// <param name="videoPath">视频路径</param>
        /// <returns>影片信息</returns>
        internal static MovieInfo Load(string videoPath)
        {
            try
            {
                var filename = Path.GetFileNameWithoutExtension(videoPath);
                var dirName = Path.GetDirectoryName(videoPath);
                var nfoPath = $"{dirName}{Path.DirectorySeparatorChar}{filename.Replace("-cd1", "").Replace("-cd2", "").Replace("-cd3", "").Replace("-cd4", "").Replace("-cd5", "")}.nfo";
                var coverPath = $"{dirName}{Path.DirectorySeparatorChar}Cover.png";
                var backdropPath = $"{dirName}{Path.DirectorySeparatorChar}Backdrop.jpg";
                var doc = new XmlDocument();
                doc.Load(nfoPath);
                var title = doc.GetElementsByTagName("title")[0].InnerText;
                var num = doc.GetElementsByTagName("num")[0].InnerText;
                var genres = doc.GetElementsByTagName("genre");
                var actors = doc.GetElementsByTagName("actor");
                var node = doc.GetElementsByTagName("premiered");
                var premiered = node == null|| node.Count == 0? "": node[0].InnerText;
                var website = doc.GetElementsByTagName("website")[0].InnerText;
                var attr = Regex.Match(filename, @"-cd(\d{1})").Groups[1].Value;
                var movieInfo = new MovieInfo()
                {
                    Number = num,
                    Title = title,
                    Cover = coverPath,
                    Year = premiered,
                    WebSite = website,
                    Backdrop = backdropPath,
                    Attr = attr
                };

                var list = new List<string>();
                foreach (XmlNode genre in genres)
                {
                    if (genre.InnerText.Contains("发行商"))
                    {
                        movieInfo.Publisher = genre.InnerText.Replace("发行商:", "");
                    }
                    else if (genre.InnerText.Contains("制作商"))
                    {
                        movieInfo.Studio = genre.InnerText.Replace("制作商:", "");
                    }
                    else if (genre.InnerText.Contains("导演"))
                    {
                        movieInfo.Direct = genre.InnerText.Replace("导演:", "");
                    }
                    else if (genre.InnerText.Contains("系列"))
                    {
                        movieInfo.Series = genre.InnerText.Replace("系列:", "");
                    }
                    else
                    {
                        list.Add(genre.InnerText);
                    }
                }
                movieInfo.Genre = list;
                list = new List<string>();
                foreach (XmlNode actor in actors)
                {
                    list.Add(actor.ChildNodes[0].InnerText);
                }
                movieInfo.Star = list;
                return movieInfo;
            }
            catch (Exception e)
            {
                throw new Exception($"加载nfo出错{e.Message}");
            }
        }
    }
}