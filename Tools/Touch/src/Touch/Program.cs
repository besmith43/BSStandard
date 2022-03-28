using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;

namespace Touch
{
    class Program
    {
        static void Main(string[] args)
        {
            var results = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run);
        }

        public static void Run(Options opts)
        {
            Parallel.ForEach(opts.FileSeq, (file) => {
                string currentPath = Environment.CurrentDirectory;
                string FullPathForNewFile = Path.Combine(currentPath, file);
                File.Create(FullPathForNewFile);
            });
        }

        public class Options
        {
            [Value(0)]
            public IEnumerable<string> FileSeq { get; set; }
        }
    }
}
