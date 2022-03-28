using System;

namespace BSStandard.Utilities
{
    public class Path
    {
        public static string GetApplicationRootDebug()
        {
            return Environment.CurrentDirectory;
        }

        public static string GetApplicationRootRelease()
        {
            return System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }
    }
}
