using System;
using System.IO;

namespace Sidekick.Extensions
{
    public static class SidekickPaths
    {
        public static string GetDataFilePath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.Combine("sidekick/", fileName));
        }
    }
}
