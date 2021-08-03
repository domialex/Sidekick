using System.Threading.Tasks;

namespace Sidekick.Common.Platform
{
    public interface IClipboardProvider
    {
        Task<string> Copy();
        Task<string> GetText();
        Task SetText(string text);
    }
}
