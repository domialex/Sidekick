using System;
using System.Diagnostics;
using Serilog;
using Sidekick.Core.Natives;

namespace Sidekick.Natives
{
    public class NativeBrowser : INativeBrowser
    {
        private readonly ILogger logger;

        public NativeBrowser(ILogger logger)
        {
            this.logger = logger.ForContext(GetType());
        }

        public void Open(Uri uri)
        {
            logger.Information("Opening in browser: {uri}", uri.AbsoluteUri);
            var psi = new ProcessStartInfo
            {
                FileName = uri.AbsoluteUri,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
