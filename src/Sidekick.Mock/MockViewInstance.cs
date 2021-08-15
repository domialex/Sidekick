using System.Threading.Tasks;
using Sidekick.Common.Blazor.Views;

namespace Sidekick.Mock
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

        public string Title { get; private set; }

        public virtual void SetTitle(string title)
        {
            Title = title;
        }
    }
}
