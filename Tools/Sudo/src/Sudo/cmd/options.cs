using System;
using System.Text;
using System.Reflection;

namespace Sudo.cmd
{
    public class Options
    {
        public bool helpFlag { get; set; }
        public string helpText { get; set; }
        public bool versionFlag { get; set; }
        public string versionText { get; set; }
        public string command { get; set; }
		public string commandArgs { get; set; }

        public Options()
        {
            helpFlag = false;
            versionFlag = false;
            versionText = $"Sudo { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }";
            StringBuilder helpTextBuilder = new StringBuilder();
            helpTextBuilder.AppendLine($"{ versionText }");
            helpTextBuilder.AppendLine("Usage:  Sudo <Command>  ");
            helpTextBuilder.AppendLine("");
            helpTextBuilder.AppendLine("    -v | --version          Display version message");
            helpTextBuilder.AppendLine("    -h | --help             Display this help message");
            helpTextBuilder.AppendLine("");
            helpText = helpTextBuilder.ToString();

            command = null;
        }
    }
}