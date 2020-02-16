using System;
using System.Diagnostics;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;

namespace Sidekick.Natives
{
    public class NativeBrowser : INativeBrowser
    {
        private readonly ILogger logger;

        public NativeBrowser(ILogger logger)
        {
            this.logger = logger;
        }

        public void Open(Uri uri)
        {
            logger.Log($"Opening in browser: {uri.AbsoluteUri}");
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = uri.AbsoluteUri,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
