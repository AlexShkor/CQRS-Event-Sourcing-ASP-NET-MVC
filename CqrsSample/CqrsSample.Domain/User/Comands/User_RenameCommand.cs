using Paralect.Domain;

namespace CqrsSample.Domain.User.Comands
{
    public class User_RenameCommand: Command
    {
        public string UserId { get; set; }

        public string Name { get; set; }
    }
}