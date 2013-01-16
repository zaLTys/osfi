using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatistinesAtaskaitos.Models
{
    public class DefaultableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _underlying;
        private readonly TValue _default;

        public DefaultableDictionary(Dictionary<TKey, TValue> underlying, TValue @default)
        {
            _underlying = underlying;
            _default = @default;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _underlying.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _underlying.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _underlying.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _underlying.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _underlying.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _underlying.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _underlying.Remove(item);
        }

        public int Count
        {
            get { return _underlying.Count; }
        }

        public bool IsReadOnly
        {
            get { return _underlying.IsReadOnly; }
        }
        public bool ContainsKey(TKey key)
        {
            return _underlying.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _underlying.Add(key, value);
        }

        public bool Remove(TKey key)
        {
            return _underlying.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _underlying.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get 
            { 
                TValue val;
                if (_underlying.TryGetValue(key, out val)) return val;
                return _default;
            }
            set { _underlying[key] = value; }
        }

        public ICollection<TKey> Keys
        {
            get { return _underlying.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return _underlying.Values; }
        }
    }
}