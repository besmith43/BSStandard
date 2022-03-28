using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using DiscUtils.Iso9660;
using iso_builder.cmd;

namespace iso_builder
{
    class Program
    {
        public static Options cmdFlags;
        static void Main(string[] args)
        {
            cmdParser cmdP = new(args);

            cmdFlags = cmdP.Parse();  

            if (cmdFlags.versionFlag)
            {
                Console.WriteLine(cmdFlags.versionText);
                return;
            }

            if (cmdFlags.helpFlag)
            {
                Console.WriteLine(cmdFlags.helpText);
                return;
            }

            Run();
        }

        public static void Run()
        {
            int buildResult = BuildIso(cmdFlags.folder, cmdFlags.output);
        }

        private static Dictionary<string, string> getFileList(DirectoryInfo folder, DirectoryInfo home)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            CDBuilder builder = new CDBuilder();

            foreach (FileInfo file in folder.GetFiles())
            {
                string fileFullPath = file.FullName;
                string fileOnIso = fileFullPath.TrimStart(home.FullName.ToCharArray());
                output.Add(fileOnIso, file.FullName);
            }

            // Now do it for all subfolders
            foreach (DirectoryInfo directory in folder.GetDirectories())
            {
                getFileList(directory, home).ToList().ForEach(file => output.Add(file.Key, file.Value));
            }

            return output;
        }

        private static int BuildIso(DirectoryInfo sourceDirectory, string targetFile)
        {
            CDBuilder builder = new CDBuilder();
            Dictionary<string, string> resultList = new Dictionary<string, string>();

            try
            {
                // Get main folder and put it into results.
                getFileList(sourceDirectory, sourceDirectory).ToList().ForEach(file => resultList.Add(file.Key, file.Value));

                // Finally, add all files collected to the ISO.
                foreach (KeyValuePair<string, string> pair in resultList.ToList())
                {
                    builder.AddFile(pair.Key, pair.Value);
                }

                builder.Build(targetFile);
            } catch(Exception e)
            {
                Console.WriteLine("Error Writing ISO. Check Permissions and Files. " + e.Message);
                return 1;
            }
            
            return 0;
        }
    }
}
