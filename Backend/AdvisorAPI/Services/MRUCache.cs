namespace AdvisorAPI.Services
{
    public class MRUCache<K, V>
    {
        private readonly int _capacity;
        private readonly Dictionary<K, LinkedListNode<(K Key, V Value)>> _cache;
        private readonly LinkedList<(K Key, V Value)> _recencyList;

        public MRUCache(int capacity = 5)
        {
            _capacity = capacity;
            _cache = new Dictionary<K, LinkedListNode<(K Key, V Value)>>(_capacity);
            _recencyList = new LinkedList<(K Key, V Value)>();
        }

        public V Get(K key)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _recencyList.Remove(node);
                _recencyList.AddFirst(node);
                return node.Value.Value;
            }
            return default;
        }

        public void Put(K key, V value)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _recencyList.Remove(node);
                node.Value = (key, value);
            }
            else
            {
                if (_cache.Count >= _capacity)
                {
                    var lru = _recencyList.Last;
                    _cache.Remove(lru.Value.Key);
                    _recencyList.RemoveLast();
                }
                node = new LinkedListNode<(K Key, V Value)>((key, value));
                _cache[key] = node;
            }
            _recencyList.AddFirst(node);
        }

        public void Delete(K key)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _recencyList.Remove(node);
                _cache.Remove(key);
            }
        }
    }
}
