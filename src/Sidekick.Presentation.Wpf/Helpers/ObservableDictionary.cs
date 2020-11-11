using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PropertyChanged;

namespace Sidekick.Presentation.Wpf.Helpers
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : ObservableList<ObservableKeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>
    {
        [SuppressPropertyChangedWarnings]
        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var result))
                    return result;

                return default;
            }
            set
            {
                if (ContainsKey(key))
                    GetPairByKey(key).Value = value;
                else
                    Add(key, value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
                throw new ArgumentException(string.Format("The dictionary already contains the key \"{0}\"", key));

            Add(new ObservableKeyValuePair<TKey, TValue>(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (ContainsKey(item.Key))
                throw new ArgumentException(string.Format("The dictionary already contains the key \"{0}\"", item.Key));

            Add(item.Key, item.Value);
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            var arrayofItems = items.ToArray();
            if (arrayofItems.Any(i => ContainsKey(i.Key)))
                throw new ArgumentException(string.Format("The dictionary already contains the key \"{0}\"", arrayofItems.First(i => ContainsKey(i.Key)).Key));

            foreach (var item in arrayofItems)
                Add(item.Key, item.Value);
        }

        public new void AddRange(IEnumerable<ObservableKeyValuePair<TKey, TValue>> items)
        {
            var arrayofItems = items.ToArray();
            if (arrayofItems.Any(i => ContainsKey(i.Key)))
                throw new ArgumentException(string.Format("The dictionary already contains the key \"{0}\"", arrayofItems.First(i => ContainsKey(i.Key)).Key));

            foreach (var item in arrayofItems)
                Add(item);
        }

        public bool Remove(TKey key)
        {
            var remove = ToCollection().Where(pair => Equals(key, pair.Key)).ToList();
            foreach (var pair in remove)
                ToCollection().Remove(pair);

            return remove.Count > 0;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var pair = GetPairByKey(item.Key);
            if (Equals(pair, default(ObservableKeyValuePair<TKey, TValue>)))
                return false;

            if (!Equals(pair.Value, item.Value))
                return false;

            return ToCollection().Remove(pair);
        }

        public bool ContainsKey(TKey key)
        {
            return !Equals(default(ObservableKeyValuePair<TKey, TValue>), ToCollection().FirstOrDefault(i => Equals(key, i.Key)));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var pair = GetPairByKey(item.Key);
            if (Equals(pair, default(ObservableKeyValuePair<TKey, TValue>)))
                return false;

            return Equals(pair.Value, item.Value);
        }

        public ICollection<TKey> Keys => ToCollection().Select(x => x.Key).ToList();

        public ICollection<TValue> Values => ToCollection().Select(x => x.Value).ToList();

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
            var pair = GetPairByKey(key);
            if (pair != null)
            {
                value = pair.Value;
                return true;
            }
            return false;
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            key = default;
            var pair = GetPairByValue(value);
            if (pair != null)
            {
                key = pair.Key;
                return true;
            }
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public new IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (from i in ToCollection() select new KeyValuePair<TKey, TValue>(i.Key, i.Value)).ToList().GetEnumerator();
        }

        private bool Equals(TKey firstKey, TKey secondKey)
        {
            return EqualityComparer<TKey>.Default.Equals(firstKey, secondKey);
        }

        public ObservableCollection<ObservableKeyValuePair<TKey, TValue>> ToCollection()
        {
            return this;
        }

        private ObservableKeyValuePair<TKey, TValue> GetPairByKey(TKey key)
        {
            return ToCollection().FirstOrDefault(i => i.Key.Equals(key));
        }

        private ObservableKeyValuePair<TKey, TValue> GetPairByValue(TValue value)
        {
            return ToCollection().FirstOrDefault(i => i.Value.Equals(value));
        }
    }
}
