using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using McMaster.Extensions.CommandLineUtils;

namespace BSStandard.Utilities.Work
{
    public class GenerateInfo
    {
        private static string headerInfo = "adap.mac,siblings,host.host,adap.loc,host.devType,host.expireDate,host.inact";
        private static string genericInfo = "Never,1825 Days";
        private static string OS;
        private static List<string> MACInfo;
        private static string hostname;
        private static string roomLocation;

        public GenerateInfo(string passedHostName)
        {
            hostname = passedHostName;
            MACInfo = new List<string>();
        }

        public string StartGenerateInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                GenerateMACInfoWin();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                GenerateMACInfoLinux();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                GenerateMACInfoOSX();
            }
            else
            {
                return "non-standard OS";
            }

            // get hostname
            // see https://docs.microsoft.com/en-us/dotnet/api/system.environment.machinename?view=netcore-3.0

            roomLocation = "";

            try
            {
                roomLocation = hostname.Remove(hostname.LastIndexOf('-'), 4);
            }
            catch
            {
                roomLocation = Environment.MachineName;
                roomLocation = roomLocation.ToUpper();
            }

            if(MACInfo.Count > 0)
            {
                return GenFinalString();
            }
            else
            {
                return "no ethernet mac addresses found";
            }
        }

        public string StartGeneratePrinterInfo(string MACAddress, string RoomNumber)
        {
            roomLocation = RoomNumber;
            MACInfo.Add(MACAddress);
            OS = "Printer";

            return GenFinalString();
        }

        // the goal for all three of these functions is to produce the csv information for each
        // need to gather the number of valid nics, get their mac addresses, and get the computers hostname
        // in this case valid nics means physical ethernet port

        private void GenerateMACInfoWin()
        {
            OS = "Windows";

            NetworkInterface[] adpaters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adpaters)
            {
                if (adapter.Name.Contains("Ethernet"))
                {
                    MACInfo.Add(FormatMACAddress(adapter.GetPhysicalAddress().ToString()));
                }
            }
        }

        private void GenerateMACInfoLinux()
        {
            OS = "Linux";

            NetworkInterface[] adpaters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adpaters)
            {
                if (adapter.Name.Contains("en"))
                {
                    MACInfo.Add(FormatMACAddress(adapter.GetPhysicalAddress().ToString()));
                }
            }
        }

        //will only come back with a single ethernet port regardless of the actual number due to bash command
        private void GenerateMACInfoOSX()
        {
            OS = "MacOSX";

            string ethernet = Bash("networksetup -listallhardwareports | awk '/Hardware Port: Ethernet/{getline; print $2}'");
            string tbEthernet = Bash("networksetup -listallhardwareports | awk '/Hardware Port: Thunderbolt Ethernet/{getline; print $2}'");

            ethernet = ethernet.Replace(Environment.NewLine, "");
            tbEthernet = tbEthernet.Replace(Environment.NewLine, "");

            NetworkInterface[] adpaters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adpaters)
            {
                if (adapter.Name.Equals(ethernet))
                {
                    MACInfo.Add(FormatMACAddress(adapter.GetPhysicalAddress().ToString()));
                }
                else if (adapter.Name.Equals(tbEthernet))
                {
                    MACInfo.Add(FormatMACAddress(adapter.GetPhysicalAddress().ToString()));
                }
            }
        }

        // see https://loune.net/2017/06/running-shell-bash-commands-in-net-core/ for original text
        private string Bash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        // see GenPrinterMAC function in Program.cs if you need to modify this
        private string FormatMACAddress(string mac_address)
        {
            mac_address = mac_address.Insert(2, ":");
            mac_address = mac_address.Insert(5, ":");
            mac_address = mac_address.Insert(8, ":");
            mac_address = mac_address.Insert(11, ":");
            mac_address = mac_address.Insert(14, ":");

            return mac_address;
        }

        private string GenFinalString()
        {
            if(MACInfo.Count == 1)
            {
                return $"{ headerInfo }{ Environment.NewLine }{ MACInfo[0] },{ MACInfo[0] },{ hostname },{ roomLocation },{ OS },{ genericInfo }";
            }
            else if(MACInfo.Count == 2)
            {
                return $"{ headerInfo }{ Environment.NewLine }{ MACInfo[0] },{ MACInfo[1] },{ hostname },{ roomLocation },{ OS },{ genericInfo }{ Environment.NewLine }{ MACInfo[1] },{ MACInfo[0] },{ hostname },{ roomLocation },{ OS },{ genericInfo }";
            }
            else
            {
                string tempFinalString = $"{ headerInfo }{ Environment.NewLine }{ MACInfo[0] },{ MACInfo[0] },{ hostname },{ roomLocation },{ OS },{ genericInfo }{ Environment.NewLine }";

                foreach(var tempMAC in MACInfo)
                {
                    tempFinalString = $"{ tempFinalString }{ tempMAC }{ Environment.NewLine }";
                }

                return tempFinalString;
            }
        }

        private string MACSelect()
        {
            //Console.WriteLine($"No Ethernet Nics were found.{ Environment.NewLine }Would you like to select from a list of all Nics found? (y/n)");
            //string Answer = Console.ReadLine();

            bool Answer = Prompt.GetYesNo($"No Ethernet Nics were found.{ Environment.NewLine }Would you like to select from a list of all Nics found?", true);

            //if (Answer == "y" || Answer == "Y" || Answer.ToLower() == "yes")
            if (Answer)
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                bool quit = false;
                
                do
                {
                    Console.WriteLine($"{ Environment.NewLine }{ Environment.NewLine }Please select the Nic that you would like to add to the CSV or press q to quit.  (Max 2)");
                    for (int i = 1; i <= adapters.Length; i++)
                    {
                        Console.WriteLine($"{ i } - { adapters[i - 1].Name }");
                    }

                    string rawInput = Console.ReadLine();
                    rawInput = rawInput.Replace(Environment.NewLine, "");
                    int selectedIndex;

                    Console.WriteLine(rawInput);

                    if (rawInput == "q")
                    {
                        quit = true;
                    }
                    else
                    {
                        try
                        {
                            Int32.TryParse(rawInput, out selectedIndex);

                            if (selectedIndex > adapters.Length)
                            {
                                Console.WriteLine("Your choice is outside of the options.");
                            }
                            else
                            {
                                MACInfo.Add(FormatMACAddress(adapters[selectedIndex - 1].GetPhysicalAddress().ToString()));
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                } while (MACInfo.Count < 2 && !quit);

                if (MACInfo.Count != 0)
                {
                    return GenFinalString();
                }
                else
                {
                    return "no ethernet mac addresses found";
                }
            }
            else
            {
                return "no ethernet mac addresses found";
            }
        }
    }
}
