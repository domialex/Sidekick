using System.Threading.Tasks;
using Sidekick.Common.Settings;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Keybinds
{
    public class OpenCheatsheetsKeybindHandler : IKeybindHandler
    {
        private readonly IViewLocator viewLocator;
        private readonly ISettings settings;
        private readonly IProcessProvider processProvider;

        public OpenCheatsheetsKeybindHandler(
            IViewLocator viewLocator,
            ISettings settings,
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
