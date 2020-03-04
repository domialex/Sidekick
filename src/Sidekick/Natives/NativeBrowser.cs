using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
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
            logger.LogInformation($"Opening in browser: {uri.AbsoluteUri}");
            var psi = new ProcessStartInfo
            {
                FileName = uri.AbsoluteUri,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
