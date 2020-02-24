using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 老司机影片整理
{
    public partial class MainForm : Form
    {
        private bool IsRun;
        private List<string> Notkeys;
        private Config config = new Config();
        private string configPath;

        public MainForm()
        {
            InitializeComponent();
            Notkeys = new List<string>();
            LoadConfig();
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
                textBox_labrarypath.Text = config.LabraryPath;
                textBox_titletemplate.Text = config.TitleTemplate;
                textBox_pathtemplate.Text = config.PathTemplate;
                radioButton_clip.Checked = !config.AIClip;
                radioButton_aiclip.Checked = config.AIClip;
                textBox_apikey.Text = config.BD_API_KEY;
                textBox_secretkey.Text = config.BD_SECRET_KEY;
                numericUpDown_maxthread.Value = config.MaxThread;
            }
            else
            {
                comboBox_site.SelectedIndex = 0;
                pathListBox.Items.Add(@"F:\剑客\东洋\测试");
                textBox_labrarypath.Text = @"F:\剑客\东洋\测试";
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
            //绑定FileProcess事件
            FileProcess.OnProgress += (value) =>
            {
                UpdateProgress(value);
            };
            FileProcess.OnLog += (str, type, color) =>
            {
                LogAdd(str, type, color);
            };
            FileProcess.OnComplete += () =>
            {
                //更新界面为停止运行
                UpdateUI(false);
                toolStripStatusLabel1.Text = "整理完成";
                IsRun = false;
            };

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
                pathListBox.Items.Add(fbd.SelectedPath);
            }
            fbd.Dispose();
        }

        private void button_clearpath_Click(object sender, EventArgs e)
        {
            pathListBox.Items.Clear();
        }

        private void button_selectlabrarypath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBox_labrarypath.Text = fbd.SelectedPath;
            }
            fbd.Dispose();
        }

        private void radioButton_aiclip_CheckedChanged(object sender, EventArgs e)
        {
            textBox_apikey.Enabled = radioButton_aiclip.Checked;
            textBox_secretkey.Enabled = radioButton_aiclip.Checked;
            label4.Enabled = radioButton_aiclip.Checked;
            label5.Enabled = radioButton_aiclip.Checked;
            if (radioButton_aiclip.Checked)
            {
                numericUpDown_maxthread.Maximum = 2;
            }
            else
            {
                numericUpDown_maxthread.Maximum = 10;
            }
            config.AIClip = radioButton_aiclip.Checked;
            SaveConfig();
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

        #endregion

        /// <summary>
        /// 开始处理
        /// </summary>
        private void Start()
        {
            if (IsRun)
            {
                FileProcess.Stop();
                toolStripStatusLabel1.Text = "手动停止中...";
                button_start.Enabled = false;
                return;
            }
            if (pathListBox.Items.Count > 0)
            {
                IsRun = true;
                //整理配置
                config.Site = comboBox_site.SelectedIndex;
                config.PathTemplate = textBox_pathtemplate.Text;
                config.TitleTemplate = textBox_titletemplate.Text;
                config.LabraryPath = textBox_labrarypath.Text;
                config.AIClip = radioButton_aiclip.Checked;
                config.CreateNfo = checkBox_createnfo.Checked;
                config.MaxThread = (int)numericUpDown_maxthread.Value;
                //创建媒体库文件夹
                if (!Directory.Exists(config.LabraryPath))
                {
                    Directory.CreateDirectory(config.LabraryPath);
                }
                //设置进度条为流水效果
                progressBar1.Style = ProgressBarStyle.Marquee;
                //准备路径数组
                List<string> list = new List<string>();
                foreach (string item in (pathListBox.Items as ListBox.ObjectCollection))
                {
                    //检查用户添加是否是文件夹
                    if (Directory.Exists(item))
                    {
                        //获取目录下面的视频文件
                        var top_files = Directory.EnumerateFiles(item, "*.*", SearchOption.AllDirectories)
                            .Where(f =>
                            f.EndsWith(".mkv", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".mp4", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".avi", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".asf", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".wmv", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".rmvb", StringComparison.CurrentCultureIgnoreCase) ||
                            f.EndsWith(".rm", StringComparison.CurrentCultureIgnoreCase));
                        list.AddRange(top_files);
                    }
                }
                //过滤已经存在nfo的视频
                if (checkBox_filternfo.Checked)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var item = list[i];
                        var name = $"{Path.GetDirectoryName(item)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(item)}.nfo";
                        if (File.Exists(name))
                        {
                            list.RemoveAt(i);
                            i--;
                        }
                    }
                }
                //没有需要整理的文件
                if (list.Count == 0)
                {
                    IsRun = false;
                    //设置进度条为普通效果
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    toolStripStatusLabel1.Text = "没有需要处理的视频文件";
                    return;
                }
                LogAdd($"共搜索到 {list.Count} 个视频文件");

                toolStripStatusLabel1.Text = "开始处理中...";

                //开始整理
                FileProcess.Start(list, config);

                progressBar1.Style = ProgressBarStyle.Blocks;

                //没找到文件的时候FileProcess会直接触发结束事件，优先执行，导致这里覆盖了UI状态
                if (IsRun)
                {
                    //更新界面为开始运行
                    UpdateUI(true);
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "请先添加待整理的影片目录";
            }
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="value">进度</param>
        private void UpdateProgress(int value)
        {
            Invoke(new EventHandler((o, e) =>
            {
                toolStripStatusLabel1.Text = $"已处理 {value}% ...";
                progressBar1.Value = value;
            }));
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="str">内容</param>
        private void LogAdd(string str, string type = "tabPage_main", string color = "Black")
        {
            Invoke(new EventHandler((o, e) =>
            {
                if (tabControl1.TabPages[type] == null)
                {
                    TabPage page = new TabPage()
                    {
                        Name = type,
                        Text = type
                    };
                    RichTextBox textBox = new RichTextBox()
                    {
                        Dock = DockStyle.Fill,
                        WordWrap = false,
                        BorderStyle = BorderStyle.None
                    };
                    page.Controls.Add(textBox);
                    tabControl1.TabPages.Add(page);
                }
                var richTextBox = tabControl1.TabPages[type].Controls[0] as RichTextBox;
                richTextBox.SelectionColor = type == "tabPage_main" ? Color.Red:ColorTranslator.FromHtml(color);
                richTextBox.AppendText(str + "\n");
                richTextBox.ScrollToCaret();
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
                groupBox1.Enabled = !isStart;
                groupBox2.Enabled = !isStart;
                button_start.Text = isStart ? "停止整理" : "开始整理";
                button_start.Enabled = true;
                IsRun = isStart;
            }));
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
            config.LabraryPath = textBox_labrarypath.Text;
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

        private void numericUpDown_maxthread_ValueChanged(object sender, EventArgs e)
        {
            config.MaxThread = (int)numericUpDown_maxthread.Value;
            SaveConfig();
        }
    }
}
