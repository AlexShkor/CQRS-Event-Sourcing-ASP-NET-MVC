using System.Security.Authentication;
using CqrsSample.Domain.Events;
using Paralect.Domain;

namespace CqrsSample.Domain
{
    public class UserAR : BaseAR
    {
        private string _name;
        private string _password;

        public UserAR()
        {
            
        }

        public UserAR(string userId, string name, string password, ICommandMetadata metadata)
            : this()
        {
            _id = userId;
            SetCommandMetadata(metadata);
            Apply(new User_CreatedEvent
            {
                UserId = userId,
                Password = password,
                Name = name
            });
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (_password != oldPassword)
            {
                throw new AuthenticationException();
            }
            Apply(new User_Password_ChangedEvent
                      {
                          UserId = _id,
                          NewPassword = newPassword,
                          OldPassword = oldPassword
                      });
        }

        protected void On(User_CreatedEvent created)
        {
            _id = created.UserId;
            _name = created.Name;
            _password = created.Password;
        }

        protected void On(User_Password_ChangedEvent passwordChanged)
        {
            _password = passwordChanged.NewPassword;
        }
    }
}