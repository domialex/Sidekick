using System.Threading.Tasks;

namespace Sidekick.Platforms
{
    public interface INativeClipboard
    {
        Task<string> GetText();
        Task SetText(string text);
    }
}
