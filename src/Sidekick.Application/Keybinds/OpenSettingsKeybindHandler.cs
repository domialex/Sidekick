using System.Threading.Tasks;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;

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
