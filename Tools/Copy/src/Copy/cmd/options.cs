using System;
using System.Text;
using System.Reflection;

#nullable enable

namespace Copy.cmd
{
    public class Options
    {
        public string source { get; set; }
        public string destination { get; set; }
        public bool helpFlag { get; set; }
        public string helpText { get; set; }
        public bool versionFlag { get; set; }
        public string versionText { get; set; }
        public bool verbose { get; set; }

        public Options()
        {
            helpFlag = false;
            versionFlag = false;
            versionText = $"Copy { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }";
            verbose = false;
            StringBuilder helpTextBuilder = new StringBuilder();
            helpTextBuilder.AppendLine($"{ versionText }");
            helpTextBuilder.AppendLine("Usage:  Copy [OPTION] <source> <destination> ");
            helpTextBuilder.AppendLine("");
            helpTextBuilder.AppendLine("    -V | --verbose          Set verbose mode");
            helpTextBuilder.AppendLine("    -v | --version          Display version message");
            helpTextBuilder.AppendLine("    -h | --help             Display this help message");
            helpTextBuilder.AppendLine("");
            helpText = helpTextBuilder.ToString();
        }
    }
}