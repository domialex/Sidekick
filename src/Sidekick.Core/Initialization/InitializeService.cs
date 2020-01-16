using Sidekick.Core.DependencyInjection.Services;
using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    [SidekickService(typeof(IInitializeService))]
    public class InitializeService : IInitializeService
    {
        public event Func<Task> OnBeforeInitialize;

        public event Func<Task> OnInitialize;

        public event Func<Task> OnAfterInitialize;

        public event Func<Task> OnReset;

        public async Task Initialize()
        {
            await Reset();
            await CallEvent(OnBeforeInitialize);
            await CallEvent(OnInitialize);
            await CallEvent(OnAfterInitialize);
        }

        public async Task Reset()
        {
            await CallEvent(OnReset);
        }

        private async Task CallEvent(Func<Task> handler)
        {
            if (handler == null)
            {
                return;
            }

            var invocationList = handler.GetInvocationList();
            var tasks = new Task[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                tasks[i] = ((Func<Task>)invocationList[i])();
            }

            await Task.WhenAll(tasks);
        }
    }
}
