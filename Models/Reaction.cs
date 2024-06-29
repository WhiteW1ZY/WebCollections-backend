using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Reaction
    {
        [Key]
        public int Id { get; set; }

        public User Owner { get; set; }

        public bool IsLike { get; set; }

        public Item Item { get; set; }
    }
}
