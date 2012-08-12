using System;
using System.Collections.Generic;
using System.Linq;
using Paralect.Domain;
using Paralect.Domain.EventBus;
using Paralect.Transitions;

namespace CqrsSample.Infrastructure.EventStore
{
    public class EventStore: IEventStore
    {
        private readonly ITransitionRepository _transitionRepository;
        private readonly IEventBus _eventBus;

        public EventStore(ITransitionRepository transitionRepository, IEventBus eventBus)
        {
            _transitionRepository = transitionRepository;
            _eventBus = eventBus;
        }

        public TransitionStream OpenStream(string id)
        {
            return new TransitionStream(id,_transitionRepository, 0 , int.MaxValue);
        }

        public TransitionStream OpenStream(string id, int fromVersion, int toVersion)
        {
            return new TransitionStream(id, _transitionRepository, fromVersion, toVersion);
        }

        public void AppendToStream(string id, int originalVersion, ICollection<IEvent> events)
        {
            var stream = OpenStream(id);
            var transitionEvents = new List<TransitionEvent>();
            foreach (var e in events)
            {
                e.Metadata.StoredDate = DateTime.UtcNow;
                e.Metadata.TypeName = e.GetType().Name;
                transitionEvents.Add(new TransitionEvent(e.GetType().AssemblyQualifiedName, e, null));
            }
            var transition = new Transition(new TransitionId(id, originalVersion + 1), DateTime.UtcNow, transitionEvents, null);
            stream.Write(transition);
            _eventBus.Publish(events);
        }
    }

    public static class Extenssions
    {
        public static int GetVersion(this TransitionStream stream)
        {
            return stream.Read().Any() ? stream.Read().Max(x => x.Id.Version) : 0;
        }
    }
}