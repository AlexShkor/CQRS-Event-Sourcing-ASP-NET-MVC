namespace CqrsSample.Models
{
    public class ChangePasswordModel
    {
        public ChangePasswordModel(string id)
        {
            UserId = id;
        }

        public ChangePasswordModel()
        {
            
        }

        public string UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}