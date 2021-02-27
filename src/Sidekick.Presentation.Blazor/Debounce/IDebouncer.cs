using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sidekick.Presentation.Blazor.Debounce
{
    public interface IDebouncer
    {
        Task Debounce(string id,
                      Func<Task> func,
                      CancellationToken cancellationToken = new CancellationToken(),
                      int refreshRate = 1000,
                      int delay = 2000,
                      Action<int> delayUpdate = null);
    }
}
