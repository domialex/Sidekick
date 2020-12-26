using MediatR;

namespace Sidekick.Domain.Cache.Commands
{
    /// <summary>
    /// Indicates the a clear cache command has executed
    /// </summary>
    public class CacheClearedNotification : INotification
    {
    }
}
