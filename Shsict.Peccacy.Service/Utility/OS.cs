using System;

namespace Shsict.Peccacy.Service.Utility
{
    public class OSInfo
    {
        public static string GetOS()
        {
            return Environment.OSVersion.VersionString;
        }
    }
}