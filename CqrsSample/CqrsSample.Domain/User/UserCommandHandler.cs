using System;
using CqrsSample.Domain.User.Comands;
using CqrsSample.Infrastructure.EventStore;
using CqrsSample.Infrastructure.Snapshots;
using Paralect.ServiceBus;

namespace CqrsSample.Domain.User
{
    public class UserCommandHandler :
        IMessageHandler<User_ChangePasswordCommand>,
        IMessageHandler<User_RenameCommand>,
        IMessageHandler<User_CreateCommand>
    {
        private readonly ISnapshotRepository _snapshotRepository;
        private readonly IEventStore _eventStore;
        private const int SnapshotInterval = 100;

        public UserCommandHandler(ISnapshotRepository snapshotRepository, IEventStore eventStore)
        {
            _snapshotRepository = snapshotRepository;
            _eventStore = eventStore;
        }

        public void Handle(User_ChangePasswordCommand message)
        {
            Update(message.UserId, (user) => user.ChangePassword(message.OldPassword,message.NewPassword));
        }

        public void Handle(User_CreateCommand message)
        {
            var user = new UserAR(message.UserId, message.Name, message.Password);
            _eventStore.AppendToStream(message.UserId,0,user.Changes);
        }

        public void Handle(User_RenameCommand message)
        {
            Update(message.UserId, (user) => user.Rename(message.Name));
        }

        private void Update(string userId, Action<UserAR> updateAction)
        {
            var snapshot = _snapshotRepository.Load(userId);
            var startVersion = snapshot == null ? 0 : snapshot.StreamVersion + 1;
            var stream = _eventStore.OpenStream(userId, startVersion, int.MaxValue);
            var user = new UserAR(snapshot, stream);
            updateAction(user);
            var originalVersion = stream.GetVersion();
            _eventStore.AppendToStream(userId, originalVersion, user.Changes);
            var newVersion = originalVersion + 1;
            if (newVersion % SnapshotInterval == 0)
            {
                _snapshotRepository.Save(new Snapshot(userId, newVersion,user.State));
            }
        }
    }
}