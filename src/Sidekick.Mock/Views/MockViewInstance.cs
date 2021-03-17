using System.Threading.Tasks;
using Sidekick.Domain.Views;

namespace Sidekick.Mock.Views
{
    public class MockViewInstance : IViewInstance
    {
        public View View => View.Error;

        public bool Minimizable => false;

        public bool Maximizable => false;

        public Task Close()
        {
            return Task.CompletedTask;
        }

        public Task Maximize()
        {
            return Task.CompletedTask;
        }

        public Task Minimize()
        {
            return Task.CompletedTask;
        }
    }
}
