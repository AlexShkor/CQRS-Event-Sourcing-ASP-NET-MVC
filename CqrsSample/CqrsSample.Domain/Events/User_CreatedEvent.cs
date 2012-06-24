using Paralect.Domain;

namespace CqrsSample.Domain.Events
{
    public class User_CreatedEvent: Event
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Password { get; set; } 
    }
}