using PictureManagerApp.src.Lib;
using System;
using System.Windows.Forms;

namespace PictureManagerApp.src
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var cfg = new Config();
            cfg.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            cfg.Save();
        }
    }
}
