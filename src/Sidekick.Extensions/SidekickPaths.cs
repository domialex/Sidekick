using System;
using System.IO;

namespace Sidekick.Extensions
{
    public static class SidekickPaths
    {
        public static string GetDataFilePath(string fileName)
        {
            var environmentFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var sidekickFolder = Path.Combine(environmentFolder, "sidekick");
            var dataFile = Path.Combine(sidekickFolder, fileName);

            if (!Directory.Exists(sidekickFolder))
            {
                Directory.CreateDirectory(sidekickFolder);
            }

            return dataFile;
        }
    }
}
