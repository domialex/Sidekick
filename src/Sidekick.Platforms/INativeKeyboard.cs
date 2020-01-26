using System;
using System.Threading.Tasks;

namespace Sidekick.Platforms
{
    public interface INativeKeyboard
    {
        event Func<string, Task> OnKeyDown;
        void SendCommand(KeyboardCommandEnum command);
        bool IsKeyPressed(string key);
    }
}
