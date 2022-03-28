using System;
using System.Text;
using System.Reflection;

#nullable enable

namespace Caffeine.cmd
{
    public class Options
    {
        public bool helpFlag { get; set; }
        public string helpText { get; set; }
        public bool versionFlag { get; set; }
        public string versionText { get; set; }
        public bool verbose { get; set; }
        public bool disableFlag { get; set; }

        public Options()
        {
            disableFlag = false;
            helpFlag = false;
            versionFlag = false;
            versionText = $"Caffeine { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }";
            verbose = false;
            StringBuilder helpTextBuilder = new StringBuilder();
            helpTextBuilder.AppendLine($"{ versionText }");
            helpTextBuilder.AppendLine("Usage:  Caffeine [OPTION]  ");
            helpTextBuilder.AppendLine("");
            helpTextBuilder.AppendLine("    --disable               Switch to start the application as disabled");
            helpTextBuilder.AppendLine("    -V | --verbose          Set verbose mode");
            helpTextBuilder.AppendLine("    -v | --version          Display version message");
            helpTextBuilder.AppendLine("    -h | --help             Display this help message");
            helpTextBuilder.AppendLine("");
            helpText = helpTextBuilder.ToString();
        }
    }
}