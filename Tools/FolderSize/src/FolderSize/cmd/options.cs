using System;
using System.Text;
using System.Reflection;

#nullable enable

namespace FolderSize.cmd
{
    public class Options
    {
        public bool helpFlag { get; set; }
        public string helpText { get; set; }
        public bool versionFlag { get; set; }
        public string versionText { get; set; }
        public bool verbose { get; set; }
        public string file { get; set; }
        public string folder { get; set; }

        public Options()
        {
            helpFlag = false;
            versionFlag = false;
            versionText = $"FolderSize { Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version }";
            verbose = false;
            StringBuilder helpTextBuilder = new StringBuilder();
            helpTextBuilder.AppendLine($"{ versionText }");
            helpTextBuilder.AppendLine("Usage:  FolderSize [OPTION]  <file or folder> ");
            helpTextBuilder.AppendLine("");
            helpTextBuilder.AppendLine("If no options are given, the folder that serves as the current working directory will be used");
            helpTextBuilder.AppendLine("");
            helpTextBuilder.AppendLine("    -f | --file <file>      (Optional) Pass in file to be checked for size");
            helpTextBuilder.AppendLine("    -r | --folder <folder>  (Optional) Pass in folder to be recurvsively checked for size");
            helpTextBuilder.AppendLine("    -V | --verbose          Set verbose mode");
            helpTextBuilder.AppendLine("    -v | --version          Display version message");
            helpTextBuilder.AppendLine("    -h | --help             Display this help message");
            helpTextBuilder.AppendLine("");
            helpText = helpTextBuilder.ToString();
        }
    }
}