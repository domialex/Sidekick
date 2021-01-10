using System.Threading.Tasks;

namespace Sidekick.Domain.Platforms
{
    public interface IClipboardProvider
    {
        Task<string> Copy();
        Task<string> GetText();
        Task SetText(string text);
    }
}
