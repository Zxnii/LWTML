using System;
using System.IO;
using System.Reflection;

namespace LWTML
{
    public class MultiVersionUtil
    {
        public static string GetStorageDirectory()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                {
                    return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "My Games");
                }
                case PlatformID.MacOSX:
                {
                    var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    
                    if (string.IsNullOrEmpty(homeDirectory)) return ".";
                    
                    return homeDirectory + "/Library/Application Support";
                }
                default:
                {
                    var xdgBaseDirectory = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
                    var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    
                    if (!string.IsNullOrEmpty(xdgBaseDirectory)) return xdgBaseDirectory;
                    if (string.IsNullOrEmpty(home)) return ".";

                    return home + "/.local/share";
                }
            }
        }

        public static string GetStorageSubdirectory(string subDirectory)
        {
            return Path.Combine(GetStorageDirectory(), subDirectory);
        }
    }
}