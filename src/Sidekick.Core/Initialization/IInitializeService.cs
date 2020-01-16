using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public interface IInitializeService
    {
        event Func<Task> OnAfterInitialize;
        event Func<Task> OnBeforeInitialize;
        event Func<Task> OnInitialize;
        event Func<Task> OnReset;

        Task Initialize();
        Task Reset();
    }
}
