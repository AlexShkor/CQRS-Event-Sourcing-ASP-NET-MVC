using CqrsSample.Domain.User.Events;
using Paralect.Domain;

namespace CqrsSample.Domain.User
{
    public class UserState
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }

        public void On(User_RenamedEvent userRenamed)
        {
            Name = userRenamed.NewName;
        }

        public void On(User_Password_ChangedEvent userPasswordChanged)
        {
            Password = userPasswordChanged.NewPassword;
        }

        public void On(User_CreatedEvent userCreated)
        {
            Id = userCreated.UserId;
            Name = userCreated.Name;
            Password = userCreated.Password;
        }

        public void Mutate(IEvent @event)
        {
            ((dynamic) this).On((dynamic) @event);
        }
    }
}