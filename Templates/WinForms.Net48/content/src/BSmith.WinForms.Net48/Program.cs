using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BSmith.WinForms.Net48.cmd;

namespace BSmith.WinForms.Net48
{
    static class Program
    {
        public static Options cmdFlags;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            cmdParser cmdP = new cmdParser(args);

            cmdFlags = cmdP.Parse();  

            if (cmdFlags.versionFlag)
            {
                RuntimeMessage(cmdFlags.versionText, "Version");
                return;
            }

            if (cmdFlags.helpFlag)
            {
                RuntimeMessage(cmdFlags.helpText, "Help");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static void RuntimeMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
