using System.Collections.ObjectModel;
using System.ComponentModel;
using Sidekick.Core.Loggers;

namespace Sidekick.UI.ApplicationLogs
{
    public interface IApplicationLogViewModel : INotifyPropertyChanged
    {
        ObservableCollection<Log> Logs { get; }
    }
}
