using System.Threading.Tasks;

namespace Sidekick.Domain.Clipboard
{
    public interface INativeClipboard
    {
        string LastCopiedText { get; }
        Task<string> Copy();
        Task<string> GetText();
        Task SetText(string text);
    }
}
