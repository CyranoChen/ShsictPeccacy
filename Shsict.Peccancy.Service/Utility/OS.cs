using System;

namespace Shsict.Peccancy.Service.Utility
{
    public class OSInfo
    {
        public static string GetOS()
        {
            return Environment.OSVersion.VersionString;
        }
    }
}