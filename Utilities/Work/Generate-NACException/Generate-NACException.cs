using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace BSStandard.Utilities.Work
{
    public class GenerateNACException
    {
        public string ComputerRun(string SavePath = null)
        {
            string hostname = Environment.MachineName.ToUpper();

            GenerateInfo info = new GenerateInfo(hostname);
            string csvContent = info.StartGenerateInfo();

            if (csvContent.Equals("no ethernet mac addresses found") || csvContent.Equals("non-standard OS"))
            {
                return csvContent;
            }

            return SaveContentToFile(csvContent, hostname, SavePath);
        }

        public void PrinterIPRun()
        {

        }

        public void PrinterMACRun()
        {

        }

        // see http://codebuckets.com/2017/10/19/getting-the-root-directory-path-for-net-core-applications/ for original text
        public static string GetApplicationRootDebug()
        {
            return Environment.CurrentDirectory;
        }

        public static string GetApplicationRootRelease()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        }

        public static string SaveContentToFile(string content, string devicename, string GivenPath = null)
        {
            string path = "";
            string FileName = "";

            if (GivenPath != null)
            {
                path = GivenPath;
            }
            else
            {
                #if DEBUG
                    path = GetApplicationRootDebug();
                #else
                    path = GetApplicationRootRelease();
                #endif
            }
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FileName = $"{ path }\\{ GenerateFileName(devicename) }";
            }
            else
            {
                FileName = $"{ path }/{ GenerateFileName(devicename) }";
            }

            if (!File.Exists(FileName))
            {
                try
                {
                	using(StreamWriter sw = File.CreateText(FileName))
                	{
                    		sw.WriteLine(content);
                	}
		        }
                catch
                {
                    return $"Failed: Couldn't write to path: { FileName }";
                }
            }
            else
            {
                return $"Failed: { FileName } already exists";
            }

            return "Successful";
        }

        public static string GenerateFileName(string host)
        {
            string FormatedDate = $"{ DateTime.Today.ToString("d").Replace("/","") }";

            return $"{ FormatedDate }-{ host }.csv";
        }
    }
}
