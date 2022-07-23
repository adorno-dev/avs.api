namespace AVS.API.Services.Mappings
{
    public class ConnectionMapping<T> where T : class
    {
        private readonly Dictionary<T, HashSet<string>> connections = new Dictionary<T, HashSet<string>>();

        public int Count { get { return connections.Count; } }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string>? _connections;

            return connections.TryGetValue(key, out _connections) ?
                _connections : Enumerable.Empty<string>();
        }

        public void Add(T key, string connectionId)
        {
            lock (connections)
            {
                HashSet<string>? _connections;

                if (!connections.TryGetValue(key, out _connections)) {
                    _connections = new HashSet<string>();
                    connections.Add(key, _connections);
                }

                lock (connections) {
                    _connections.Add(connectionId);
                }
            }
        }

        public void Remove(T key, string connectionId)
        {
            lock (connections)
            {
                HashSet<string>? _connections;

                if (!connections.TryGetValue(key, out _connections))
                    return;
                
                lock (_connections) {
                    _connections.Remove(connectionId);
                    
                    if (_connections.Count == 0)
                        connections.Remove(key);
                }
            }
        }
    }
}