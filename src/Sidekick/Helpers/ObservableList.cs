using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Sidekick.Helpers
{
    public class ObservableList<T> : ObservableCollection<T>
    {
        public ObservableList() : base() { }
        public ObservableList(IEnumerable<T> collection) : base(collection) { }
        public ObservableList(List<T> list) : base(list) { }

        public new void Add(T item)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                base.Add(item);
            });
        }

        public new void Remove(T item)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                base.Remove(item);
            });
        }

        public new void RemoveAt(int index)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                base.RemoveAt(index);
            });
        }

        public void AddRange(IEnumerable<T> items)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var item in items)
                {
                    base.Add(item);
                }
            });
        }
    }
}
