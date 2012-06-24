using System;
using Paralect.Domain;
using Paralect.ServiceBus;

namespace CqrsSample
{
    public interface ICommandService
    {
        void Send(params ICommand[] commands);
        void PrepareCommands(params ICommand[] commands);
    }

    public class CommandService : ICommandService
    {
        private readonly IServiceBus _bus;

        public CommandService(IServiceBus bus)
        {
            _bus = bus;
        }

        public virtual void Send(params ICommand[] commands)
        {
            PrepareCommands(commands);
            _bus.Send(commands);
        }

        public void PrepareCommands(params ICommand[] commands)
        {
            foreach (ICommand t in commands)
            {
                t.Metadata.CommandId = Guid.NewGuid().ToString();
                t.Metadata.CreatedDate = DateTime.UtcNow;
                t.Metadata.TypeName = t.GetType().FullName;
            }
        }
    }
}