using Paralect.Domain;

namespace CqrsSample.Domain.User.Comands
{
    public class User_CreateCommand: Command
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
    }
}