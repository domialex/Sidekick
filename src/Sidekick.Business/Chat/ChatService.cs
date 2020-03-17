using System.Threading.Tasks;
using Serilog;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Chat
{
    public class ChatService : IChatService
    {
        private readonly ILogger logger;
        private readonly INativeKeyboard keyboard;
        private readonly INativeClipboard clipboard;
        private readonly SidekickSettings settings;

        public ChatService(
            ILogger logger,
            INativeKeyboard keyboard,
            INativeClipboard clipboard,
            SidekickSettings settings)
        {
            this.logger = logger.ForContext(GetType());
            this.keyboard = keyboard;
            this.clipboard = clipboard;
            this.settings = settings;
        }

        public async Task Write(string text)
        {
            string clipboardValue = null;
            if (settings.RetainClipboard)
            {
                clipboardValue = await clipboard.GetText();
            }

            await clipboard.SetText(text);

            keyboard.SendInput("Enter");
            keyboard.Paste();
            keyboard.SendInput("Enter");
            keyboard.SendInput("Enter");
            keyboard.SendInput("Up");
            keyboard.SendInput("Up");
            keyboard.SendInput("Esc");

            if (settings.RetainClipboard)
            {
                await clipboard.SetText(clipboardValue);
            }

            logger.Information("ChatService - Wrote '{text}' in the chat", text);
        }

        public async Task StartWriting(string text)
        {
            string clipboardValue = null;
            if (settings.RetainClipboard)
            {
                clipboardValue = await clipboard.GetText();
            }

            await clipboard.SetText(text);

            keyboard.SendInput("Enter");
            keyboard.Paste();

            if (settings.RetainClipboard)
            {
                await clipboard.SetText(clipboardValue);
            }

            logger.Information("ChatService - Started writing '{text}' in the chat", text);
        }
    }
}
