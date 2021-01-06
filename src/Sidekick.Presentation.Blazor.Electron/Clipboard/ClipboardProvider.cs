using System.Threading.Tasks;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Settings;

namespace Sidekick.Presentation.Blazor.Electron.Clipboard
{
    public class ClipboardProvider : IClipboardProvider
    {
        private readonly ISidekickSettings settings;
        private readonly IKeybindsProvider keybindsProvider;

        public ClipboardProvider(
            ISidekickSettings settings,
            IKeybindsProvider keybindsProvider)
        {
            this.settings = settings;
            this.keybindsProvider = keybindsProvider;
        }

        public async Task<string> Copy()
        {
            var clipboardText = string.Empty;

            if (settings.RetainClipboard)
            {
                clipboardText = await ElectronNET.API.Electron.Clipboard.ReadTextAsync();
                if (clipboardText == null)
                {
                    clipboardText = string.Empty;
                }
            }

            await SetText(string.Empty);

            keybindsProvider.PressKey("Copy");

            await Task.Delay(100);

            // Retrieve clipboard.
            var result = await ElectronNET.API.Electron.Clipboard.ReadTextAsync();

            if (settings.RetainClipboard)
            {
                await Task.Delay(100);
                ElectronNET.API.Electron.Clipboard.WriteText(clipboardText);
            }

            return result;
        }

        public Task<string> GetText()
        {
            return ElectronNET.API.Electron.Clipboard.ReadTextAsync();
        }

        public Task SetText(string text)
        {
            if (text == null)
            {
                text = string.Empty;
            }
            ElectronNET.API.Electron.Clipboard.WriteText(text);
            return Task.CompletedTask;
        }
    }
}
