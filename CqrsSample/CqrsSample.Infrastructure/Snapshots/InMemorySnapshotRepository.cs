using System.Collections.Generic;

namespace CqrsSample.Infrastructure.Snapshots
{
    public class InMemorySnapshotRepository : ISnapshotRepository
    {
        private readonly IDictionary<string, Snapshot> _snapshots;

        public InMemorySnapshotRepository()
        {
            _snapshots = new Dictionary<string, Snapshot>();
        }

        public void Save(Snapshot snapshot)
        {
            _snapshots[snapshot.StreamId] = snapshot;
        }

        public Snapshot Load(string id)
        {
            return _snapshots.ContainsKey(id) ? _snapshots[id] : null;
        }
    }
}