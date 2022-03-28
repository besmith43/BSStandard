using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BSmith.WPF.Net48.cmd;

namespace BSmith.WPF.Net48
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Options cmdFlags;

        public MainWindow()
        {
            String[] args = App.Args;
            
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

            InitializeComponent();
        }

        public static void RuntimeMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
