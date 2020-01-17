﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Sidekick.Helpers
{
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : ObservableCollection<ObservableKeyValuePair<TKey, TValue>>, IDictionary<TKey, TValue>, IEnumerable<ObservableKeyValuePair<TKey, TValue>>
    {
        public TValue this[TKey key]
        {
            get
            {
                TValue result;
                if (!TryGetValue(key, out result))
                    throw new ArgumentException("Key not found", "key");

                return result;
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

        public void AddRange(IEnumerable<ObservableKeyValuePair<TKey, TValue>> items)
        {
            var arrayofItems = items.ToArray();
            if (arrayofItems.Any(i => ContainsKey(i.Key)))
                throw new ArgumentException(string.Format("The dictionary already contains the key \"{0}\"", arrayofItems.First(i => ContainsKey(i.Key)).Key));

            foreach (var item in arrayofItems)
                Add(item);
        }

        public bool Remove(TKey key)
        {
            var remove = ThisAsCollection().Where(pair => Equals(key, pair.Key)).ToList();
            foreach (var pair in remove)
                ThisAsCollection().Remove(pair);

            return remove.Count > 0;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var pair = GetPairByKey(item.Key);
            if (Equals(pair, default(ObservableKeyValuePair<TKey, TValue>)))
                return false;

            if (!Equals(pair.Value, item.Value))
                return false;

            return ThisAsCollection().Remove(pair);
        }

        public bool ContainsKey(TKey key)
        {
            return !Equals(default(ObservableKeyValuePair<TKey, TValue>), ThisAsCollection().FirstOrDefault(i => Equals(key, i.Key)));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var pair = GetPairByKey(item.Key);
            if (Equals(pair, default(ObservableKeyValuePair<TKey, TValue>)))
                return false;

            return Equals(pair.Value, item.Value);
        }

        public ICollection<TKey> Keys
        {
            get { return (from i in ThisAsCollection() select i.Key).ToList(); }
        }

        public ICollection<TValue> Values
        {
            get { return (from i in ThisAsCollection() select i.Value).ToList(); }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            var pair = GetPairByKey(key);
            if(pair != null)
            {
                value = pair.Value;
                return true;
            }
            return false;
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            key = default(TKey);
            var pair = GetPairByValue(value);
            if(pair != null)
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
            return (from i in ThisAsCollection() select new KeyValuePair<TKey, TValue>(i.Key, i.Value)).ToList().GetEnumerator();
        }

        private bool Equals(TKey firstKey, TKey secondKey)
        {
            return EqualityComparer<TKey>.Default.Equals(firstKey, secondKey);
        }

        private ObservableCollection<ObservableKeyValuePair<TKey, TValue>> ThisAsCollection()
        {
            return this;
        }

        private ObservableKeyValuePair<TKey, TValue> GetPairByKey(TKey key)
        {
            return ThisAsCollection().FirstOrDefault(i => i.Key.Equals(key));
        }

        private ObservableKeyValuePair<TKey, TValue> GetPairByValue(TValue value)
        {
            return ThisAsCollection().FirstOrDefault(i => i.Value.Equals(value));
        }
    }

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
