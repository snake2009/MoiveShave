using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace 老司机影片整理
{
    public partial class InfoCard : UserControl
    {
        private Image _cover;
        private Font font1 = new Font("微软雅黑", 10.7F, FontStyle.Bold);
        private Font font2 = new Font("微软雅黑", 9F);
        private StringFormat format = new StringFormat();
        private MovieInfo _movie;
        private bool _IsSelected;

        public MovieInfo Movie
        {
            set
            {
                if (!string.IsNullOrEmpty(value.Cover))
                {
                    _cover = Image.FromStream(new MemoryStream(File.ReadAllBytes(value.Cover)));
                }
                _movie = value;
            }
            get
            {
                return _movie;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                Invalidate();
            }
        }

        internal VideoInfo VideoFile { get; set; }

        public InfoCard()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            format.Alignment = StringAlignment.Center;
        }

        private void InfoCard_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            if (_IsSelected)
            {
                g.FillRectangle(Brushes.DarkGray, new Rectangle(10, 8, Width, Height));
                g.FillRectangle(Brushes.AliceBlue, 5, 0, Width - 11, Height - 7);
                g.DrawRectangle(Pens.CornflowerBlue, 5, 0, Width - 11, Height - 7);
            }
            else
            {
                g.FillRectangle(Brushes.White, new RectangleF(5, 0, Width - 11, Height - 7));
                g.DrawRectangle(Pens.Black, 5, 0, Width - 11, Height - 7);
            }
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            var width = Width - 12;
            if (_cover != null)
            {
                g.DrawImage(_cover, new RectangleF(6, 1, width, width / 0.7f), new Rectangle(0, 0, _cover.Width, _cover.Height), GraphicsUnit.Pixel);
            }
            if (!string.IsNullOrEmpty(Movie.Number))
            {
                g.DrawString(_movie.Number, font1, Brushes.Blue, new RectangleF(5, Height - 96, width - 10, 30), format);
            }
            if (!string.IsNullOrEmpty(Movie.Title))
            {
                g.DrawString(_movie.Title, font2, new SolidBrush(SystemColors.ControlDarkDark), new RectangleF(5, Height - 60, width - 10, 48), format);
            }
            if (!string.IsNullOrEmpty(Movie.Attr))
            {
                var rectangle = new RectangleF(Width - 65, 1, 58, 30);
                g.FillRectangle(new SolidBrush(Color.FromArgb(180, 0, 0, 0)), rectangle);
                g.DrawString("CD" + _movie.Attr, font1, Brushes.Yellow, rectangle, format);
            }
        }
    }
}
