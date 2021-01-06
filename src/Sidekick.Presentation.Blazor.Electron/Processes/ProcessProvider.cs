using System;
using System.Threading;
using System.Threading.Tasks;
using Sidekick.Domain.Process;

namespace Sidekick.Presentation.Blazor.Electron.Processes
{
    public class ProcessProvider : INativeProcess
    {
        public Mutex Mutex { get; set; }

        public bool IsPathOfExileInFocus { get; }

        public bool IsSidekickInFocus { get; }

        public string ClientLogPath { get; }

        public Task CheckPermission()
        {
            throw new NotImplementedException();
        }
    }
}
