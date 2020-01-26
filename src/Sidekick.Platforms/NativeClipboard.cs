using System.Threading.Tasks;

namespace Sidekick.Platforms
{
    public class NativeClipboard : INativeClipboard
    {
        public async Task<string> GetText()
        {
            return await TextCopy.Clipboard.GetTextAsync();
        }

        public async Task SetText(string text)
        {
            await TextCopy.Clipboard.SetTextAsync(text);
        }
    }
}
