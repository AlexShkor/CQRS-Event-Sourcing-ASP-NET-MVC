namespace CqrsSample.Infrastructure.Snapshots
{
    public class Snapshot
    {
        public Snapshot(string streamId, int streamVersionId, object payload)
        {
            StreamId = streamId;
            StreamVersion = streamVersionId;
            Payload = payload;
        }

        public string StreamId { get; set; }

        public int StreamVersion { get; set; }

        public object Payload { get; set; }
    }
}