namespace 老司机影片整理
{
    internal class Config
    {
        /// <summary>
        /// 网站来源 0 自适应 1 javbus 2 javlibrary 3 avsox【无码】4 fc2club
        /// </summary>
        public int Site { get;set; }
        /// <summary>
        /// 文件名重命名模版
        /// </summary>
        public string PathTemplate { get; set; }
        /// <summary>
        /// 目录格式 0 演员/影片
        /// </summary>
        public int PathType { get; set; }
        /// <summary>
        /// 媒体库路径
        /// </summary>
        public string LibraryPath { get; set; }
        /// <summary>
        /// AI截图
        /// </summary>
        public bool AIClip { get; set; }
        /// <summary>
        /// 是否生成nfo
        /// </summary>
        public bool CreateNfo { get; set; }
        /// <summary>
        /// 最大线程数 默认5 范围1~10
        /// </summary>
        public int MaxThread { get; set; } = 1;
        /// <summary>
        /// 百度AI平台key
        /// </summary>
        public string BD_API_KEY { get; set; }
        /// <summary>
        /// 百度AI平台secret
        /// </summary>
        public string BD_SECRET_KEY { get; set; }
        /// <summary>
        /// 媒体库显示标题模版
        /// </summary>
        public string TitleTemplate { get; set; }
        /// <summary>
        /// 跳过存在的图片
        /// </summary>
        public bool SkipExistsImage { get;set; }
        /// <summary>
        /// 跳过搜索存在nfo的视频
        /// </summary>
        public bool SkipSearchExistsNfo { get; set; }
        /// <summary>
        /// 有码片不使用AI裁剪
        /// </summary>
        public bool CensoredNoAI { get; set; }
        /// <summary>
        /// AI人脸识别准确度阀值
        /// </summary>
        public double AIScore { get; set; }
    }
}