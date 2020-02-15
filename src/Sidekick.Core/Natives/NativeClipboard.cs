using System.Threading.Tasks;
using Sidekick.Core.Loggers;
using Sidekick.Core.Settings;

namespace Sidekick.Core.Natives
{
    public class NativeClipboard : INativeClipboard
    {
        private readonly SidekickSettings settings;
        private readonly INativeKeyboard keyboard;
        private readonly ILogger logger;

        public NativeClipboard(SidekickSettings settings, INativeKeyboard keyboard, ILogger logger)
        {
            this.settings = settings;
            this.keyboard = keyboard;
            this.logger = logger;
        }

        public async Task<string> Copy()
        {
            var clipboardText = string.Empty;

            if (settings.RetainClipboard)
            {
                clipboardText = await GetText();
            }

            await SetText(string.Empty);

            keyboard.SendCommand(KeyboardCommandEnum.Copy);

            await Task.Delay(100);

            // Retrieve clipboard.
            var itemText = await GetText();

            if (settings.RetainClipboard)
            {
                await SetText(clipboardText);
            }

            if (string.IsNullOrWhiteSpace(itemText))
            {
                logger.Log("No item detected in the clipboard.");
                return null;
            }

            return itemText;
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
        }
    }
}
