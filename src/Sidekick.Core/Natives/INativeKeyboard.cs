using System;

namespace Sidekick.Core.Natives
{
    public interface INativeKeyboard
    {
        event Func<string, bool> OnKeyDown;
        void SendCommand(KeyboardCommandEnum command);
        bool IsKeyPressed(string key);
    }
}
