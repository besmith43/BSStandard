using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShellProgressBar;

namespace Copy.Class
{
    public static class Class1
    {
        public static List<string> errorPaths = new();
        public static ConsoleColor originalColor = Console.ForegroundColor;
        private static int progBarTotal;
        private static int currTotal = 0;
        private static ProgressBar pbar;

        public static void CopyFile(string localSource, string localDestination)
        {
                string name = Path.GetFileName(localSource);
                string dest = Path.Combine(localDestination, name);
                File.Copy(localSource, dest);  // need error checking that the file doesn't already exist
        }

        public static List<string> CopyDirectory(string localSource, string localDestination, Action<string, int, int> update)
        {
            progBarTotal = CountItems(localSource);

            if (!Directory.Exists(localDestination))
            {
                Directory.CreateDirectory(localDestination);
            }

            DirectoryInfo sourceDirInfo = new DirectoryInfo(localSource);
            string newDestination = Path.Join(localDestination, sourceDirInfo.Name);
            Directory.CreateDirectory(newDestination);

            CopyFolder(localSource, newDestination, update);

            return errorPaths;
        }

        public static void CopyDirectory(string localSource, string localDestination)
        {
            progBarTotal = CountItems(localSource);
            var options = new ProgressBarOptions
            {
                ForegroundColor = ConsoleColor.Yellow,
                ForegroundColorDone = ConsoleColor.DarkGreen,
                BackgroundColor = ConsoleColor.DarkGray,
                BackgroundCharacter = '\u2593'
            };
            pbar = new ProgressBar(progBarTotal, "Prepping to copy your files and folders", options);

            CopyFolder(localSource, localDestination, updateProgrBar);

            Console.Clear();

            if (errorPaths.Count > 0)
            {
                Console.Error.WriteLine("The following paths were not copied: ");
                foreach(string epath in errorPaths)
                {
                    Console.Error.WriteLine(epath);
                }
            }

            Console.ForegroundColor = originalColor;
        }

        private static int CountItems(string path)
        {
            if (File.Exists(path))
            {
                return 1;
            }
            else if (Directory.Exists(path))
            {
                int total = 0;

                try
                {
                    string[] files = Directory.GetFiles(path);

                    string[] folders = Directory.GetDirectories(path);

                    total = files.Length;

                    foreach (string folder in folders)
                    {
                        total = total + CountItems(folder);
                    }
                }
                catch
                {
                    errorPaths.Add(path);
                }

                return total;
            }
            else
            {
                return 0;
            }
        }

        private static void CopyFolder(string sourceFolder, string destFolder, Action<string, int, int> update = null)
        {
            string[] files;
            string[] folders;

            try
            {
                files = Directory.GetFiles(sourceFolder);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return;
            }

            try
            {
                folders = Directory.GetDirectories(sourceFolder);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return;
            }

            if (!Directory.Exists( destFolder ))
            {
                try
                {
                    Directory.CreateDirectory( destFolder );
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    return;
                }
            }

            // copy source files
            Parallel.ForEach (files, (file) => {
                string name = Path.GetFileName( file );
                string dest = Path.Combine( destFolder, name );
                currTotal += 1;

                if (update != null)
                {
                    update($"Copying { file } to { destFolder }", progBarTotal, currTotal);
                }

                try
                {
                    File.Copy( file, dest );
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    errorPaths.Add(file);
                }
            });

            // copy source directory structure
            foreach (string folder in folders)
            {
                string name = Path.GetFileName( folder );
                string dest = Path.Combine( destFolder, name );
                CopyFolder( folder, dest, update);  // this is recursive... need to look into a different way to handle it
            }
        }

        public static void updateProgrBar(string msg, int total, int num_completed)
        {
            pbar.Tick(msg);
        }
    }
}
