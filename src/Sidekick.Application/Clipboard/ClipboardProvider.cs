using System.Threading.Tasks;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Settings;

namespace Sidekick.Application.Clipboard
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

        public string LastCopiedText { get; private set; }

        public async Task<string> Copy()
        {
            var clipboardText = string.Empty;

            if (settings.RetainClipboard)
            {
                clipboardText = await GetText();
                if (clipboardText == null)
                {
                    clipboardText = string.Empty;
                }
            }

            await SetText(string.Empty);

            keybindsProvider.PressKey("Copy");

            await Task.Delay(100);

            // Retrieve clipboard.
            LastCopiedText = await GetText();

            if (settings.RetainClipboard)
            {
                await Task.Delay(100);
                await TextCopy.ClipboardService.SetTextAsync(clipboardText);
            }

            return LastCopiedText;
        }

        public async Task<string> GetText()
        {
            return await TextCopy.ClipboardService.GetTextAsync();
        }

        public async Task SetText(string text)
        {
            if (text == null)
            {
                text = string.Empty;
            }
            await TextCopy.ClipboardService.SetTextAsync(text);
            LastCopiedText = text;
        }
    }
}
