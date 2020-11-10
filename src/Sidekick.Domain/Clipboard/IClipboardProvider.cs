using System.Threading.Tasks;

namespace Sidekick.Domain.Clipboard
{
    public interface IClipboardProvider
    {
        Task<string> Copy();
        Task<string> GetText();
        Task SetText(string text);
    }
}
