using Paralect.Domain;

namespace CqrsSample.Domain.Events
{
    public class User_Password_ChangedEvent: Event
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; } 
    }
}