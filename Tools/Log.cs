using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace 老司机影片整理
{
    public class Log
    {
        private static object m_Lock = new object();
        private static string logPath = $"{Application.StartupPath}{Path.DirectorySeparatorChar}/logs";
        public static void Save(string str)
        {
			try
			{
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);
                lock (m_Lock)
                {
                    using (StreamWriter sw = new StreamWriter($@"{logPath}{Path.DirectorySeparatorChar}log{DateTime.Now:yyyyMMdd}.log", true, Encoding.Default))
                    {
                        sw.WriteLine($"{DateTime.Now:yyyyMMdd HH:mm:ss} {str}");
                        sw.Flush();
                    }
                }
            }
			catch (Exception)
			{

			}
        }
    }
}
