using System.Threading.Tasks;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Keybinds
{
    public class OpenCheatsheetsKeybindHandler : IKeybindHandler
    {
        private readonly IViewLocator viewLocator;
        private readonly ISidekickSettings settings;
        private readonly IProcessProvider processProvider;

        public OpenCheatsheetsKeybindHandler(
            IViewLocator viewLocator,
            ISidekickSettings settings,
            IProcessProvider processProvider)
        {
            this.viewLocator = viewLocator;
            this.settings = settings;
            this.processProvider = processProvider;
        }

        public bool IsValid() => processProvider.IsPathOfExileInFocus || processProvider.IsSidekickInFocus;

        public Task Execute()
        {
            viewLocator.Open(View.League, settings.Cheatsheets_Selected);
            return Task.CompletedTask;
        }
    }
}
