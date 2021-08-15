#pragma warning disable CS0067

using System;
using System.Threading.Tasks;
using Sidekick.Common.Platform;

namespace Sidekick.Mock
{
    public class MockProcessProvider : IProcessProvider
    {
        public string ClientLogPath => string.Empty;

        public event Action OnFocus;
        public event Action OnBlur;

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public bool IsPathOfExileInFocus => true;
        public bool IsSidekickInFocus => false;
    }
}
