using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WallPaperDemo
{
    static class Program
    {
        /// <summary>
        /// mutext互斥锁
        /// </summary>
        private static Mutex mutext = null;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            GlobalMutext();
            Application.Run(new Form1());
        }

        private static void GlobalMutext()
        {
            bool createdNew = false;
            string name = "WinformOneApp";
            try
            {
                mutext = new Mutex(initiallyOwned: false, name, out createdNew);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Thread.Sleep(1000);
                Environment.Exit(1);
            }
            if (createdNew)
            {
                Console.WriteLine("程序已启动");
                return;
            }
            MessageBox.Show("另一个窗口已在运行,不能重复运行.");
            Thread.Sleep(1000);
            Environment.Exit(1);
        }
    }
}
