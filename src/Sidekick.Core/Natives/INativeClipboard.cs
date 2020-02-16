using System.Threading.Tasks;

namespace Sidekick.Core.Natives
{
    public interface INativeClipboard
    {
        Task<string> Copy();
        Task<string> GetText();
        Task SetText(string text);
    }
}
