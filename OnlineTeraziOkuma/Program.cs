using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineTeraziOkuma
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Control.CheckForIllegalCrossThreadCalls = false;
            if (args.Count() > 0)
            {
                
                //Process[] processCollection = Process.GetProcesses();
                //foreach (Process process in processCollection)
                //{
                //    if (process.ProcessName == "OnlineTeraziOkuma")
                //    {
                //       //MessageBox.Show(args[0])
                //       //process.Kill(); 
                //    }  
                //}
                Application.Run(new Form1(args[0]));
            }
            else
                Application.Run(new Form1(""));


        }
    }
}
