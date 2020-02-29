using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
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
        /// <summary>
        /// 进度事件
        /// </summary>
        internal delegate void OnProgressHandle(int value);

        /// <summary>
        /// 日志事件
        /// </summary>
        internal delegate void OnLogHandle(string str, LogLevels logLevel = LogLevels.Out);

        /// <summary>
        /// 完成事件
        /// </summary>
        internal delegate void OnCompleteHandle();

        private static Config config;
        private static OnCompleteHandle OnComplete;
        private static OnLogHandle OnLog;
        private static OnProgressHandle OnProgress;

        /// <summary>
        /// 当前处理的索引
        /// </summary>
        private static int currentIndex = 0;

        /// <summary>
        /// 等待处理的视频列表
        /// </summary>
        private static List<VideoInfo> filelist;

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
        internal static void Start(List<VideoInfo> _list, Config _config, OnCompleteHandle _onCompleteHandle = null, OnLogHandle _onLogHandle = null, OnProgressHandle _onProgressHandle = null)
        {
            OnComplete = _onCompleteHandle;
            OnLog = _onLogHandle;
            OnProgress = _onProgressHandle;
            config = _config;
            filelist = _list;

            if (_list.Count > 0)
            {
                //创建媒体库文件夹
                if (!Directory.Exists(config.LibraryPath))
                {
                    try
                    {
                        Directory.CreateDirectory(config.LibraryPath);
                    }
                    catch (Exception)
                    {
                        OnLog?.Invoke($"创建媒体库 \"{config.LibraryPath}\" 失败，请检查分区是否存在或者选择一个新路径作为媒体库");
                        OnComplete?.Invoke();
                        return;
                    }
                }
                currentIndex = 0;
                //更新状态为运行中
                State = States.Runing;
                StartThread(filelist.Count >= config.MaxThread ? config.MaxThread : filelist.Count);
            }
            else
            {
                OnLog?.Invoke($"没有需要处理的视频文件", LogLevels.Waining);
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
                        ProcMoive();
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
        private static void ProcMoive()
        {
            //获取当前等待处理的文件
            var videoInfo = GetFileName();
            if (!string.IsNullOrEmpty(videoInfo.nfo) || !File.Exists(videoInfo.filename))
            {
                return;
            }
            int i = currentIndex - 1;
            //是否取到文件
            if (string.IsNullOrEmpty(videoInfo.filename) 
                || string.IsNullOrEmpty(videoInfo.name) 
                || string.IsNullOrEmpty(videoInfo.num))
                return;
            Log.Save($"开始处理文件 {videoInfo.filename}");
            try
            {
                OnLog?.Invoke($"==========开始处理{videoInfo.num}==========");
                int value = (int)(i * 10.0 / filelist.Count * 10.0);
                OnProgress?.Invoke(value);
                //获取影片信息
                Log.Save($"准备获取影片信息 {videoInfo.num}");
                Log.Save($"无码影片: {videoInfo.avtype}");
                MovieInfo info = GetMovieInfo(videoInfo.num, config);
                filelist[i].generate = info.Generate;
                filelist[i].info = "成功";

                //移动视频
                var videoPath = MoveVideo(videoInfo.filename, info, config);
                filelist[i].video = "成功";

                //处理封面图
                ProcessCover(videoInfo.num, videoPath, info, filelist[i]);

                //创建NFO文件
                NfoTools.Create($"{videoPath}{Path.DirectorySeparatorChar}{info.Number}.nfo", info, config);
                OnLog?.Invoke($"创建nfo文件完成", LogLevels.Success);
                Log.Save($"创建nfo文件完成");
                filelist[i].nfo = "成功";
            }
            catch (Exception e)
            {
                OnLog?.Invoke($"处理出错 {e.Message}\n{e.StackTrace}", LogLevels.Error);
            }
        }

        /// <summary>
        /// 处理封面图
        /// </summary>
        /// <param name="videoPath">视频文件夹</param>
        /// <param name="backdropPath">背景图</param>
        private static void ProcessCover(string num, string videoPath, MovieInfo info, VideoInfo videoInfo)
        {
            //下载背景图
            var backdropPath = DownImage(videoPath, info.Backdrop, "Backdrop.jpg", config.SkipExistsImage);
            if (backdropPath == "")
            {
                if (info.Generate == "avsox")//avsox 老片子个别没图片，用javbus 在尝试一次
                {
                    Log.Save($"下载 avsox 图片失败, 换 javbus");
                    info = GetMovieInfo(num, config, SiteTypes.javbus);
                    if (info != null)
                    {
                        backdropPath = DownImage(videoPath, info.Backdrop, "Backdrop.jpg", config.SkipExistsImage);
                    }
                }
            }
            if (backdropPath == "")
            {
                Log.Save($"下载背景图失败失败");
                throw new Exception("下载背景图失败");
            }
            OnLog?.Invoke($"下载背景图完成", LogLevels.Success);
            videoInfo.back = "成功";

            string coverPath;
            //如果有现成的就下载
            if (!string.IsNullOrEmpty(info.Cover))
            {
                Log.Save($"下载网站上的封面");
                //下载封面
                coverPath = DownImage(videoPath, info.Cover, "Cover.png");
                if (coverPath != "")
                {
                    var img = Image.FromFile(coverPath);
                    if (img.Height >= 396 && Math.Abs(img.Height * 0.7029702970297 - img.Width) <= 5)
                    {
                        //封面图高度大于396，可以用，不再处理
                        img.Dispose();
                        return;
                    }
                    img.Dispose();
                    img = Image.FromFile(backdropPath);
                    if (img.Height <= 350)
                    {
                        //背景图高度<396，没有截图的必要，不再处理
                        img.Dispose();
                        return;
                    }
                    img.Dispose();
                }
            }

            Log.Save($"下载识别，或者下载的尺寸太小，准备截图");
            //没发下载，或者下载的尺寸太小 需要截图
            new ImageTools(config).ClipCover(videoPath, backdropPath, !config.CensoredNoAI && videoInfo.avtype != "无码");
            videoInfo.cover = "成功";
            Log.Save($"处理封面图完成");
            OnLog?.Invoke($"处理封面图完成", LogLevels.Success);
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
            var site = userSite != SiteTypes.Auto ? userSite : (SiteTypes)Enum.Parse(typeof(SiteTypes), config.Site.ToString());
            switch (site)
            {
                case SiteTypes.All:
                    Log.Save($"使用系统默认采集顺序");
                    if (NumberTools.IsUncensored(num)) //无码处理
                    {
                        if (num.StartsWith("fc2", StringComparison.CurrentCultureIgnoreCase))
                        {
                            Log.Save($"使用 Fc2Club 影片采集");
                            //fc2 单独处理
                            info = new Fc2Club().GetInfoAsync(num);
                            if (info == null)
                            {
                                Log.Save($"Fc2Club 采集失败，使用 Javdb 采集");
                                //未找到再 Javdb
                                info = new Javdb().GetInfoAsync(num);
                            }
                        }
                        else
                        {
                            // 先 Avsox
                            Log.Save($"使用 Avsox 影片采集");
                            info = Avsox.Instance.GetInfoAsync(num);
                            if (info == null)
                            {
                                //未找到再 JavBus
                                Log.Save($"Avsox 采集失败，使用 JavBus 采集");
                                info = JavBus.Instance.GetInfoAsync(num);
                                if (info == null)
                                {
                                    Log.Save($"JavBus 采集失败，使用 Javdb 采集");
                                    //未找到再 Javdb
                                    info = new Javdb().GetInfoAsync(num);
                                }
                            }
                        }
                    }
                    else
                    {
                        //先 Avmoo
                        Log.Save($"使用 Avmoo 影片采集");
                        info = new Avmoo().GetInfoAsync(num);
                        if (info == null)
                        {
                            //未找到再 avBus
                            Log.Save($"Avmoo 采集失败，使用 JavBus 采集");
                            info = JavBus.Instance.GetInfoAsync(num);
                            if (info == null)
                            {
                                //未找到再 Javdb
                                Log.Save($"JavBus 采集失败，使用 Javdb 采集");
                                info = new Javdb().GetInfoAsync(num);
                                /*if (info == null)
                                {
                                    //未找到再 JavLibrary
                                    info = new JavLibrary().GetInfoAsync(num);
                                }*/
                            }
                        }
                        else
                        {
                            if (info.Star == null)
                            {
                                //未找到再 avBus
                                Log.Save($"Avmoo 未采集到演员，使用 JavBus 采集");
                                info = JavBus.Instance.GetInfoAsync(num);
                            }
                        }
                    }
                    break;
                case SiteTypes.javbus:
                    Log.Save($"使用 JavBus 采集");
                    info = JavBus.Instance.GetInfoAsync(num);
                    break;
                case SiteTypes.avsox:
                    Log.Save($"使用 Avsox 采集");
                    info = Avsox.Instance.GetInfoAsync(num);
                    break;
                case SiteTypes.javlibrary:
                    Log.Save($"使用 JavLibrary 采集");
                    info = new JavLibrary().GetInfoAsync(num);
                    break;
                case SiteTypes.fc2club:
                    Log.Save($"使用 Fc2Club 采集");
                    info = new Fc2Club().GetInfoAsync(num);
                    break;
                case SiteTypes.avmoo:
                    Log.Save($"使用 Avmoo 采集");
                    info = new Avmoo().GetInfoAsync(num);
                    break;
                case SiteTypes.javdb:
                    Log.Save($"使用 Javdb 采集");
                    info = new Javdb().GetInfoAsync(num);
                    break;
            }
            if (info != null)
            {
                info.Number = num;
                Log.Save($"获取影片信息完成 <= [{info.Generate}]");
                OnLog?.Invoke($"获取影片信息完成 <= [{info.Generate}]", LogLevels.Success);
                return info;
            }
            else
            {
                Log.Save($"未获取到影片信息");
                throw new Exception($"未获取到影片信息 {num}");
            }
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
                Log.Save($"准备移动视频");
                var extName = Path.GetExtension(filename);
                var star = (info.Star == null ? "未知" : info.Star[0]);
                star = star.Replace("/", " ").Replace(@"\", "").Replace("?", "");
                var starPath = $"{config.LibraryPath}{Path.DirectorySeparatorChar}{star}";
                //创建演员文件夹
                Log.Save($"创建演员文件夹 {starPath}");
                Directory.CreateDirectory(starPath);
                //移动视频
                string newName = config.PathTemplate.Replace("$番号", info.Number).Replace("$标题", info.Title).Replace("$年代", info.Year).Replace("$女优", info.Star==null?"": info.Star[0]);
                newName = newName.Replace(" : ", "").Replace(":", "").Replace("?", "").Replace("/", "").Replace("*", "").Replace("\n", "").Replace(@"\", "").TrimStart().TrimEnd();
                string videoPath = $"{starPath}{Path.DirectorySeparatorChar}{(newName.Length > 50 ? newName.Substring(0, 50) : newName)}".TrimEnd();
                Log.Save($"创建视频文件夹 {videoPath}"); 
                Directory.CreateDirectory(videoPath);
                string newFilePath = $"{videoPath}{Path.DirectorySeparatorChar}{info.Number}{extName}";
                //判断是否两个视频
                var dirName = Path.GetDirectoryName(filename);
                var name = Path.GetFileNameWithoutExtension(filename);
                var filename1 = $"{dirName}{Path.DirectorySeparatorChar}{name}_1{extName}";
                var filename2 = $"{dirName}{Path.DirectorySeparatorChar}{name}_2{extName}";
                var filename3 = $"{dirName}{Path.DirectorySeparatorChar}{name}_3{extName}";
                var filename4 = $"{dirName}{Path.DirectorySeparatorChar}{name}_4{extName}";
                if (File.Exists(filename1))
                {
                    newFilePath = $"{videoPath}{Path.DirectorySeparatorChar}{info.Number}-cd1{extName}";
                    File.Move(filename, newFilePath);
                    Log.Save($"移动cd1完成 {newFilePath}");

                    newFilePath = $"{videoPath}{Path.DirectorySeparatorChar}{info.Number}-cd2{extName}";
                    File.Move(filename1, newFilePath);
                    Log.Save($"移动cd2完成 {newFilePath}");
                    if (File.Exists(filename2))
                    {
                        newFilePath = $"{videoPath}{Path.DirectorySeparatorChar}{info.Number}-cd3{extName}";
                        File.Move(filename2, newFilePath);
                        Log.Save($"移动cd3完成 {newFilePath}");
                    }
                    if (File.Exists(filename3))
                    {
                        newFilePath = $"{videoPath}{Path.DirectorySeparatorChar}{info.Number}-cd4{extName}";
                        File.Move(filename3, newFilePath);
                        Log.Save($"移动cd3完成 {newFilePath}");
                    }
                    if (File.Exists(filename4))
                    {
                        newFilePath = $"{videoPath}{Path.DirectorySeparatorChar}{info.Number}-cd5{extName}";
                        File.Move(filename4, newFilePath);
                        Log.Save($"移动cd3完成 {newFilePath}");
                    }
                    OnLog?.Invoke($"移动多个视频完成", LogLevels.Success);

                    return videoPath;
                }
                else
                {
                    File.Move(filename, newFilePath);
                    OnLog?.Invoke($"移动视频完成", LogLevels.Success); 
                    Log.Save($"移动视频完成 {newFilePath}");
                    return videoPath;
                }
            }
            catch (Exception e)
            {
                OnLog?.Invoke($"移动视频出错 {info.Number}", LogLevels.Error);
                throw e;
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="filename">目录名</param>
        /// <param name="cover">图片地址</param>
        private static string DownImage(string dirName, string imgUrl, string fileName, bool skipExistsImage = false)
        {
            var imgPath = $"{dirName}{Path.DirectorySeparatorChar}{fileName}";
            try
            {
                if (!skipExistsImage || !File.Exists(imgPath))
                {
                    HttpClient client = new HttpClient();
                    var stram = client.GetStreamAsync(imgUrl).GetAwaiter().GetResult();
                    Image.FromStream(stram).Save(imgPath);
                }
                Log.Save($"下载图片成功 [{imgPath}]");
                return imgPath;
            }
            catch(Exception e)
            {
                Log.Save($"下载图片失败 [{imgUrl}] {e.Message}");
                return "";
            }
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
        private static VideoInfo GetFileName()
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