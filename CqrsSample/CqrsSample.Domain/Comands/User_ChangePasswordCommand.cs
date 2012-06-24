using Paralect.Domain;
using Paralect.ServiceBus;

namespace CqrsSample.Domain.Comands
{
    public class User_ChangePasswordCommand: Command
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class User_ChangePasswordCommandHandler: IMessageHandler<User_ChangePasswordCommand>
    {
        private readonly IRepository _repository;

        public User_ChangePasswordCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(User_ChangePasswordCommand message)
        {
            var user = _repository.GetById<UserAR>(message.UserId);
            user.SetCommandMetadata(message.Metadata);
            user.ChangePassword(message.OldPassword, message.NewPassword);
            _repository.Save(user);
        }
    }
}