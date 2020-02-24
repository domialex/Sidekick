using System.Collections.ObjectModel;
using Sidekick.Core.Loggers;

namespace Sidekick.UI.ApplicationLogs
{
    public interface IApplicationLogViewModel
    {
        ObservableCollection<Log> Logs { get; }
    }
}
