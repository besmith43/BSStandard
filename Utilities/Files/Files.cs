using System;

namespace BSStandard.Utilities
{
    public class Files
    {
        private static void WriteToFile(string content, string FullFilename, bool verboseFlag = false)
        {
            if (!System.IO.File.Exists(FullFilename))
            {
                try
                {
                	using(System.IO.StreamWriter sw = System.IO.File.CreateText(FullFilename))
                	{
                    		sw.WriteLine(content);
                	}
		        }
                catch
                {
                    Console.WriteLine($"Couldn't write to path: { FullFilename }");
                }
            }
            else
            {
                bool Answer = Prompt.GetYesNo($"CSV already exists.{ System.Environment.NewLine }Would you like to replace it?", true);

                if(Answer)
                {
                    System.IO.File.Delete(FullFilename);
                    using (System.IO.StreamWriter sw = System.IO.File.CreateText(FullFilename))
                    {
                        sw.WriteLine(content);
                    }
                }
            }

            if (verboseFlag)
            {
                Console.WriteLine($"File Path: { FullFilename }");
                Console.WriteLine($"File Content: { content }");
            }
        }

        public static void SaveContentToFile(string content, string filename, bool verboseFlag = false)
        {
            string path = "";
            string FileName = "";

            if (OperatingSystem.IsWindows())
            {
                #if DEBUG
                    path = AppRoot.GetApplicationRootDebug();
                #else
                    path = AppRoot.GetApplicationRootRelease();
                #endif

                FileName = $"{ path }\\{ filename }";
            }
            else
            {
                #if DEBUG
                    path = AppRoot.GetApplicationRootDebug();
                #else
                    path = AppRoot.GetApplicationRootRelease();
                #endif

                FileName = $"{ path }/{ filename }";
            }

            WriteToFile(content, FileName, verboseFlag);
        }

        public static void SaveContentToFile(string content, string filename, string path, bool verboseFlag = false)
        {
            if (OperatingSystem.IsWindows())
            {
                WriteToFile(content, $"{ path }\\{ filename }", verboseFlag);
            }
            else
            {
                WriteToFile(content, $"{ path }/{ filename }", verboseFlag);
            }
        }
    }
}
