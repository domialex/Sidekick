using System.Threading.Tasks;
using Sidekick.Common.Blazor.Views;
using Sidekick.Common.Platform;
using Sidekick.Domain.Keybinds;

namespace Sidekick.Application.Keybinds
{
    public class OpenSettingsKeybindHandler : IKeybindHandler
    {
        private readonly IViewLocator viewLocator;
        private readonly IProcessProvider processProvider;

        public OpenSettingsKeybindHandler(
            IViewLocator viewLocator,
            IProcessProvider processProvider)
        {
            this.viewLocator = viewLocator;
            this.processProvider = processProvider;
        }

        public bool IsValid() => processProvider.IsPathOfExileInFocus || processProvider.IsSidekickInFocus;

        public Task Execute()
        {
            viewLocator.Open(View.Settings);
            return Task.CompletedTask;
        }
    }
}
