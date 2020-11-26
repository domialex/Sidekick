using System;
using System.ComponentModel;

namespace Sidekick.Presentation.Wpf.Helpers
{
    [Serializable]
    public sealed class ObservableKeyValuePair<TKey, TValue> : INotifyPropertyChanged
    {
        private TKey _key;
        private TValue _value;

        public TKey Key
        {
            get { return _key; }
            set
            {
                _key = value;
                OnPropertyChanged("Key");
            }
        }

        public TValue Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public ObservableKeyValuePair()
        { }

        public ObservableKeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
