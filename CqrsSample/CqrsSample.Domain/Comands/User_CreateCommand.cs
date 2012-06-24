using Paralect.Domain;
using Paralect.ServiceBus;

namespace CqrsSample.Domain.Comands
{
    public class User_CreateCommand: Command
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
    }

    public class User_CreateCommandHandler : IMessageHandler<User_CreateCommand>
    {
        private readonly IRepository _repository;

        public User_CreateCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(User_CreateCommand message)
        {
            var ar = new UserAR(message.UserId, message.Name, message.Password, message.Metadata);
            _repository.Save(ar);
        }
    }
}