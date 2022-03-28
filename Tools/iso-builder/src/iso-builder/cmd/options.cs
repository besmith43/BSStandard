using System;
using System.Text;
using System.IO;
using System.Reflection;

#nullable enable

namespace iso_builder.cmd
{
    public class Options
    {
        public bool helpFlag { get; set; }
        public string helpText { get; set; }
        public bool versionFlag { get; set; }
        public string versionText { get; set; }
        public bool verbose { get; set; }
        public DirectoryInfo folder { get; set; }
        public string output { get; set; }

        public Options()
        {
            helpFlag = false;
            versionFlag = false;
            versionText = $"iso_builder { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }";
            verbose = false;
            StringBuilder helpTextBuilder = new StringBuilder();
            helpTextBuilder.AppendLine($"{ versionText }");
            helpTextBuilder.AppendLine("Usage:  iso_builder [OPTION]  ");
            helpTextBuilder.AppendLine("");
            helpTextBuilder.AppendLine("    -f | --folder           Set the path to the folder");
            helpTextBuilder.AppendLine("    -o | --output           Set the output path and filename");
            helpTextBuilder.AppendLine("");
            helpTextBuilder.AppendLine("    -V | --verbose          Set verbose mode");
            helpTextBuilder.AppendLine("    -v | --version          Display version message");
            helpTextBuilder.AppendLine("    -h | --help             Display this help message");
            helpTextBuilder.AppendLine("");
            helpText = helpTextBuilder.ToString();
        }
    }
}