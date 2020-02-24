using HtmlAgilityPack;
using System.Collections.Generic;

namespace 老司机影片整理
{
    class MovieInfo
    {
        /// <summary>
        /// 番号
        /// </summary>
        public string Number { get; internal set; }
        /// <summary>
        /// 封面
        /// </summary>
        public string Cover { get; internal set; }
        /// <summary>
        /// 背景图
        /// </summary>
        public string Backdrop { get; internal set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; internal set; }
        /// <summary>
        /// 发行年份
        /// </summary>
        public string Year { get; internal set; }
        /// <summary>
        /// 演员
        /// </summary>
        public List<string> Star { get; internal set; }
        /// <summary>
        /// 类别
        /// </summary>
        public List<string> Genre { get; internal set; }
        /// <summary>
        /// 长度
        /// </summary>
        public string Length { get; internal set; }
        /// <summary>
        /// 厂商
        /// </summary>
        public string Studio { get; internal set; }
        /// <summary>
        /// 系列
        /// </summary>
        public string Series { get; internal set; }
        /// <summary>
        /// 网址
        /// </summary>
        public string WebSite { get; internal set; }
        /// <summary>
        /// 导演
        /// </summary>
        public string Direct { get; internal set; }
        /// <summary>
        /// 发行商
        /// </summary>
        public string Publisher { get; internal set; }
        public string Generate { get; internal set; }
    }
}
