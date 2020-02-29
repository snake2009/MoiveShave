using DevComponents.DotNetBar;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace 老司机影片整理
{
    public partial class MainForm : OfficeForm
    {
        private bool IsRun;
        private List<string> Notkeys;
        private Config config = new Config();
        private string configPath;
        private BindingSource bs_file;
        private InfoCard selectCard;
        private List<string> libFiles;
        private int libPage = -1;

        private List<VideoInfo> VideoFileList { get; set; } = new List<VideoInfo>();

        public MainForm()
        {
            InitializeComponent();
            table.BackgroundColor = Color.AliceBlue;
            tabControlPanel1.Style.BackColor2.Color = Color.White;
            tabControlPanel2.Style.BackColor2.Color = Color.White;
            tabControlPanel3.Style.BackColor2.Color = Color.White;
            tabControlPanel4.Style.BackColor2.Color = Color.White;
            SetToastNotification();
            Notkeys = new List<string>();
            LoadConfig();
        }

        private void SetToastNotification()
        {
            ToastNotification.DefaultToastPosition = eToastPosition.MiddleCenter;
            ToastNotification.DefaultTimeoutInterval = 1100;
            ToastNotification.ToastFont = new Font("微软雅黑", 14F);
            ToastNotification.ToastBackColor = Color.FromArgb(50, 50, 50);
            ToastNotification.ToastForeColor = Color.FromArgb(200, 255, 255, 255);
        }

        private void LoadConfig()
        {
            configPath = $"{Application.StartupPath}{Path.DirectorySeparatorChar}config.json";
            if (File.Exists(configPath))
            {
                var str = File.ReadAllText(configPath);
                config = JsonConvert.DeserializeObject<Config>(str);
                if (config == null)
                {
                    config = new Config();
                }
                comboBox_site.SelectedIndex = config.Site;
                checkBox_createnfo.Checked = config.CreateNfo;
                textBox_librarypath.Text = config.LibraryPath;
                textBox_titletemplate.Text = config.TitleTemplate;
                textBox_pathtemplate.Text = config.PathTemplate;
                radioButton_clip.Checked = !config.AIClip;
                radioButton_aiclip.Checked = config.AIClip;
                textBox_apikey.Text = config.BD_API_KEY;
                textBox_secretkey.Text = config.BD_SECRET_KEY;
                checkBox_imgnodown.Checked = config.SkipExistsImage;
                checkBox_filternfo.Checked = config.SkipSearchExistsNfo;
                checkBox_noai.Checked = config.CensoredNoAI;
                doubleInput_score.Value = config.AIScore;
            }
            else
            {
                comboBox_site.SelectedIndex = 0;
            }
        }

        private void SaveConfig()
        {
            var str = JsonConvert.SerializeObject(config);
            File.WriteAllText(configPath, str, Encoding.UTF8);
        }

        #region 界面事件

        private void Form1_Load(object sender, EventArgs e)
        {
            bs_file = new BindingSource(VideoFileList, "");
            table.DataSource = bs_file;

            Notkeys.Add("獨佔動畫");
            Notkeys.Add("６９");
            Notkeys.Add("家中");
            Notkeys.Add("辣妹");
            Notkeys.Add("顏射");
            Notkeys.Add("亂搞");
            Notkeys.Add("姐姐");
            Notkeys.Add("振動");
            Notkeys.Add("美少女");
            Notkeys.Add("豐滿");
            Notkeys.Add("素人");
            Notkeys.Add("美女");
            Notkeys.Add("中出");
            Notkeys.Add("吸精");
            Notkeys.Add("坐浴盆");
            Notkeys.Add("中出し精液大量垂れ流し");
            Notkeys.Add("鴨嘴");
            Notkeys.Add("レ○プ");
            Notkeys.Add("無套");
            Notkeys.Add("中出し精液飲み・啜り・食い");
            Notkeys.Add("精液流し込みカメラ");
            Notkeys.Add("懸掛");
            Notkeys.Add("面具男(面具)");
            Notkeys.Add("陰道放屁");
            Notkeys.Add("陽台");
            Notkeys.Add("潮吹き（大量）");
            Notkeys.Add("陰道放入食物");
            Notkeys.Add("胸に精液");
            Notkeys.Add("髪に精液");
            Notkeys.Add("目隠しプレイ(男女)");
            Notkeys.Add("中出し精液潮混合噴射");
            Notkeys.Add("テコキ発射");
            Notkeys.Add("マンコ2本挿し");
            Notkeys.Add("テコキ発射");
            Notkeys.Add("精液流し込み・押込み");
            Notkeys.Add("アナル玩弄");
            Notkeys.Add("アナルマンコ同時挿入");
            Notkeys.Add("腸汁");
            Notkeys.Add("文檔/實錄");
            Notkeys.Add("1080p");
            Notkeys.Add("60fps");
            Notkeys.Add("マンコぶっかけ");
            Notkeys.Add("膣內カメラ・膣內撮影具");
            Notkeys.Add("10代");
            Notkeys.Add("女のアナルを舐める");
            Notkeys.Add("腹に精液");
            Notkeys.Add("烤肉妹");
            Notkeys.Add("指法");
            Notkeys.Add("企劃物");
            Notkeys.Add("自慰");
            Notkeys.Add("マングリ逆流ザー汁浴び");
            Notkeys.Add("顔に大量精液");
            Notkeys.Add("淫語");
            Notkeys.Add("VIP");
            Notkeys.Add("美乳");
            Notkeys.Add("69");
            Notkeys.Add("舔陰");
            Notkeys.Add("手淫");
            Notkeys.Add("後入");
            Notkeys.Add("AV女優");
            Notkeys.Add("美屁股");
            Notkeys.Add("肛門");
            Notkeys.Add("性交");
            Notkeys.Add("口交");
            Notkeys.Add("POV");
            Notkeys.Add("口內射精");
            Notkeys.Add("精液塗抹");
            Notkeys.Add("尿道插入異物");
            Notkeys.Add("白目剝き");
            Notkeys.Add("打屁股");
            Notkeys.Add("女が男のアナル舐める");
            Notkeys.Add("中出し拒み無視");
            Notkeys.Add("薄馬賽克");
            Notkeys.Add("主觀視角");
            Notkeys.Add("デカチン・巨根");
            Notkeys.Add("アナル中出し");
            Notkeys.Add("ザーメン舐め取り・啜り");
            Notkeys.Add("顔に精液");
            Notkeys.Add("中出し掻き回し");
            Notkeys.Add("顏面騎乘");
            Notkeys.Add("玩具");
            Notkeys.Add("中出し20発以上");
            Notkeys.Add("マングリ返し");
            Notkeys.Add("中出しクスコ覗き");
            Notkeys.Add("男が女の耳を舐める");
            Notkeys.Add("アナルセックス");
            Notkeys.Add("側位中出し");
            Notkeys.Add("拘束・拘束具");
            Notkeys.Add("輪姦中出し");
            Notkeys.Add("汁男優連続膣內射精");
            Notkeys.Add("手足拘束枕営業中");
            Notkeys.Add("クスコ膣內見せ");
            Notkeys.Add("高畫質");
            Notkeys.Add("數位馬賽克");
            Notkeys.Add("DMM獨家");
            Notkeys.Add("合集");
            Notkeys.Add("單體作品");
            Notkeys.Add("DVD多士爐");
            Notkeys.Add("身體意識");
            Notkeys.Add("女生");
            Notkeys.Add("蕩婦");
            Notkeys.Add("電動按摩棒");
            Notkeys.Add("パンスト破き・切り");
            Notkeys.Add("バック中出し");
        }

        private void button_addpath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                //检查用户添加是否是文件夹
                if (Directory.Exists(fbd.SelectedPath))
                {
                    //List<VideoInfo> list = new List<VideoInfo>();
                    //获取目录下面的视频文件
                    var files = Directory.EnumerateFiles(fbd.SelectedPath, "*.*", SearchOption.AllDirectories)
                        .Where(f =>
                        {
                            var filename = Path.GetFileNameWithoutExtension(f);
                            return (f.EndsWith(".mkv", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".avi", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".asf", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".wmv", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".rmvb", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".rm", StringComparison.CurrentCultureIgnoreCase)) &&
                            !filename.EndsWith("-cd1", StringComparison.CurrentCultureIgnoreCase) &&
                            !filename.EndsWith("-cd2", StringComparison.CurrentCultureIgnoreCase) &&
                            !filename.EndsWith("-cd3", StringComparison.CurrentCultureIgnoreCase) &&
                            !filename.EndsWith("-cd4", StringComparison.CurrentCultureIgnoreCase) &&
                            !filename.EndsWith("-cd5", StringComparison.CurrentCultureIgnoreCase) &&
                            !filename.EndsWith("_1", StringComparison.CurrentCultureIgnoreCase) &&
                            !filename.EndsWith("_2", StringComparison.CurrentCultureIgnoreCase) &&
                            !filename.EndsWith("_3", StringComparison.CurrentCultureIgnoreCase) &&
                            !filename.EndsWith("_4", StringComparison.CurrentCultureIgnoreCase);
                        }).Select(f =>
                        {
                            var name = Path.GetFileName(f);
                            return new VideoInfo()
                            {
                                name = name,
                                filename = f,
                                num = NumberTools.Get(name),
                                avtype = NumberTools.IsUncensored(name) ? "无码" : ""
                            };
                        }).ToList();
                    //过滤已经存在nfo的视频
                    if (checkBox_filternfo.Checked)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            var item = files[i];
                            var name = $"{Path.GetDirectoryName(item.filename)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(item.name).Replace("-cd1", "").Replace("-cd2", "")}.nfo";
                            if (File.Exists(name))
                            {
                                files.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                    files.Sort((x1, y1) =>
                    {
                        if (x1.num != null && y1.num != null)
                        {
                            return x1.num.CompareTo(y1.num);
                        }
                        return 0;
                    });
                    for (int i = 0; i < files.Count; i++)
                    {
                        files[i].index = i + 1;
                    }
                    VideoFileList.AddRange(files);
                    bs_file.ResetBindings(false);
                    ShowToast($"共搜索到 {VideoFileList.Count} 个视频文件");
                }
            }
            fbd.Dispose();
        }

        private void button_clearpath_Click(object sender, EventArgs e)
        {
            if (!IsRun)
            {
                for (int i = 0; i < VideoFileList.Count; i++)
                {
                    if (!string.IsNullOrEmpty(VideoFileList[i].nfo))
                    {
                        table.Rows.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < VideoFileList.Count; i++)
                {
                    VideoFileList[i].index = i + 1;
                }
            }
            else
            {
                ShowToast("请先停止任务，再清空");
            }
        }

        private void button_clearall_Click(object sender, EventArgs e)
        {
            if (!IsRun)
            {
                table.Rows.Clear();
            }
            else
            {
                ShowToast("请先停止任务，再清空");
            }
        }

        private void button_selectlabrarypath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBox_librarypath.Text = fbd.SelectedPath;
            }
            fbd.Dispose();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (IsRun)
                {
                    Hide();
                    e.Cancel = true;
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Visible = !Visible;
        }

        private void textBox_filenametemplate_Click(object sender, EventArgs e)
        {
            toolTip1.Show("支持模版：$番号 $标题 $年代 $女优\n例：$年代-$番号-$标题", textBox_pathtemplate);
        }

        private void textBox_titletemplate_Click(object sender, EventArgs e)
        {
            toolTip1.Show("支持模版：$番号 $标题 $年代 $女优\n例：$年代-$番号-$标题", textBox_titletemplate);
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void table_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 2)
            {
                Process.Start(Path.GetDirectoryName(table[1, e.RowIndex].Value.ToString()));
            }
        }

        #endregion

        /// <summary>
        /// 开始处理
        /// </summary>
        private void Start()
        {
            if (IsRun)
            {
                FileProcess.Stop();
                ShowToast("手动停止中...");
                button_start.Enabled = false;
                progressBar1.Visible = false;
                return;
            }
            IsRun = true;
            config.MaxThread = 1;
            progressBar1.Visible = true;
            //开始整理
            FileProcess.Start(VideoFileList, config, new FileProcess.OnCompleteHandle(() =>
            {
                //更新界面为停止运行
                UpdateUI(false);
                IsRun = false;
            }), new FileProcess.OnLogHandle((str, logLevel) =>
            {
                LogAdd(str, logLevel);
            }), new FileProcess.OnProgressHandle((value) => { UpdateProgress(value); }));
            progressBar1.ProgressType = eProgressItemType.Standard;

            //没找到文件的时候FileProcess会直接触发结束事件，优先执行，导致这里覆盖了UI状态
            if (IsRun)
            {
                //更新界面为开始运行
                UpdateUI(true);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (bgWorker1.IsBusy)
            {
                ShowToast("媒体库正在扫描中，请稍等");
                return;
            }
            if (Directory.Exists(config.LibraryPath))
            {
                if (tabControlPanel4.Controls["cardPanel"] != null)
                {
                    tabControlPanel4.Controls["cardPanel"].Dispose();
                    GC.Collect();
                }
                circularProgress1.Visible = true;
                labelX5.Visible = true;
                circularProgress1.IsRunning = true;

                bgWorker1.RunWorkerAsync();
            }
            else
            {
                ShowToast("媒体库文件夹不存在");
            }
        }

        /// <summary>
        /// 加载媒体库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            libFiles = Directory.EnumerateFiles(config.LibraryPath, "*.*", SearchOption.AllDirectories)
                    .Where(f =>
                    f.EndsWith(".mkv", StringComparison.CurrentCultureIgnoreCase) ||
                    f.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) ||
                    f.EndsWith(".avi", StringComparison.CurrentCultureIgnoreCase) ||
                    f.EndsWith(".asf", StringComparison.CurrentCultureIgnoreCase) ||
                    f.EndsWith(".wmv", StringComparison.CurrentCultureIgnoreCase) ||
                    f.EndsWith(".rmvb", StringComparison.CurrentCultureIgnoreCase) ||
                    f.EndsWith(".rm", StringComparison.CurrentCultureIgnoreCase)).ToList();
            libFiles.Sort((x1, y1) =>
            {
                var x = Path.GetFileNameWithoutExtension(x1);
                var y = Path.GetFileNameWithoutExtension(y1);
                return x.CompareTo(y);
            });
            libPage++;
            AddCard();
        }

        private void AddCard()
        {
            Invoke(new EventHandler((o1, e1) =>
            {
                tabControlPanel4.Controls.RemoveByKey("cardPanel");
            }));
            var flowLayoutPanel1 = new Panel()
            {
                Name = "cardPanel",
                AutoScroll = true,
                Location = new Point(55, 44),
                Size = new Size(tabControlPanel4.Width - 479, tabControlPanel4.Height - 8),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White
            };
            var count = 6;
            var width = (flowLayoutPanel1.Width - 30) / count;
            for (int i = libPage * 12; i < libPage * 12 + 12; i++)
            {
                var item = libFiles[i];
                try
                {
                    //加载影片信息
                    var movieInfo = NfoTools.Load(item);

                    //创建信息卡片
                    var card = new InfoCard()
                    {
                        Width = width,
                        Height = (int)(width / 0.55),
                        Cursor = Cursors.Hand,
                        Movie = movieInfo,
                        VideoFile = new VideoInfo()
                        {
                            filename = item,
                            name = Path.GetFileNameWithoutExtension(item),
                            num = movieInfo.Number
                        }
                    };
                    //双击播放事件
                    card.DoubleClick += (o1, e1) =>
                    {
                        Process.Start(item);
                    };
                    //单击信息查看
                    card.Click += (o1, e1) =>
                    {
                        ShowMovieDetail(card);
                    };
                    //计算位置
                    var x = (i % 12) % count * card.Width + ((i % 12) % count) + 4;
                    var y = (i % 12) / count * card.Height + ((i % 12) / count * 20) + 10;
                    card.Location = new Point(x, y);
                    flowLayoutPanel1.Controls.Add(card);
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1.Message);
                }
            }
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                Invoke(new EventHandler((o1, e1) =>
                {
                    tabControlPanel4.Controls.Add(flowLayoutPanel1);
                }));
            }
            else
            {
                flowLayoutPanel1.Dispose();
            }
        }

        /// <summary>
        /// 媒体库加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            circularProgress1.Visible = false;
            labelX5.Visible = false;
            circularProgress1.IsRunning = false;
        }

        /// <summary>
        /// 手动裁剪封面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_clip_Click(object sender, EventArgs e)
        {
            if (selectCard == null)
            {
                ShowToast("请先选择一部影片");
                return;
            }
            try
            {
                new ImageTools(config).ClipCover(Path.GetDirectoryName(selectCard.VideoFile.filename), selectCard.Movie.Backdrop, !config.CensoredNoAI && selectCard.VideoFile.avtype != "无码");
                selectCard.Movie = selectCard.Movie;
                selectCard.Refresh();
                ShowToast("裁剪完成");
                return;
            }
            catch (Exception e1)
            {
                ShowToast($"裁剪出错 {e1.Message}");
            }
        }

        /// <summary>
        /// 打开所在文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_openfolder_Click(object sender, EventArgs e)
        {
            if (selectCard == null)
            {
                ShowToast("请先选择一部影片");
                return;
            }
            Process.Start(Path.GetDirectoryName(selectCard.Movie.Backdrop));
        }

        /// <summary>
        /// 访问影片采集页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_website_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel_website.Text);
        }

        /// <summary>
        /// 采集影片资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_update_Click(object sender, EventArgs e)
        {
            if (IsRun)
            {
                ShowToast("有任务正在执行");
                return;
            }
            if (selectCard == null)
            {
                ShowToast("请先选择一部影片");
                return;
            }
            panelEx1.Enabled = false;
            ShowToast("正在采集影片信息...");
            FileProcess.Start(new List<VideoInfo>() { selectCard.VideoFile }, config, new FileProcess.OnCompleteHandle(() =>
            {
                Invoke(new EventHandler((o1, e1) =>
                {
                    selectCard.Movie = NfoTools.Load(selectCard.VideoFile.filename);
                    panelEx1.Enabled = true;
                    ShowToast("影片信息采集成功");
                }));
            }),
            new FileProcess.OnLogHandle((str, logLevel) =>
            {
                LogAdd(str, logLevel);
            }));
        }

        /// <summary>
        /// 显示影片详情
        /// </summary>
        /// <param name="card">卡片</param>
        private void ShowMovieDetail(InfoCard card)
        {
            if (selectCard != null)
            {
                selectCard.IsSelected = false;
            }
            pictureBox_backdrop.Image = Image.FromStream(new MemoryStream(File.ReadAllBytes(card.Movie.Backdrop)));
            textBox_year.Text = card.Movie.Year;
            textBox_publisher.Text = card.Movie.Publisher;
            textBox_studio.Text = card.Movie.Studio;
            textBox_direct.Text = card.Movie.Direct;
            textBox_series.Text = card.Movie.Series;
            textBox_genre.Text = string.Join("，", card.Movie.Genre);
            textBox_star.Text = string.Join("，", card.Movie.Star);
            linkLabel_website.Text = card.Movie.WebSite;
            selectCard = card;
            selectCard.IsSelected = true;
        }

        /// <summary>
        /// 显示Toast提示
        /// </summary>
        /// <param name="str">内容</param>
        private void ShowToast(string str)
        {
            ToastNotification.Show(this, str);
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="value">进度</param>
        private void UpdateProgress(int value)
        {
            Invoke(new EventHandler((o, e) =>
            {
                progressBar1.Text = $"已处理 {value}% ...";
                progressBar1.Value = value;
            }));
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="str">内容</param>
        private void LogAdd(string str, LogLevels logLevel = LogLevels.Out)
        {
            var color = logLevel == LogLevels.Out ? "Black" : logLevel == LogLevels.Error ? "Red" : logLevel == LogLevels.Waining ? "Orange" : "Green";
            var logState = logLevel == LogLevels.Out ? "提示" : logLevel == LogLevels.Error ? "错误" : logLevel == LogLevels.Waining ? "警告" : "成功";
            Invoke(new EventHandler((o, e) =>
            {
                richTextBox1.SelectionColor = ColorTranslator.FromHtml(color);
                richTextBox1.AppendText($"{DateTime.Now:yyyy/MM/dd HH:mm:ss} [{logState}] {str}\n");
                richTextBox1.ScrollToCaret();
            }));
        }

        /// <summary>
        /// 更新界面状态
        /// </summary>
        /// <param name="isStart">是否开始状态</param>
        private void UpdateUI(bool isStart)
        {
            Invoke(new EventHandler((o, e) =>
            {
                if (!isStart)
                {
                    progressBar1.Value = 0;
                }
                groupPanel1.Enabled = !isStart;
                groupPanel2.Enabled = !isStart;
                groupPanel3.Enabled = !isStart;
                groupPanel4.Enabled = !isStart;
                groupPanel5.Enabled = !isStart;
                panelEx1.Enabled = !isStart;
                button_start.Text = isStart ? "停止整理" : "开始整理";
                button_start.Enabled = true;
                progressBar1.Visible = false;
                ShowToast(isStart ? "开始整理" : "整理完成");
                IsRun = isStart;
            }));
        }

        #region 设置自动保存

        private void radioButton_aiclip_CheckedChanged(object sender, EventArgs e)
        {
            textBox_apikey.Enabled = radioButton_aiclip.Checked;
            textBox_secretkey.Enabled = radioButton_aiclip.Checked;
            labelX1.Enabled = radioButton_aiclip.Checked;
            labelX2.Enabled = radioButton_aiclip.Checked;
            config.AIClip = radioButton_aiclip.Checked;
            SaveConfig();
        }

        private void button_titledefault_Click(object sender, EventArgs e)
        {
            textBox_titletemplate.Text = "$番号-$标题";
        }

        private void button_pathdefault_Click(object sender, EventArgs e)
        {
            textBox_pathtemplate.Text = "$番号 $标题";
        }

        private void comboBox_site_SelectedIndexChanged(object sender, EventArgs e)
        {
            config.Site = comboBox_site.SelectedIndex;
            SaveConfig();
        }

        private void textBox_labrarypath_TextChanged(object sender, EventArgs e)
        {
            config.LibraryPath = textBox_librarypath.Text;
            SaveConfig();
        }

        private void checkBox_createnfo_CheckedChanged(object sender, EventArgs e)
        {
            config.CreateNfo = checkBox_createnfo.Checked;
            SaveConfig();
        }

        private void textBox_titletemplate_TextChanged(object sender, EventArgs e)
        {
            config.TitleTemplate = textBox_titletemplate.Text;
            SaveConfig();
        }

        private void textBox_pathtemplate_TextChanged(object sender, EventArgs e)
        {
            config.PathTemplate = textBox_pathtemplate.Text;
            SaveConfig();
        }

        private void textBox_apikey_TextChanged(object sender, EventArgs e)
        {
            config.BD_API_KEY = textBox_apikey.Text;
            SaveConfig();
        }

        private void textBox_secretkey_TextChanged(object sender, EventArgs e)
        {
            config.BD_SECRET_KEY = textBox_secretkey.Text;
            SaveConfig();
        }

        private void checkBox_imgnodown_CheckedChanged(object sender, EventArgs e)
        {
            config.SkipExistsImage = checkBox_imgnodown.Checked;
            SaveConfig();
        }

        private void checkBox_filternfo_CheckedChanged(object sender, EventArgs e)
        {
            config.SkipSearchExistsNfo = checkBox_filternfo.Checked;
            SaveConfig();
        }

        private void doubleInput_score_ValueChanged(object sender, EventArgs e)
        {
            config.AIScore = doubleInput_score.Value;
            SaveConfig();

        }

        private void checkBox_noai_CheckedChanged(object sender, EventArgs e)
        {
            config.CensoredNoAI = checkBox_noai.Checked;
            SaveConfig();
        }
        #endregion

        private void button_next_Click(object sender, EventArgs e)
        {
            if (libPage <= libFiles.Count / 12 + 1)
            {
                libPage++;
                AddCard();
            }
        }

        private void button_prev_Click(object sender, EventArgs e)
        {
            if (libPage - 1 >= 0)
            {
                libPage--;
                AddCard();
            }
        }
    }
}
