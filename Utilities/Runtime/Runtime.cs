using System;
using System.Security.Principal;

namespace BSStandard.Utilities
{
    public class Runtime
    {
        public static bool IsAdministratorWin()
        {
            return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
                 .IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
