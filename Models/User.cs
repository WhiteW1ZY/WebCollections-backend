using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool isBanned { get; set; }

        public List<Collection> Collections { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Reaction> reactions { get; set; }
    }
}
