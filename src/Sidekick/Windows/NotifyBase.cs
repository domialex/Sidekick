using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sidekick.Windows
{
    public abstract class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyProperty<T>(ref T storedValue, T newValue, [CallerMemberName]string propertyName = null)
        {
            if(newValue.Equals(storedValue))
            {
                return;
            }

            storedValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
