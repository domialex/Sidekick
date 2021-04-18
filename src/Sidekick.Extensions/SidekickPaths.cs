using System;
using System.IO;

namespace Sidekick.Extensions
{
    public static class SidekickPaths
    {
        public static string GetDataFilePath(string fileName)
        {
            // Possible solution for cross platform support
            // var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return Path.Combine(Environment.ExpandEnvironmentVariables("%AppData%\\sidekick"), fileName);
        }
    }
}
