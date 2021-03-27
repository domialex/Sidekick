using System.Threading.Tasks;
using Sidekick.Domain.Views;

namespace Sidekick.Mock.Views
{
    public class MockViewInstance : IViewInstance
    {
        public virtual Task Close()
        {
            return Task.CompletedTask;
        }

        public virtual Task Maximize()
        {
            return Task.CompletedTask;
        }

        public virtual Task Minimize()
        {
            return Task.CompletedTask;
        }

        public virtual Task SetTitle(string title)
        {
            return Task.CompletedTask;
        }
    }
}
