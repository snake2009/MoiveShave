namespace 老司机影片整理
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button_start = new System.Windows.Forms.Button();
            this.pathListBox = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.button_addpath = new System.Windows.Forms.Button();
            this.button_clearpath = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_pathdefault = new System.Windows.Forms.Button();
            this.button_titledefault = new System.Windows.Forms.Button();
            this.radioButton_aiclip = new System.Windows.Forms.RadioButton();
            this.radioButton_clip = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_titletemplate = new System.Windows.Forms.TextBox();
            this.textBox_secretkey = new System.Windows.Forms.TextBox();
            this.textBox_apikey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_maxthread = new System.Windows.Forms.NumericUpDown();
            this.checkBox_createnfo = new System.Windows.Forms.CheckBox();
            this.textBox_pathtemplate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_selectlabrarypath = new System.Windows.Forms.Button();
            this.textBox_labrarypath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_site = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox_filternfo = new System.Windows.Forms.CheckBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_main = new System.Windows.Forms.TabPage();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxthread)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_start.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_start.Location = new System.Drawing.Point(768, 125);
            this.button_start.Margin = new System.Windows.Forms.Padding(4);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(182, 40);
            this.button_start.TabIndex = 0;
            this.button_start.Text = "开始整理";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // pathListBox
            // 
            this.pathListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pathListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pathListBox.FormattingEnabled = true;
            this.pathListBox.ItemHeight = 24;
            this.pathListBox.Location = new System.Drawing.Point(11, 31);
            this.pathListBox.Margin = new System.Windows.Forms.Padding(4);
            this.pathListBox.Name = "pathListBox";
            this.pathListBox.Size = new System.Drawing.Size(729, 122);
            this.pathListBox.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.progressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 749);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(977, 31);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(10, 4, 0, 3);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(540, 24);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar1
            // 
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(400, 23);
            // 
            // button_addpath
            // 
            this.button_addpath.ForeColor = System.Drawing.Color.Blue;
            this.button_addpath.Location = new System.Drawing.Point(752, 31);
            this.button_addpath.Name = "button_addpath";
            this.button_addpath.Size = new System.Drawing.Size(86, 34);
            this.button_addpath.TabIndex = 7;
            this.button_addpath.Text = "添加";
            this.button_addpath.UseVisualStyleBackColor = true;
            this.button_addpath.Click += new System.EventHandler(this.button_addpath_Click);
            // 
            // button_clearpath
            // 
            this.button_clearpath.ForeColor = System.Drawing.Color.Red;
            this.button_clearpath.Location = new System.Drawing.Point(848, 31);
            this.button_clearpath.Name = "button_clearpath";
            this.button_clearpath.Size = new System.Drawing.Size(86, 34);
            this.button_clearpath.TabIndex = 8;
            this.button_clearpath.Text = "清空";
            this.button_clearpath.UseVisualStyleBackColor = true;
            this.button_clearpath.Click += new System.EventHandler(this.button_clearpath_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button_pathdefault);
            this.groupBox1.Controls.Add(this.button_titledefault);
            this.groupBox1.Controls.Add(this.radioButton_aiclip);
            this.groupBox1.Controls.Add(this.radioButton_clip);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_titletemplate);
            this.groupBox1.Controls.Add(this.textBox_secretkey);
            this.groupBox1.Controls.Add(this.textBox_apikey);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDown_maxthread);
            this.groupBox1.Controls.Add(this.checkBox_createnfo);
            this.groupBox1.Controls.Add(this.textBox_pathtemplate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button_selectlabrarypath);
            this.groupBox1.Controls.Add(this.textBox_labrarypath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox_site);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(13, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(952, 287);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "选项";
            // 
            // button_pathdefault
            // 
            this.button_pathdefault.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.button_pathdefault.Location = new System.Drawing.Point(381, 141);
            this.button_pathdefault.Name = "button_pathdefault";
            this.button_pathdefault.Size = new System.Drawing.Size(62, 32);
            this.button_pathdefault.TabIndex = 31;
            this.button_pathdefault.Text = "默认";
            this.button_pathdefault.UseVisualStyleBackColor = true;
            this.button_pathdefault.Click += new System.EventHandler(this.button_pathdefault_Click);
            // 
            // button_titledefault
            // 
            this.button_titledefault.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.button_titledefault.Location = new System.Drawing.Point(874, 88);
            this.button_titledefault.Name = "button_titledefault";
            this.button_titledefault.Size = new System.Drawing.Size(62, 32);
            this.button_titledefault.TabIndex = 30;
            this.button_titledefault.Text = "默认";
            this.button_titledefault.UseVisualStyleBackColor = true;
            this.button_titledefault.Click += new System.EventHandler(this.button_titledefault_Click);
            // 
            // radioButton_aiclip
            // 
            this.radioButton_aiclip.AutoSize = true;
            this.radioButton_aiclip.Location = new System.Drawing.Point(744, 142);
            this.radioButton_aiclip.Name = "radioButton_aiclip";
            this.radioButton_aiclip.Size = new System.Drawing.Size(197, 28);
            this.radioButton_aiclip.TabIndex = 29;
            this.radioButton_aiclip.Text = "使用百度AI裁剪封面";
            this.radioButton_aiclip.UseVisualStyleBackColor = true;
            this.radioButton_aiclip.CheckedChanged += new System.EventHandler(this.radioButton_aiclip_CheckedChanged);
            // 
            // radioButton_clip
            // 
            this.radioButton_clip.AutoSize = true;
            this.radioButton_clip.Checked = true;
            this.radioButton_clip.Cursor = System.Windows.Forms.Cursors.Default;
            this.radioButton_clip.Location = new System.Drawing.Point(537, 142);
            this.radioButton_clip.Name = "radioButton_clip";
            this.radioButton_clip.Size = new System.Drawing.Size(143, 28);
            this.radioButton_clip.TabIndex = 28;
            this.radioButton_clip.TabStop = true;
            this.radioButton_clip.Text = "普通裁剪封面";
            this.radioButton_clip.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(453, 194);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 24);
            this.label5.TabIndex = 26;
            this.label5.Text = "secret_key";
            // 
            // textBox_titletemplate
            // 
            this.textBox_titletemplate.Location = new System.Drawing.Point(558, 89);
            this.textBox_titletemplate.Name = "textBox_titletemplate";
            this.textBox_titletemplate.Size = new System.Drawing.Size(310, 31);
            this.textBox_titletemplate.TabIndex = 22;
            this.textBox_titletemplate.Text = "$番号-$标题";
            this.textBox_titletemplate.Click += new System.EventHandler(this.textBox_titletemplate_Click);
            this.textBox_titletemplate.TextChanged += new System.EventHandler(this.textBox_titletemplate_TextChanged);
            // 
            // textBox_secretkey
            // 
            this.textBox_secretkey.Enabled = false;
            this.textBox_secretkey.Location = new System.Drawing.Point(558, 191);
            this.textBox_secretkey.Name = "textBox_secretkey";
            this.textBox_secretkey.Size = new System.Drawing.Size(378, 31);
            this.textBox_secretkey.TabIndex = 25;
            this.textBox_secretkey.TextChanged += new System.EventHandler(this.textBox_secretkey_TextChanged);
            // 
            // textBox_apikey
            // 
            this.textBox_apikey.Enabled = false;
            this.textBox_apikey.Location = new System.Drawing.Point(125, 191);
            this.textBox_apikey.Name = "textBox_apikey";
            this.textBox_apikey.Size = new System.Drawing.Size(250, 31);
            this.textBox_apikey.TabIndex = 23;
            this.textBox_apikey.TextChanged += new System.EventHandler(this.textBox_apikey_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(44, 194);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 24);
            this.label4.TabIndex = 24;
            this.label4.Text = "api_key";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(381, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(172, 24);
            this.label8.TabIndex = 21;
            this.label8.Text = "媒体库显示标题模版";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 244);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(355, 24);
            this.label1.TabIndex = 20;
            this.label1.Text = "并发处理数（1~10，启用AI裁剪时最大2）";
            // 
            // numericUpDown_maxthread
            // 
            this.numericUpDown_maxthread.Location = new System.Drawing.Point(382, 241);
            this.numericUpDown_maxthread.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_maxthread.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_maxthread.Name = "numericUpDown_maxthread";
            this.numericUpDown_maxthread.Size = new System.Drawing.Size(62, 31);
            this.numericUpDown_maxthread.TabIndex = 19;
            this.numericUpDown_maxthread.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_maxthread.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDown_maxthread.ValueChanged += new System.EventHandler(this.numericUpDown_maxthread_ValueChanged);
            // 
            // checkBox_createnfo
            // 
            this.checkBox_createnfo.AutoSize = true;
            this.checkBox_createnfo.Checked = true;
            this.checkBox_createnfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_createnfo.Location = new System.Drawing.Point(22, 91);
            this.checkBox_createnfo.Name = "checkBox_createnfo";
            this.checkBox_createnfo.Size = new System.Drawing.Size(190, 28);
            this.checkBox_createnfo.TabIndex = 18;
            this.checkBox_createnfo.Text = "生成媒体库nfo文件";
            this.checkBox_createnfo.UseVisualStyleBackColor = true;
            this.checkBox_createnfo.CheckedChanged += new System.EventHandler(this.checkBox_createnfo_CheckedChanged);
            // 
            // textBox_pathtemplate
            // 
            this.textBox_pathtemplate.Location = new System.Drawing.Point(125, 141);
            this.textBox_pathtemplate.Name = "textBox_pathtemplate";
            this.textBox_pathtemplate.Size = new System.Drawing.Size(250, 31);
            this.textBox_pathtemplate.TabIndex = 13;
            this.textBox_pathtemplate.Text = "$番号 $标题";
            this.textBox_pathtemplate.Click += new System.EventHandler(this.textBox_filenametemplate_Click);
            this.textBox_pathtemplate.TextChanged += new System.EventHandler(this.textBox_pathtemplate_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 24);
            this.label6.TabIndex = 12;
            this.label6.Text = "文件夹模版";
            // 
            // button_selectlabrarypath
            // 
            this.button_selectlabrarypath.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.button_selectlabrarypath.Location = new System.Drawing.Point(874, 36);
            this.button_selectlabrarypath.Name = "button_selectlabrarypath";
            this.button_selectlabrarypath.Size = new System.Drawing.Size(62, 32);
            this.button_selectlabrarypath.TabIndex = 11;
            this.button_selectlabrarypath.Text = "选择";
            this.button_selectlabrarypath.UseVisualStyleBackColor = true;
            this.button_selectlabrarypath.Click += new System.EventHandler(this.button_selectlabrarypath_Click);
            // 
            // textBox_labrarypath
            // 
            this.textBox_labrarypath.Location = new System.Drawing.Point(558, 36);
            this.textBox_labrarypath.Name = "textBox_labrarypath";
            this.textBox_labrarypath.ReadOnly = true;
            this.textBox_labrarypath.Size = new System.Drawing.Size(310, 31);
            this.textBox_labrarypath.TabIndex = 3;
            this.textBox_labrarypath.Text = "javmovie";
            this.textBox_labrarypath.TextChanged += new System.EventHandler(this.textBox_labrarypath_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(452, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 24);
            this.label3.TabIndex = 2;
            this.label3.Text = "媒体库位置";
            // 
            // comboBox_site
            // 
            this.comboBox_site.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_site.FormattingEnabled = true;
            this.comboBox_site.Items.AddRange(new object[] {
            "程序自动选择【推荐】",
            "javbus【有码+无码】",
            "javlibrary【有问题】",
            "avsox【无码】",
            "fc2club【fc2专用】",
            "avmoo【有码】",
            "javdb【有码+无码】"});
            this.comboBox_site.Location = new System.Drawing.Point(125, 36);
            this.comboBox_site.Name = "comboBox_site";
            this.comboBox_site.Size = new System.Drawing.Size(250, 32);
            this.comboBox_site.TabIndex = 1;
            this.comboBox_site.SelectedIndexChanged += new System.EventHandler(this.comboBox_site_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "信息采集源";
            // 
            // checkBox_filternfo
            // 
            this.checkBox_filternfo.AutoSize = true;
            this.checkBox_filternfo.Checked = true;
            this.checkBox_filternfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_filternfo.Location = new System.Drawing.Point(752, 76);
            this.checkBox_filternfo.Name = "checkBox_filternfo";
            this.checkBox_filternfo.Size = new System.Drawing.Size(190, 28);
            this.checkBox_filternfo.TabIndex = 22;
            this.checkBox_filternfo.Text = "跳过存在nfo的文件";
            this.checkBox_filternfo.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(942, 227);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.checkBox_filternfo);
            this.groupBox2.Controls.Add(this.pathListBox);
            this.groupBox2.Controls.Add(this.button_addpath);
            this.groupBox2.Controls.Add(this.button_clearpath);
            this.groupBox2.Location = new System.Drawing.Point(16, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(948, 164);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "待整理影片目录（支持添加多个）";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "老司机影片整理";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage_main);
            this.tabControl1.Location = new System.Drawing.Point(12, 473);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(956, 270);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage_main
            // 
            this.tabPage_main.Controls.Add(this.richTextBox1);
            this.tabPage_main.Location = new System.Drawing.Point(4, 33);
            this.tabPage_main.Name = "tabPage_main";
            this.tabPage_main.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_main.Size = new System.Drawing.Size(948, 233);
            this.tabPage_main.TabIndex = 0;
            this.tabPage_main.Text = "主任务";
            this.tabPage_main.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(977, 780);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "老司机影片整理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_maxthread)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_main.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.ListBox pathListBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar progressBar1;
        private System.Windows.Forms.Button button_addpath;
        private System.Windows.Forms.Button button_clearpath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_labrarypath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_site;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_selectlabrarypath;
        private System.Windows.Forms.TextBox textBox_pathtemplate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox_createnfo;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_main;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_maxthread;
        private System.Windows.Forms.CheckBox checkBox_filternfo;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBox_titletemplate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_apikey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_secretkey;
        private System.Windows.Forms.RadioButton radioButton_aiclip;
        private System.Windows.Forms.RadioButton radioButton_clip;
        private System.Windows.Forms.Button button_pathdefault;
        private System.Windows.Forms.Button button_titledefault;
    }
}

