using Sidekick.Domain.Keybinds;

namespace Sidekick.Business.Stashes
{
    public class StashService : IStashService
    {
        private readonly IKeybindsProvider keybindsProvider;

        public StashService(
            IKeybindsProvider keybindsProvider)
        {
            this.keybindsProvider = keybindsProvider;
        }

        public void ScrollRight()
        {
            keybindsProvider.PressKey("Right");
        }

        public void ScrollLeft()
        {
            keybindsProvider.PressKey("Left");
        }
    }
}
