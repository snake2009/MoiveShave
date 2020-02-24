using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;

namespace 老司机影片整理
{
    enum States
    {
        /// <summary>
        /// 运行中
        /// </summary>
        Runing,
        /// <summary>
        /// 已停止
        /// </summary>
        Stoped,
        /// <summary>
        /// 用户停止
        /// </summary>
        UserStop
    }

    /// <summary>
    /// 采集网站
    /// </summary>
    enum SiteTypes
    {
        All = 0,
        javbus = 1,
        javlibrary = 2,
        avsox = 3,
        fc2club = 4,
        avmoo = 5,
        javdb = 6,
        Auto = 10
    }

    internal class FileProcess
    {
        internal delegate void OnProgressHandle(int value);
        /// <summary>
        /// 进度事件
        /// </summary>
        internal static OnProgressHandle OnProgress;

        internal delegate void OnLogHandle(string str, string type = "tabPage_main", string color = "black");
        /// <summary>
        /// 日志事件
        /// </summary>
        internal static OnLogHandle OnLog;

        internal delegate void OnCompleteHandle();
        /// <summary>
        /// 完成事件
        /// </summary>
        internal static OnCompleteHandle OnComplete;

        /// <summary>
        /// 当前处理的索引
        /// </summary>
        private static int currentIndex = 0;
        private static Config config;

        /// <summary>
        /// 等待处理的视频列表
        /// </summary>
        private static List<string> filelist;

        /// <summary>
        /// 运行中的线程列表
        /// </summary>
        private static List<Thread> threadList;

        /// <summary>
        /// 同步状态
        /// </summary>
        private static object syncState = new object();

        /// <summary>
        /// 当前执行状态
        /// </summary>
        internal static States State { get; private set; } = States.Stoped;

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="_list">文件列表</param>
        internal static void Start(List<string> _list, Config _config)
        {
            if (_list.Count > 0)
            {
                currentIndex = 0;
                filelist = _list;
                config = _config;
                //更新状态为运行中
                State = States.Runing;
                StartThread(_list.Count >= config.MaxThread ? config.MaxThread : _list.Count);
            }
            else
            {
                OnLog?.Invoke($"没有需要处理的视频文件");
                OnComplete?.Invoke();
            }
        }

        /// <summary>
        /// 启动线程组
        /// </summary>
        /// <param name="count">启动数量</param>
        private static void StartThread(int count)
        {
            threadList = new List<Thread>();
            for (int i = 0; i < count; i++)
            {
                Thread thread1 = new Thread(()=> {
                    //循环处理
                    while (State == States.Runing && currentIndex < filelist.Count)
                    {
                        Thread.Sleep(10);
                        //处理
                        Proc();
                    }
                    //循环结束，触发完成事件
                    Complete();
                })
                {
                    Name = $"线程{i + 1}",
                    IsBackground = true
                };
                threadList.Add(thread1);
                thread1.Start();
            }
        }

        /// <summary>
        /// 处理视频
        /// </summary>
        private static void Proc()
        {
            //获取当前等待处理的文件
            string filename = GetFileName();
            try
            {
                int i = currentIndex;
                if (string.IsNullOrEmpty(filename))
                    return;
                var dirName = Path.GetDirectoryName(filename);
                //提取番号
                string num = GetMovieNum(filename);
                if (num == "")
                {
                    //没提取出番号，下一个
                    OnLog?.Invoke($"未能提取番号 {filename}", Thread.CurrentThread.Name);
                    return;
                }
                OnLog?.Invoke($"==========开始处理{num}==========", Thread.CurrentThread.Name);
                int value = (int)(i * 10.0 / filelist.Count * 10.0);
                OnProgress?.Invoke(value);
                //获取影片信息
                MovieInfo info = GetMovieInfo(num, config);
                if (info == null)
                {
                    //没取到信息，下一个
                    OnLog?.Invoke($"未取到影片信息【{filename}】");
                    return;
                }
                OnLog?.Invoke($"1.获取信息完成", Thread.CurrentThread.Name, "Green");
                //移动视频
                var videoPath = MoveVideo(filename, info, config);
                if (videoPath == "")
                    return;
                OnLog?.Invoke($"2.移动视频完成", Thread.CurrentThread.Name, "Green");

                //下载背景图
                var backdropPath = DownImage(videoPath, info.Backdrop, "Backdrop.jpg");
                if (backdropPath == "")
                {
                    if (info.Generate == "avsox")
                    {
                        info = GetMovieInfo(num, config, SiteTypes.javbus);
                        if (info == null)
                            return;
                        backdropPath = DownImage(videoPath, info.Backdrop, "Backdrop.jpg");
                        if (backdropPath == "")
                            return;
                    }
                }
                OnLog?.Invoke($"3.下载背景图完成", Thread.CurrentThread.Name, "Green");

                //处理封面图
                ProcessCover(videoPath, backdropPath, info);
                OnLog?.Invoke($"4.处理封面图完成", Thread.CurrentThread.Name, "Green");

                //创建NFO文件
                if (config.CreateNfo)
                {
                    NfoProcess.Create($"{videoPath}{Path.DirectorySeparatorChar}{info.Number}.nfo", info, config);
                    OnLog?.Invoke($"5.创建nfo文件完成", Thread.CurrentThread.Name, "Green");
                }
            }
            catch (Exception)
            {
                OnLog?.Invoke($"取到影片信息出错 {filename}");
            }
        }

