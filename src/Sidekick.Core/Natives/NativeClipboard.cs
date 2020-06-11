using System.Threading.Tasks;
using Sidekick.Core.Settings;

namespace Sidekick.Core.Natives
{
    public class NativeClipboard : INativeClipboard
    {
        private readonly SidekickSettings settings;
        private readonly INativeKeyboard keyboard;

        public NativeClipboard(SidekickSettings settings, INativeKeyboard keyboard)
        {
            this.settings = settings;
            this.keyboard = keyboard;
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

            keyboard.Copy();

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
