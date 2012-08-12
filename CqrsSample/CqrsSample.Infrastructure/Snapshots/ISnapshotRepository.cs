namespace CqrsSample.Infrastructure.Snapshots
{
    public interface ISnapshotRepository
    {
        void Save(Snapshot snapshot);
        Snapshot Load(string id);
    }
}