        /// <summary>
        /// 处理封面图
        /// </summary>
        /// <param name="videoPath">视频文件夹</param>
        /// <param name="backdropPath">背景图</param>
        private static void ProcessCover(string videoPath, string backdropPath, MovieInfo info)
        {
            var coverPath = $"{videoPath}{Path.DirectorySeparatorChar}Cover.png";
            //如果有现成封面就下载
            if (!string.IsNullOrEmpty(info.Cover))
            {
                //下载封面
                DownImage(videoPath, info.Cover, "Cover.png");
                var img = Image.FromFile(coverPath);
                //封面图高度大于396直接返回，不再处理
                if (img.Height >= 396)
                {
                    img.Dispose();
                    return;
                }
                img.Dispose();
            }
            Image coverImage = null;
            //没有封面裁剪封面
            if (config.AIClip)
            {
                //从背景图识别智能裁剪封面
                coverImage = ImageProcess.GetCoverByAI(backdropPath, config);
            }
            else
            {
                //从背景图识别智能裁剪封面
                coverImage = ImageProcess.GetCover(backdropPath, config);
            }
            if (coverImage != null)
            {
                //保存封面
                //File.Delete(coverPath);
                coverImage.Save(coverPath, ImageFormat.Png);
                coverImage.Dispose();
            }
            else
            {
                OnLog?.Invoke($"处理封面图出错 {info.Number}");
            }
        }


        /// <summary>
        /// 获取影片信息
        /// </summary>
        /// <param name="num">番号</param>
        /// <param name="config">用户配置</param>
        /// <returns></returns>
        private static MovieInfo GetMovieInfo(string num, Config config, SiteTypes userSite = SiteTypes.Auto)
        {
            MovieInfo info = null;
            var site = userSite != SiteTypes.Auto ? userSite :(SiteTypes)Enum.Parse(typeof(SiteTypes), config.Site.ToString());
            switch (site)
            {
                case SiteTypes.All:
                    if (IsUncensored(num)) //无码处理
                    {
                        if (num.StartsWith("fc2", StringComparison.CurrentCultureIgnoreCase))
                        {
                            //fc2 单独处理
                            info = new Fc2Club().GetInfoAsync(num);
                            if (info == null)
                            {
                                //未找到再 Javdb
                                info = new Javdb().GetInfoAsync(num);
                            }
                        }
                        else
                        {
                            // 先 Avsox
                            info = new Avsox().GetInfoAsync(num);
                            if (info == null)
                            {
                                //未找到再 JavBus
                                info = new JavBus().GetInfoAsync(num);
                                if (info == null)
                                {
                                    //未找到再 Javdb
                                    info = new Javdb().GetInfoAsync(num);
                                }
                            }
                        }
                    }
                    else
                    {
                        //先 Avmoo
                        info = new Avmoo().GetInfoAsync(num);
                        if (info == null)
                        {
                            //未找到再 avBus
                            info = new JavBus().GetInfoAsync(num);
                            if (info == null)
                            {
                                //未找到再 Javdb
                                info = new Javdb().GetInfoAsync(num);
                                /*if (info == null)
                                {
                                    //未找到再 JavLibrary
                                    info = new JavLibrary().GetInfoAsync(num);
                                }*/
                            }
                        }
                    }
                    break;
                case SiteTypes.javbus:
                    info = new JavBus().GetInfoAsync(num);
                    break;
                case SiteTypes.avsox:
                    info = new Avsox().GetInfoAsync(num);
                    break;
                case SiteTypes.javlibrary:
                    info = new JavLibrary().GetInfoAsync(num);
                    break;
                case SiteTypes.fc2club:
                    info = new Fc2Club().GetInfoAsync(num);
                    break;
                case SiteTypes.avmoo:
                    info = new Avmoo().GetInfoAsync(num);
                    break;
                case SiteTypes.javdb:
                    info = new Javdb().GetInfoAsync(num);
                    break;
            }
            if (info != null)
            {
                info.Number = num;
            }
            return info;
        }

