using Sidekick.Core.Natives;

namespace Sidekick.Business.Stashes
{
    public class StashService : IStashService
    {
        private readonly INativeKeyboard keyboard;

        public StashService(INativeKeyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        public void ScrollRight()
        {
            keyboard.SendInput("Right");
        }

        public void ScrollLeft()
        {
            keyboard.SendInput("Left");
        }
    }
}
