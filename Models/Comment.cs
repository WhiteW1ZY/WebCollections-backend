using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public User Owner { get; set; }

        public string Text { get; set; }

        public Item Item { get; set; }
    }
}
