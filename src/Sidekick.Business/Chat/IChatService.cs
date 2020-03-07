using System.Threading.Tasks;

namespace Sidekick.Business.Chat
{
    public interface IChatService
    {
        Task Write(string text);
        Task StartWriting(string text);
    }
}
