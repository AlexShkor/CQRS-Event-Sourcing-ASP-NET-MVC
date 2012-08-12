using System.Collections.Generic;
using Paralect.Domain;
using Paralect.Transitions;

namespace CqrsSample.Infrastructure.EventStore
{
    public interface IEventStore
    {
        TransitionStream OpenStream(string id);
        TransitionStream OpenStream(string id, int fromVersion, int toVersion);
        void AppendToStream(string id, int originalVersion, ICollection<IEvent> events);
    }
}