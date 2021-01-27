using ElectronNET.API;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    internal class SidekickView
    {
        public BrowserWindow Browser { get; internal set; }
        public View View { get; internal set; }
    }
}
