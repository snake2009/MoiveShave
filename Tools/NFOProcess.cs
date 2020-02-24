using System.Xml;

namespace 老司机影片整理
{
    internal class NfoProcess
    {
        internal static void Create(string path, MovieInfo data, Config config)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("movie");
            doc.AppendChild(root);

            XmlElement title = doc.CreateElement("title");
            title.InnerText = config.TitleTemplate.Replace("$番号", data.Number).Replace("$标题", data.Title).Replace("$年代", data.Year).Replace("$女优", data.Star[0]);
            root.AppendChild(title);

            XmlElement year = doc.CreateElement("year");
            year.InnerText = data.Year.Substring(0, 4);
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
    }
}