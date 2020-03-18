using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Sidekick.UI.ApplicationLogs
{
    public interface IApplicationLogViewModel : INotifyPropertyChanged
    {
        ObservableCollection<string> Logs { get; }
    }
}
