using System.Collections.Generic;
using System.Linq;
using CqrsSample.Domain.Events;
using Paralect.ServiceBus;

namespace CqrsSample.EventHandlers
{
    public class UserEntityEventHandler:
        IMessageHandler<User_CreatedEvent>,
        IMessageHandler<User_Password_ChangedEvent>
    {
        private static List<User> _users = new List<User>();
        public static List<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        public void Handle(User_CreatedEvent message)
        {
            Users.Add(new User
                          {
                              Id = message.UserId,
                              Name = message.Name,
                              Password = message.Password
                          });
        }

        public void Handle(User_Password_ChangedEvent message)
        {
            var user = Users.Single(x => x.Id == message.UserId);
            user.Password = message.NewPassword;
        }
    }
}