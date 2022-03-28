using System;

namespace BSStandard.Utilities
{
    public class Networking
    {
        public static bool IsIP(string host)
        {
            System.Net.IPAddress ip;
            return System.Net.IPAddress.TryParse(host, out ip);
        }
    }
}
