using System;
using System.Windows.Forms;

namespace 老司机影片整理
{
    /// <summary>
    /// 日志级别
    /// </summary>
    enum LogLevels
    {
        Out = 0,
        Error = 1,
        Waining = 2,
        Success = 3
    }

    static class Program
    {
        internal static string VersionNum;
        internal static string VersionText = " 内测版";
        internal static string Verinfo = string.Empty;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Thread.CurrentThread.Name = "UI主线程";
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
            Application.ThreadException += (o, e) => {
                MessageBox.Show($"很抱歉！程序发生异常。({e.Exception.Message})");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            };
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                MessageBox.Show($"很抱歉！程序发生异常。({e.ExceptionObject})");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            };

            string[] verinfo = Application.ProductVersion.Split(".".ToCharArray());
            VersionNum = string.Format("v{0}.{1} Build {2}", verinfo[0], verinfo[1], verinfo[2]);
            Verinfo = string.Format("{0}{1}", VersionNum, VersionText);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
