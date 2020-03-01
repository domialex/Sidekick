using System.Threading.Tasks;

namespace Sidekick.Core.Natives
{
    public interface INativeClipboard
    {
        string LastCopiedText { get; }
        Task<string> Copy();
        Task<string> GetText();
        Task SetText(string text);
    }
}
