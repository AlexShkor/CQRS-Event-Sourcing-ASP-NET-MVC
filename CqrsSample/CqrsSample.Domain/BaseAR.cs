using System;
using Paralect.Domain;

namespace CqrsSample.Domain
{
    public class BaseAR : AggregateRoot
    {
        private ICommandMetadata _commandMetadata;

        public void SetCommandMetadata(ICommandMetadata commandMetadata)
        {
            _commandMetadata = commandMetadata;
        }

        public new void Apply(IEvent evt)
        {
            if (_commandMetadata == null)
                throw new ArgumentException("You should send command metadata to Aggregate Root before Apply event");

            evt.Metadata.UserId = _commandMetadata.UserId;
            evt.Metadata.CommandId = _commandMetadata.CommandId;
            evt.Metadata.EventId = Guid.NewGuid().ToString();

            base.Apply(evt);
        }
    }
}