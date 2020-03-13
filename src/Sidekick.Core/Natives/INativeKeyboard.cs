using System;

namespace Sidekick.Core.Natives
{
    public interface INativeKeyboard
    {
        event Func<string, bool> OnKeyDown;
        void SendInput(string input);
        bool IsKeyPressed(string key);
        void Copy();
        void Paste();
    }
}
