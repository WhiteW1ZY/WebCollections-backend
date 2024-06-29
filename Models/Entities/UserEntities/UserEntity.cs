namespace backend.Models.Entityes.UserEntityes
{
    public class UserEntity
    {
        public string login { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public bool isBanned { get; set; }
    }
}
