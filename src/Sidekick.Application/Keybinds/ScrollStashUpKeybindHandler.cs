using System.Threading.Tasks;
using Sidekick.Common.Platform;
using Sidekick.Domain.Keybinds;

namespace Sidekick.Application.Keybinds
{
    public class ScrollStashUpKeybindHandler : IKeybindHandler
    {
        private readonly IKeyboardProvider keyboard;
        private readonly IProcessProvider processProvider;

        public ScrollStashUpKeybindHandler(
            IKeyboardProvider keyboard,
            IProcessProvider processProvider)
        {
            this.keyboard = keyboard;
            this.processProvider = processProvider;
        }

        public bool IsValid() => processProvider.IsPathOfExileInFocus;

        public Task Execute()
        {
            keyboard.PressKey("Left");
            return Task.CompletedTask;
        }
    }
}
