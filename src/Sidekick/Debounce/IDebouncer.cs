using System;
using System.Threading.Tasks;

namespace Sidekick.Debounce
{
    public interface IDebouncer
    {
        Task Debounce(string id, Func<Task> func, int refreshRate = 1000, int delay = 2000, Action<int> delayUpdate = null);
    }
}
