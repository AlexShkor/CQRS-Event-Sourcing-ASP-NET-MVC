using Paralect.Domain;

namespace CqrsSample.Domain.User.Events
{
    public class User_RenamedEvent : Event
    {
        public string UserId { get; set; }
        public string NewName { get; set; }
        public string OldName { get; set; }
    }
}