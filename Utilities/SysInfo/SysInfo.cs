using System;

namespace BSStandard.Utilities
{
    public class SysInfo
    {
        public static bool IsWindows() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);

        public static bool IsMacOS() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);

        public static bool IsLinux() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

        public static string GetCPU()
        {
            string CPUInfo = null;

            if (IsWindows())
            {
                CPUInfo = GetCPUWindows();
            }
            else if (IsLinux())
            {
                CPUInfo = GetCPULinux();
            }
            else if (IsMacOS())
            {
                CPUInfo = GetCPUMacOS();
            }
            else
            {
                CPUInfo = "Unknown Operating System";
            }

            return CPUInfo;
        }

        private static string GetCPUWindows()
        {
            ManagementObjectSearcher myProcessorObject = new System.Management.ManagementObjectSearcher("select * from Win32_Processor");

            StringBuilder cpuString = new System.Text.StringBuilder(string.Empty);

            foreach (ManagementObject obj in myProcessorObject.Get())
            {
                cpuString.AppendFormat("Name  -  " + obj["Name"]);
                cpuString.AppendFormat("DeviceID  -  " + obj["DeviceID"]);
                cpuString.AppendFormat("Manufacturer  -  " + obj["Manufacturer"]);
                cpuString.AppendFormat("CurrentClockSpeed  -  " + obj["CurrentClockSpeed"]);
                cpuString.AppendFormat("Caption  -  " + obj["Caption"]);
                cpuString.AppendFormat("NumberOfCores  -  " + obj["NumberOfCores"]);
                cpuString.AppendFormat("NumberOfEnabledCore  -  " + obj["NumberOfEnabledCore"]);
                cpuString.AppendFormat("NumberOfLogicalProcessors  -  " + obj["NumberOfLogicalProcessors"]);
                cpuString.AppendFormat("Architecture  -  " + obj["Architecture"]);
                cpuString.AppendFormat("Family  -  " + obj["Family"]);
                cpuString.AppendFormat("ProcessorType  -  " + obj["ProcessorType"]);
                cpuString.AppendFormat("Characteristics  -  " + obj["Characteristics"]);
                cpuString.AppendFormat("AddressWidth  -  " + obj["AddressWidth"]);
            }

            return cpuString;
        }

        private static string GetCPULinux()
        {
            return "Not Implemented Yet";
        }

        private static string GetCPUMacOS()
        {
            return "Not Implemented Yet";
        }
    }
}
