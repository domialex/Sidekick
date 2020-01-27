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
            Process.Start(uri.AbsoluteUri);
        }
    }
}
