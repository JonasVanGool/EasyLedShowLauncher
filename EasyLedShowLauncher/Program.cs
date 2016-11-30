using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyLedShowLauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                if (args[0].EndsWith(".els"))
                {
                    Application.Run(new EasyLedShowLauncher(args[0]));
                }
                else
                {
                    MessageBox.Show("Invalid file type");
                }
            }
            else
            {
                Application.Run(new EasyLedShowLauncher());
            }         
        }
    }
}
