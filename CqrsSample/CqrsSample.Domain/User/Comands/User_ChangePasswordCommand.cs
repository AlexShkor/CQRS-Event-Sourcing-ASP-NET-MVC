using Paralect.Domain;

namespace CqrsSample.Domain.User.Comands
{
    public class User_ChangePasswordCommand: Command
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}