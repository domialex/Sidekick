using System;

namespace Sidekick.Core.Natives
{
    public interface INativeKeyboard
    {
        event Func<string, bool> OnKeyDown;
        void SendCommand(KeyboardCommandEnum command);
        void SendInput(string input);
        bool IsKeyPressed(string key);
        void Copy();
    }
}
