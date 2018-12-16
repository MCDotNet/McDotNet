using System.Collections;
using System.Collections.Generic;

namespace McDotNet.Data
{
    public class Assets : IDictionary<string, AssetHash>
    {
        public AssetHash this[string key] { get => ((IDictionary<string, AssetHash>)PathsCollection)[key]; set => ((IDictionary<string, AssetHash>)PathsCollection)[key] = value; }

        public Dictionary<string, AssetHash> PathsCollection { get; set; } = new Dictionary<string, AssetHash>();

        public ICollection<string> Keys => ((IDictionary<string, AssetHash>)PathsCollection).Keys;

        public ICollection<AssetHash> Values => ((IDictionary<string, AssetHash>)PathsCollection).Values;

        public int Count => ((IDictionary<string, AssetHash>)PathsCollection).Count;

        public bool IsReadOnly => ((IDictionary<string, AssetHash>)PathsCollection).IsReadOnly;

        public void Add(string key, AssetHash value)
        {
            ((IDictionary<string, AssetHash>)PathsCollection).Add(key, value);
        }

        public void Add(KeyValuePair<string, AssetHash> item)
        {
            ((IDictionary<string, AssetHash>)PathsCollection).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<string, AssetHash>)PathsCollection).Clear();
        }

        public bool Contains(KeyValuePair<string, AssetHash> item)
        {
            return ((IDictionary<string, AssetHash>)PathsCollection).Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, AssetHash>)PathsCollection).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, AssetHash>[] array, int arrayIndex)
        {
            ((IDictionary<string, AssetHash>)PathsCollection).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, AssetHash>> GetEnumerator()
        {
            return ((IDictionary<string, AssetHash>)PathsCollection).GetEnumerator();
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, AssetHash>)PathsCollection).Remove(key);
        }

        public bool Remove(KeyValuePair<string, AssetHash> item)
        {
            return ((IDictionary<string, AssetHash>)PathsCollection).Remove(item);
        }

        public bool TryGetValue(string key, out AssetHash value)
        {
            return ((IDictionary<string, AssetHash>)PathsCollection).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, AssetHash>)PathsCollection).GetEnumerator();
        }
    }
}
