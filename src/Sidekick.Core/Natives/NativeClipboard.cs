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
            }

            await SetText(string.Empty);

            keyboard.Copy();

            await Task.Delay(100);

            // Retrieve clipboard.
            LastCopiedText = await GetText();

            if (settings.RetainClipboard)
            {
                await SetText(clipboardText);
            }

            return LastCopiedText;
        }

        public async Task<string> GetText()
        {
            return await TextCopy.Clipboard.GetTextAsync();
        }

        public async Task SetText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                await TextCopy.Clipboard.SetTextAsync(text);
            }
            LastCopiedText = text;
        }
    }
}
