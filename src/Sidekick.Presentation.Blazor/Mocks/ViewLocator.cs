using System.Threading.Tasks;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Mocks
{
    public class ViewLocator : IViewLocator
    {
        public Task Open(View view, params object[] args)
        {
            return Task.CompletedTask;
        }

        public bool IsOpened(View view) => true;

        public bool IsAnyOpened() => true;

        public void CloseAll()
        {
            // Do nothing
        }

        public void Close(View view)
        {
            // Do nothing
        }
    }
}