        /// <summary>
        /// 判断番号是否无码
        /// </summary>
        /// <param name="num">番号</param>
        /// <returns>是否无码</returns>
        private static bool IsUncensored(string num)
        {
            var name = num.ToLower();
            return name.Contains("sky")
                || name.Contains("red")
                || name.Contains("rhj")
                || name.Contains("sskj")
                || name.Contains("ccdv")
                || name.Contains("sskp")
                || name.Contains("sskx")
                || name.Contains("heyzo")
                || name.Contains("laf")
                || name.Contains("cwp")
                || name.Contains("mcb")
                || name.Contains("gachip")
                || name.Contains("s2m")
                || name.Contains("smd")
                || name.Contains("cwdv")
                || name.Contains("mcdv")
                || name.Contains("mcbd")
                || name.Contains("mcb3dbd")
                || name.Contains("mk3d2dbd")
                || name.Contains("jukujo")
                || name.Contains("drc")
                || name.Contains("bt-")
                || name.Contains("ct-")
                || name.Contains("pt-")
                || name.Contains("bd-")
                || name.Contains("candys")
                || name.Contains("fh-")
                || name.Contains("kg-")
                || name.Contains("mx-")
                || name.Contains("dnk")
                || name.Contains("fc2")
                || Regex.Match(name, @"^n\d{4}").Success
                || Regex.Match(name, @"\d{6}[-_]\d{2,3}").Success //030312_01 021816-099
                || Regex.Match(name, @"\d{6}[-_]\d{3}[-_]\d{2}").Success //021816-099
                || Regex.Match(name, @"\d{6}[-_][a-z_A-Z]+").Success //120316-SAKI_NOZOMI
                || name.Contains("dsam");
        }
        
        /// <summary>
        /// 移动视频
        /// </summary>
        /// <param name="filename">原始文件名</param>
        /// <param name="info">影片信息</param>
        /// <param name="config">用户配置</param>
        private static string MoveVideo(string filename, MovieInfo info, Config config)
        {
            try
            {
                var extName = Path.GetExtension(filename);
                var starPath = $"{config.LabraryPath}{Path.DirectorySeparatorChar}{info.Star[0]}";
                //创建演员文件夹
                Directory.CreateDirectory(starPath);
                //移动视频
                string newName = config.PathTemplate.Replace("$番号", info.Number).Replace("$标题", info.Title).Replace("$年代", info.Year).Replace("$女优", info.Star[0]);
                newName = newName.Replace(" : ", "").Replace("\\", "").Replace("/", "").TrimStart().TrimEnd();
                string videoPath = $"{starPath}{Path.DirectorySeparatorChar}{(newName.Length > 50 ? newName.Substring(0, 50) : newName)}";
                Directory.CreateDirectory(videoPath);
                string newFilePath = $"{videoPath}{Path.DirectorySeparatorChar}{info.Number}{extName}";
                if(File.Exists(filename)){
                    File.WriteAllText(newFilePath, "111");
                    //File.Move(filename, newFilePath);
                    return videoPath;
                }
            }
            catch (Exception)
            {
                OnLog?.Invoke($"移动视频出错 {info.Number}");
            }
            return "";
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="filename">目录名</param>
        /// <param name="cover">图片地址</param>
        private static string DownImage(string dirName, string imgUrl, string fileName)
        {
            try
            {
                HttpClient client = new HttpClient();
                var stram = client.GetStreamAsync(imgUrl).GetAwaiter().GetResult();
                var imgPath = $"{dirName}{Path.DirectorySeparatorChar}{fileName}";
                Image.FromStream(stram).Save(imgPath);
                return imgPath;
            }
            catch
            {
                OnLog?.Invoke($"下载图片出错 {imgUrl}");
                return "";
            }
        }

        /// <summary>
        /// 从文件名获取番号
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>番号</returns>
        private static string GetMovieNum(string filename)
        {
            //优先处理 (FC2)(745325) 这种格式
            var re = Regex.Match(filename, @"\(FC2\)\([0-9]{6,8}\)");
            if (re.Success)
            {
                return re.Groups[0].Value.Replace(")(", "-").Replace("(", "").Replace(")", "");
            }
            //正则匹配番号 支持 [AAA-111] [AAA_111] [AAA|111] [AAA 111] 四种格式，AAA支持1~8位长度允许夹杂数字（S2MBD-001），111支持2~8位数字
            re = Regex.Match(filename, @"([a-zA-Z0-9]{1,8})[-|_|\s]{0,3}([0-9]{2,8})(.*?)");
            if (re.Success)
            {
                return re.Groups[0].Value;
            }
            return "";
        }

        /// <summary>
        /// 完成事件
        /// </summary>
        private static void Complete()
        {
            lock (syncState)
            {
                if (currentIndex == filelist.Count || State == States.UserStop)
                {
                    State = States.Stoped;
                    OnComplete?.Invoke();
                }
            }
        }

        /// <summary>
        /// 获取下一个要处理的视频文件
        /// </summary>
        /// <returns>文件名</returns>
        private static string GetFileName()
        {
            lock (syncState)
            {
                var name = filelist[currentIndex];
                currentIndex += 1;
                return name;
            }
        }

        /// <summary>
        /// 停止执行
        /// </summary>
        internal static void Stop()
        {
            State = States.UserStop;
        }
    }
}