using System.Threading.Tasks;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Keybinds
{
    public class CloseOverlayKeybindHandler : IKeybindHandler
    {
        private readonly IViewLocator viewLocator;

        public CloseOverlayKeybindHandler(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public bool IsValid() => viewLocator.IsOpened(View.Map) || viewLocator.IsOpened(View.Trade);

        public Task Execute()
        {
            viewLocator.Close(View.Map);
            viewLocator.Close(View.Trade);
            return Task.CompletedTask;
        }
    }
}
