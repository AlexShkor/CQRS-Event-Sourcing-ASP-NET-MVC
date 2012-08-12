using System.Collections.Generic;
using System.Security.Authentication;
using CqrsSample.Domain.User.Events;
using CqrsSample.Infrastructure.Snapshots;
using Paralect.Domain;
using Paralect.Transitions;

namespace CqrsSample.Domain.User
{
    public class UserAR
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        public List<IEvent> Changes
        {
            get { return _changes; }
        }

        private readonly UserState _state;

        internal UserState State
        {
            get { return _state; }
        }

        public UserAR(string userId, string name, string password)
        {
            _state = new UserState();
            Apply(new User_CreatedEvent
            {
                UserId = userId,
                Password = password,
                Name = name
            });
        }

        public UserAR(Snapshot snapshot, TransitionStream stream)
        {
            _state = snapshot != null ? (UserState) snapshot.Payload : new UserState();
            foreach (var transition in stream.Read())
            {
                foreach (var @event in transition.Events)
                {
                    _state.Mutate((IEvent) @event.Data);
                }
            }
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (_state.Password != oldPassword)
            {
                throw new AuthenticationException();
            }
            Apply(new User_Password_ChangedEvent
                      {
                          UserId = _state.Id,
                          NewPassword = newPassword,
                          OldPassword = oldPassword
                      });
        }

        public void Rename(string name)
        {
            if (_state.Name == name)
                return;
            Apply(new User_RenamedEvent
                      {
                          UserId = _state.Id,
                          NewName = name,
                          OldName = _state.Name
                      });
        }

        private void Apply(IEvent evt)
        {
            State.Mutate(evt);
            Changes.Add(evt);
        }
    }
}