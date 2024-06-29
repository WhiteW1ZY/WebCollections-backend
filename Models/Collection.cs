using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Collection
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public User Owner { get; set; }

        public string Image { get;set; }

        public List<Item> Items { get; set; }
    }
}
